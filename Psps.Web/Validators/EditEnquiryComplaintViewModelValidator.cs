using FluentValidation;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using Psps.Web.ViewModels.SystemMessages;

namespace Psps.Web.Validators
{
    public class EditEnquiryComplaintViewModelValidator : AbstractValidator<EditEnquiryComplaintViewModel>
    {
        private ILookupService _lookupService;

        public EditEnquiryComplaintViewModelValidator(IMessageService messageService, ILookupService lookupService)
        {
            this._lookupService = lookupService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var complaintDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.ComplaintDateEqualOrEarlierThanFirstComplaintDate);
            var OtherActivityConcernedMustBeInputtedMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.OtherActivityConcernedMustBeInputted);
            var withholdingStartDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdingStartDateMustBeInputted);
            var withholdingEndDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdingEndDateMustBeInputted);
            var endDateMustAfterStartDateMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdinEndDateMustAfterStartDate);

            RuleSet("Update", () =>
            {
                //RuleFor(x => x.FirstComplaintDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintRecordType).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintSource).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ActivityConcern).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplainantName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ProcessStatusId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OtherActivityConcern).Must(OtherActivityConcernedMustBeInputted).When(x => !string.IsNullOrEmpty(x.ActivityConcern)).WithMessage(OtherActivityConcernedMustBeInputtedMsg);

                RuleFor(x => x.OrgRef).NotEmpty().When(x => x.WithholdingListIndicator).WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintDate).LessThanOrEqualTo(x => x.FirstComplaintDate).When(x => x.ComplaintDate != null && x.FirstComplaintDate != null).WithMessage(complaintDateErrorMsg);
                RuleFor(x => x.WithholdingBeginDate).NotEmpty().When(x => x.WithholdingListIndicator).WithMessage(withholdingStartDateErrorMsg);
                RuleFor(x => x.WithholdingBeginDate).LessThanOrEqualTo(x => x.WithholdingEndDate).When(x => x.WithholdingBeginDate != null && x.WithholdingEndDate != null).WithMessage(endDateMustAfterStartDateMsg);
            });

            RuleSet("CreateComplaintAttachment", () =>
            {
                RuleFor(x => x.FileName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.FileDescription).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentFile).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdateComplaintAttachment", () =>
            {
                RuleFor(x => x.FileName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.FileDescription).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.AttachmentFile).NotEmpty().WithMessage(mandatoryMessage);
            });
        }

        private bool OtherActivityConcernedMustBeInputted(EditEnquiryComplaintViewModel model, string OtherActivityConcern)
        {
            var description = _lookupService.GetDescription(LookupType.ComplaintActivityConcern, model.ActivityConcern);
            if (description.Equals("others", System.StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(OtherActivityConcern))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
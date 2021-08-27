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
    public class EnquiryComplaintSearchViewModelValidator : AbstractValidator<EnquiryComplaintSearchViewModel>
    {
        private ILookupService _lookupService;

        public EnquiryComplaintSearchViewModelValidator(IMessageService messageService, ILookupService lookupService)
        {
            this._lookupService = lookupService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var complaintDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.ComplaintDateEqualOrEarlierThanFirstComplaintDate);
            var withholdingStartDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdingStartDateMustBeInputted);
            //var withholdingEndDateErrorMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdingEndDateMustBeInputted);
            var OtherActivityConcernedMustBeInputtedMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.OtherActivityConcernedMustBeInputted);
            var endDateMustAfterStartDateMsg = messageService.GetMessage(SystemMessage.Error.EnquiryComplaint.WithholdinEndDateMustAfterStartDate);
            var mustBeNumeric = messageService.GetMessage(SystemMessage.Error.Numeric);

            RuleSet("Create", () =>
            {
                //RuleFor(x => x.ConcernedOrgId).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FirstComplaintDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.RecordTypeId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintSourceId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ActivityConcernId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplaintDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ComplainantName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ProcessStatusId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OtherActivityConcern).Must(OtherActivityConcernedMustBeInputted).When(x => !string.IsNullOrEmpty(x.ActivityConcernId)).WithMessage(OtherActivityConcernedMustBeInputtedMsg);

                RuleFor(x => x.ComplaintDate).LessThanOrEqualTo(x => x.FirstComplaintDate).When(x => x.ComplaintDate != null && x.FirstComplaintDate != null).WithMessage(complaintDateErrorMsg);
                RuleFor(x => x.WithholdingBeginDate).NotEmpty().When(x => x.WithholdingListIndicator).WithMessage(withholdingStartDateErrorMsg);
                RuleFor(x => x.WithholdingBeginDate).LessThanOrEqualTo(x => x.WithholdingEndDate).When(x => x.WithholdingBeginDate != null && x.WithholdingEndDate != null).WithMessage(endDateMustAfterStartDateMsg);
            });
        }

        private bool OtherActivityConcernedMustBeInputted(EnquiryComplaintSearchViewModel model, string OtherActivityConcern)
        {
            var description = _lookupService.GetDescription(LookupType.ComplaintActivityConcern, model.ActivityConcernId);
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
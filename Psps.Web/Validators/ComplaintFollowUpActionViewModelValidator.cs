using FluentValidation;
using Psps.Core.Helper;
using Psps.Models.Domain;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using Psps.Web.ViewModels.SystemMessages;

namespace Psps.Web.Validators
{
    public class ComplaintFollowUpActionViewModelValidator : AbstractValidator<ComplaintFollowUpActionViewModel>
    {
        public ComplaintFollowUpActionViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var contactDateMustEarlierThanIssueDateMsg = messageService.GetMessage(SystemMessage.Error.Compliant.ContactDateMustEarlierThanIssueDate);
            var NotAllowToSaveRecord = messageService.GetMessage(SystemMessage.Error.Compliant.NotAllowToSaveRecord);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.ComplaintMasterId).Must(ValNotAllowToSaveRecord).WithMessage(NotAllowToSaveRecord);
                RuleFor(x => x.FollowUpContactDate).NotEmpty().When(x => x.FollowUpIndicator && x.FollowUpLetterIssueDate == null).WithMessage(mandatoryMessage);
                RuleFor(x => x.FollowUpContactDate).LessThanOrEqualTo(x => x.FollowUpLetterIssueDate)
                                                   .When(x => x.FollowUpIndicator && x.FollowUpContactDate != null & x.FollowUpLetterIssueDate != null)
                                                   .WithMessage(contactDateMustEarlierThanIssueDateMsg);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.ComplaintMasterId).Must(ValNotAllowToSaveRecord).WithMessage(NotAllowToSaveRecord);
                RuleFor(x => x.FollowUpContactDate).NotEmpty().When(x => x.FollowUpIndicator && x.FollowUpLetterIssueDate == null).WithMessage(mandatoryMessage);
                RuleFor(x => x.FollowUpContactDate).LessThanOrEqualTo(x => x.FollowUpLetterIssueDate)
                                                   .When(x => x.FollowUpIndicator && x.FollowUpContactDate != null & x.FollowUpLetterIssueDate != null)
                                                   .WithMessage(contactDateMustEarlierThanIssueDateMsg);
            });
        }

        private bool ValNotAllowToSaveRecord(ComplaintFollowUpActionViewModel model, int complaintMasterId)
        {
            if ((model.FollowUpIndicator == true && model.FollowUpReportPoliceIndicator == true)
                || (model.FollowUpIndicator == true && model.FollowUpLetterActionOtherFollowUpIndicator == true)
                || (model.FollowUpReportPoliceIndicator == true && model.FollowUpLetterActionOtherFollowUpIndicator == true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
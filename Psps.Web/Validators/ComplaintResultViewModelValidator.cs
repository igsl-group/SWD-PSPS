using FluentValidation;
using Psps.Core.Common;
using Psps.Models.Domain;
using Psps.Services.ComplaintMasters;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class ComplaintResultViewModelValidator : AbstractValidator<ComplaintResultViewModel>
    {
        protected readonly IMessageService _messageService;
        protected readonly IComplaintResultService _complaintResultService;

        public ComplaintResultViewModelValidator(IMessageService messageService, IComplaintResultService complaintResultService)
        {
            this._messageService = messageService;
            this._complaintResultService = complaintResultService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var notCompResultDistinctMsg = messageService.GetMessage(SystemMessage.Error.Compliant.NotCompResultDistinctMsg);

            RuleSet("default", () =>
            {
                RuleFor(x => x.NonComplianceNature).NotNull().WithMessage(mandatoryMessage);

                RuleFor(x => x.ComplaintMasterId).Must(DistinctCompResult).When(x => x.ComplaintMasterId != null).WithMessage(notCompResultDistinctMsg);
            });

            RuleSet("Update", () =>
            {
            });
        }

        private bool DistinctCompResult(ComplaintResultViewModel model, int? ComplaintMasterId)
        {
            return _complaintResultService.IsDistinctByCmIdAndResult((int)ComplaintMasterId, model.ComplaintResultId, model.NonComplianceNature);
        }
    }
}
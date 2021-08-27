using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Services.Lookups;
using Psps.Web.ViewModels.SystemMessages;
using Psps.Web.ViewModels.Organisation;
using Psps.Core.Helper;

namespace Psps.Web.Validators
{
    public class OrgNameChangeHistoryViewModelValidator : AbstractValidator<OrgNameChangeHistoryViewModel>
    {

        public OrgNameChangeHistoryViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            RuleSet("Update", () =>
            {
                RuleFor(x => x.OrgNameChangeDate).NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            });

        }

    }
}
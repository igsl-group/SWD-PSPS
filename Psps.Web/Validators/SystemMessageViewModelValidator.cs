using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.SystemMessages;

namespace Psps.Web.Validators
{
    public class SystemMessageViewModelValidator : AbstractValidator<SystemMessageViewModel>
    {
        public SystemMessageViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(mandatoryMessage);

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(mandatoryMessage);
        }
    }
}
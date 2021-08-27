using FluentValidation;
using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.ViewModels.Account;

namespace Psps.Web.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Login", () =>
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.Password).NotEmpty().WithMessage(mandatoryMessage);
            });
        }
    }
}
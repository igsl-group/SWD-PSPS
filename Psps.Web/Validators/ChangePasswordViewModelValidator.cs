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
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        protected readonly IUserService _userService;
        protected readonly string currentUserId;

        public ChangePasswordViewModelValidator(IMessageService messageService, IParameterService parameterService, IUserService userService, IWorkContext workContext)
        {
            this._userService = userService;
            currentUserId = workContext.CurrentUser != null ? workContext.CurrentUser.UserId : null;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var lengthMsg = messageService.GetMessage(SystemMessage.Error.Length);
            var passwordCompositionInvalidMsg = messageService.GetMessage(SystemMessage.Error.User.PasswordCompositionInvalid);
            var passwordHistoryCrashMsg = messageService.GetMessage(SystemMessage.Error.User.PasswordHistoryCrash);

            string regexLetterNumber = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{0,}$";

            RuleSet("ChangePassword", () =>
            {
                RuleFor(x => x.OldPassword).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.NewPassword).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(mandatoryMessage);

                var notMatchMsg = messageService.GetMessage(SystemMessage.Error.User.NotMatchBothPasswords);
                RuleFor(x => x.ConfirmNewPassword).Must(MatchWithBothNewPasswords).When(x => !string.IsNullOrEmpty(x.ConfirmNewPassword)).WithMessage(notMatchMsg);

                var oldPasswordIncorrectMsg = messageService.GetMessage(SystemMessage.Error.User.OldPasswordIncorrect);
                RuleFor(x => x.OldPassword).Must(ValidateOldPassword).When(x => !string.IsNullOrEmpty(x.OldPassword)).WithMessage(oldPasswordIncorrectMsg);

                RuleFor(x => x.NewPassword).Length(8, 20).WithMessage(lengthMsg);
                RuleFor(x => x.NewPassword).Matches(regexLetterNumber).WithMessage(passwordCompositionInvalidMsg);
                RuleFor(x => x.NewPassword).Must(NotExistsInPasswordHistory).WithMessage(passwordHistoryCrashMsg);
            });
        }

        public bool MatchWithBothNewPasswords(ChangePasswordViewModel model, string confirmNewPassword)
        {
            var newPassword = model.NewPassword;

            if (newPassword.IsNotNullOrEmpty() && confirmNewPassword.IsNotNullOrEmpty())
            {
                if (!newPassword.Equals(confirmNewPassword))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidateOldPassword(ChangePasswordViewModel model, string oldPassword)
        {
            return this._userService.ValidateUser(currentUserId ?? model.UserId, oldPassword) == UserLoginResults.Successful;
        }

        public bool NotExistsInPasswordHistory(ChangePasswordViewModel model, string newPassword)
        {
            return !this._userService.ExistsInPasswordHistory(currentUserId ?? model.UserId, newPassword);
        }
    }
}
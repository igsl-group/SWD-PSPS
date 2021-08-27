using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;

namespace Psps.Web.Validators
{
    public class ComplaintMasterViewModelValidator : AbstractValidator<ComplaintMasterViewModel>
    {
        protected readonly IUserService _userService;

        public ComplaintMasterViewModelValidator(IMessageService messageService)
        {

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleSet("Create", () =>
            {

            });

            RuleSet("Update", () =>
            {
                
            });
        }

        private bool UniqueUserId(UserViewModel model, string userId)
        {
            return _userService.GetUserById(userId) == null;
        }
    }
}
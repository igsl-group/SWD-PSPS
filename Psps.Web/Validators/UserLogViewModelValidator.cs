using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.SystemMessages;
using Psps.Web.ViewModels.UserLogs;

namespace Psps.Web.Validators
{
    public class UserLogViewModelValidator : AbstractValidator<UserLogViewModel>
    {
        public UserLogViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

         
        }
    }
}
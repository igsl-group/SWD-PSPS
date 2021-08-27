using FluentValidation;
using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class HomeViewModelValidator : AbstractValidator<HomeViewModel>
    {
        protected readonly IUserService _userService;
        protected readonly IAclService _aclService;

        public HomeViewModelValidator(IMessageService messageService, IParameterService parameterService, IUserService userService, IAclService aclService)
        {
            _userService = userService;
            _aclService = aclService;

            //var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("ChangePost", () =>
            {
                RuleFor(x => x.ChangedPostId).Must(ValidPostForChange).WithMessage("Invalid post for acting");
            });
        }

        public bool ValidPostForChange(HomeViewModel model, string changedPostId)
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var postsAllowed = _aclService.GetPostToBeActed(currentUser.UserId, DateTime.Today);

            return changedPostId.Equals(currentUser.OriginalPostIdIfActed) || (postsAllowed != null && postsAllowed.Contains(changedPostId));
        }
    }
}
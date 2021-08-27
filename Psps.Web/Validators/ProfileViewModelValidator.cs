using FluentValidation;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
    public class ProfileViewModelValidator : AbstractValidator<ProfileViewModel>
    {
        protected readonly IUserService _userService;
        protected readonly IActingService _actingService;

        public ProfileViewModelValidator(IMessageService messageService, IUserService userService, IActingService actingService)
        {
            this._userService = userService;
            this._actingService = actingService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.AssignTo).NotEmpty().WithMessage(mandatoryMessage);

                var invalidUserMsg = messageService.GetMessage(SystemMessage.Error.User.InvalidUserError);
                RuleFor(x => x.AssignTo).Must(ValidateUserIsValid).When(x => x.AssignTo.IsNotNullOrEmpty()).WithMessage(invalidUserMsg);

                var samePostErrorMsg = messageService.GetMessage(SystemMessage.Error.User.SamePostError);
                RuleFor(x => x.AssignTo).Must(ValidateAssignToOtherPost).When(x => x.AssignTo.IsNotNullOrEmpty()).WithMessage(samePostErrorMsg);

                RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.EffectiveTo).NotEmpty().WithMessage(mandatoryMessage);
                var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
                RuleFor(x => x.EffectiveFrom).Must(ValidateFromDateEarlierThanToDate).When(x => x.EffectiveFrom != null && x.EffectiveTo != null).WithMessage(fromDateLaterThanToDateMsg);

                var isAssignedErrorMsg = messageService.GetMessage(SystemMessage.Error.User.UserIsAssignedError);
                RuleFor(x => x.AssignTo).Must(ValidateNotAssigned).When(x => x.AssignTo.IsNotNullOrEmpty() && x.EffectiveFrom != null && x.EffectiveTo != null).WithMessage(isAssignedErrorMsg);
            });

            RuleSet("EditUser", () =>
            {
                RuleFor(x => x.EngUserName).NotEmpty().WithMessage(mandatoryMessage);
            });
        }

        private bool ValidateUserIsValid(ProfileViewModel model, string assignToUserId)
        {
            var user = _userService.GetUserById(assignToUserId);
            return user.IsActive;
        }

        private bool ValidateAssignToOtherPost(ProfileViewModel model, string assignToUserId)
        {
            var loginUserId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            return !assignToUserId.Equals(loginUserId);
        }

        private bool ValidateFromDateEarlierThanToDate(ProfileViewModel model, DateTime? fromDate)
        {
            return !(fromDate >= model.EffectiveTo);
        }

        private bool ValidateNotAssigned(ProfileViewModel model, string assignToUserId)
        {
            var loginUserId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var user = _userService.GetUserById(loginUserId);
            return !this._actingService.ValidateIsAssigned(model.ActingId.HasValue ? model.ActingId.Value : -1, user.Post.PostId, model.EffectiveFrom.Value, model.EffectiveTo.Value);
        }
    }
}
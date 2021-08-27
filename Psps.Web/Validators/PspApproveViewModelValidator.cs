using FluentValidation;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.PspApprove;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
    public class PspApproveViewModelValidator : AbstractValidator<PspApproveViewModel>
    {
        protected readonly IUserService _userService;
        protected readonly IActingService _actingService;

        public PspApproveViewModelValidator(IMessageService messageService, IUserService userService, IActingService actingService)
        {
            this._userService = userService;
            this._actingService = actingService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
            {
                //RuleFor(x => x.AssignTo).NotEmpty().WithMessage(mandatoryMessage);

                //var invalidUserMsg = messageService.GetMessage(SystemMessage.Error.User.InvalidUserError);

                //RuleFor(x => x.AssignTo).Must(ValidateUserIsValid).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(invalidUserMsg);

                //var samePostErrorMsg = messageService.GetMessage(SystemMessage.Error.User.SamePostError);
                //RuleFor(x => x.AssignTo).Must(ValidateAssignToOtherPost).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(samePostErrorMsg);

                //var invalidDateMsg = messageService.GetMessage(SystemMessage.Error.InvalidDate);
                //RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.EffectiveFrom).Must(ValidateIsValidDate).When(x => !string.IsNullOrEmpty(x.EffectiveFrom)).WithMessage(invalidDateMsg);

                //RuleFor(x => x.EffectiveTo).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.EffectiveTo).Must(ValidateIsValidDate).When(x => !string.IsNullOrEmpty(x.EffectiveTo)).WithMessage(invalidDateMsg);

                //var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
                //RuleFor(x => x.EffectiveFrom).Must(ValidateFromDateEarlierThanToDate).When(x => !string.IsNullOrEmpty(x.EffectiveFrom)).WithMessage(fromDateLaterThanToDateMsg);

                //var isAssignedErrorMsg = messageService.GetMessage(SystemMessage.Error.User.UserIsAssignedError);
                //RuleFor(x => x.AssignTo).Must(ValidateNotAssigned).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(isAssignedErrorMsg);
            });

            RuleSet("update", () =>
            {
                //RuleFor(x => x.EngUserName).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("ApproveRecommendEvent", () =>
            {
                RuleFor(x => x.PspRecommendApproveEventsViewModel.PermitIssueDate).NotEmpty().WithMessage(mandatoryMessage);
            });


        }
    }
}
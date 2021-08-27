using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.FlagDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class FlagDayApproveViewModelValidator : AbstractValidator<FlagDayApproveViewModel>
    {
        protected readonly IMessageService _messageService;

        public FlagDayApproveViewModelValidator(IMessageService messageService)
        {
            this._messageService = messageService;
            RuleSet("CreateFdApprovHist", () =>
            {
                RuleFor(x => x.ApprovalDate).NotNull().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));

                RuleFor(x => x.ApprovalDate).Must(ifDateLtToday).WithMessage(_messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate));
            });
        }

        protected bool ifDateLtToday(DateTime? varDate)
        {
            if (varDate < DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}
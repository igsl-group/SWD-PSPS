using FluentValidation;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.PSP;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
    public class PspEventViewModelValidator : AbstractValidator<PspEventViewModel>
    {
        protected readonly IMessageService _messageService;

        public PspEventViewModelValidator(IMessageService messageService)
        {
            this._messageService = messageService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            RuleSet("CreatePspEvent", () =>
            {
                RuleForCreate();
            });
        }

        protected void RuleForCreate()
        {
            //RuleFor(x => x.EventStartDate).NotEmpty().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));
        }
    }
}
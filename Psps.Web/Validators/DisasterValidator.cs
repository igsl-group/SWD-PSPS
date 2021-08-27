using FluentValidation;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Disaster;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Disaster;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
    public class DisasterViewModelValidator : AbstractValidator<DisasterViewModel>
    {
        protected readonly IDisasterMasterService _disasterMasterService;
        protected readonly IDisasterStatisticsService _disasterStatisticsService;

        public DisasterViewModelValidator(IMessageService messageService, IDisasterMasterService disasterMasterService, IDisasterStatisticsService disasterStatisticsService)
        {
            _disasterMasterService = disasterMasterService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var disasterNameUniqueMsg = messageService.GetMessage(SystemMessage.Error.DisasterMaster.DisasterNameUnique);

            var beginDateLaterThanEndDateMsg = messageService.GetMessage(SystemMessage.Error.DisasterMaster.BeginDateLaterThanEndDate);
            var beginDateLaterThanTodayMsg = messageService.GetMessage(SystemMessage.Error.DisasterMaster.BeginDateLaterThanToday);
            var disasterDateLaterThanBeginDateMsg = messageService.GetMessage(SystemMessage.Error.DisasterMaster.DisasterDateLaterThanBeginDate);
            var disasterDateLaterThanTodayMsg = messageService.GetMessage(SystemMessage.Error.DisasterMaster.DisasterDateLaterThanToday);

            //default rule
            RuleFor(x => x.DisasterName)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.BeginDate)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

            //rule for create disaster
            RuleSet("CreateDisaster", () =>
            {
                RuleFor(x => x.DisasterName)
                    .Must(ValidateUniqueDisasterName).When(x => !String.IsNullOrEmpty(x.DisasterName)).WithMessage(disasterNameUniqueMsg);
                RuleFor(x => x.BeginDate)
                    .Must(ValidateBeginDateLessThanEndDate).WithMessage(beginDateLaterThanEndDateMsg, x => x.BeginDate, x => x.EndDate);
                RuleFor(x => x.BeginDate)
                    .Must(ValidateBeginDateLaterThanToday).WithMessage(beginDateLaterThanTodayMsg, x => DateTime.Today, x => x.BeginDate);
                RuleFor(x => x.DisasterDate)
                    .Must(ValidateDisasterDateLaterThanBeginDate).WithMessage(disasterDateLaterThanBeginDateMsg, x => x.DisasterDate, x => x.BeginDate);
                RuleFor(x => x.DisasterDate)
                    .Must(ValidateDisasterDateLaterThanToday).WithMessage(disasterDateLaterThanTodayMsg, x => DateTime.Today, x => x.DisasterDate);
            });

            //rulke for update disaster
            RuleSet("UpdateDisaster", () =>
            {
                RuleFor(x => x.DisasterName)
                    .Must(ValidateUniqueDisasterNameByUpdate).When(x => !String.IsNullOrEmpty(x.DisasterName)).WithMessage(disasterNameUniqueMsg);
                RuleFor(x => x.BeginDate)
                    .Must(ValidateBeginDateLessThanEndDate).WithMessage(beginDateLaterThanEndDateMsg, x => x.BeginDate, x => x.EndDate);
                RuleFor(x => x.BeginDate)
                    .Must(ValidateBeginDateLaterThanToday).WithMessage(beginDateLaterThanTodayMsg, x => DateTime.Today, x => x.BeginDate);
                RuleFor(x => x.DisasterDate)
                    .Must(ValidateDisasterDateLaterThanBeginDate).WithMessage(disasterDateLaterThanBeginDateMsg, x => x.DisasterDate, x => x.BeginDate);
                RuleFor(x => x.DisasterDate)
                    .Must(ValidateDisasterDateLaterThanToday).WithMessage(disasterDateLaterThanTodayMsg, x => DateTime.Today, x => x.DisasterDate);
            });
        }

        protected bool ValidateBeginDateLessThanEndDate(DisasterViewModel model, DateTime beginDate)
        {
            if (beginDate > model.EndDate)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateBeginDateLaterThanToday(DateTime beginDate)
        {
            if (beginDate > DateTime.Today)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateDisasterDateLaterThanBeginDate(DisasterViewModel model, DateTime? disasterDate)
        {
            if (disasterDate > model.BeginDate)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateDisasterDateLaterThanToday(DateTime? disasterDate)
        {
            if (disasterDate > DateTime.Today)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateUniqueDisasterName(string disasterName)
        {
            return _disasterMasterService.IsUniqueDisasterName(disasterName);
        }

        protected bool ValidateUniqueDisasterNameByUpdate(DisasterViewModel model, string disasterName)
        {
            return _disasterMasterService.IsUniqueDisasterName(model.DisasterMasterId.Value, disasterName);
        }
    }
}
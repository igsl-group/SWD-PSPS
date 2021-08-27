using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Disaster;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Disaster;
using Psps.Web.ViewModels.DisasterStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class DisasterStatisticsViewModelValidator : AbstractValidator<DisasterStatisticsViewModel>
    {
        protected readonly IDisasterMasterService _disasterMasterService;
        protected readonly IDisasterStatisticsService _disasterStatisticsService;

        public DisasterStatisticsViewModelValidator(IMessageService messageService, IDisasterMasterService disasterMasterService, IDisasterStatisticsService disasterStatisticsService)
        {
            _disasterMasterService = disasterMasterService;
            _disasterStatisticsService = disasterStatisticsService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var reportingDateLaterThanTodayMsg = messageService.GetMessage(SystemMessage.Error.DisasterStatistics.ReportingDateLaterThanToday);
            var reportingDateWithinDisasterPeriodMsg = messageService.GetMessage(SystemMessage.Error.DisasterStatistics.ReportingDateWithinDisasterPeriod);
            var reportingDateWithDisasterRecordMsg = messageService.GetMessage(SystemMessage.Error.DisasterStatistics.ReportingDateWithDisasterRecord);

            //default rule
            RuleFor(x => x.RecordPostId)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.RecordDate)
                .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.DisasterMasterId)
                .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
//            RuleFor(x => x.DisasterName)
//                   .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));


            //rule for create disaster
            RuleSet("CreateDisasterStatistics", () =>
            {
                RuleFor(x => x.RecordDate)
                .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

                RuleFor(x => x.RecordDate).Must(ValidateRecordDateLaterThanToday).WithMessage(reportingDateLaterThanTodayMsg);

                RuleFor(x => x.RecordDate).Must(ValidateRecordDateWithinMasterBeginEndDate).WithMessage(reportingDateWithinDisasterPeriodMsg);

                RuleFor(x => x.DisasterNameForPopUp)
                    .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

                RuleFor(x => x.RecordDate).Must(ValidateReportingDateWithDisasterRecord).WithMessage(reportingDateWithDisasterRecordMsg);

            });

            //rulke for update disaster
            RuleSet("UpdateDisasterStatistics", () =>
            {
                RuleFor(x => x.DisasterStatisticsId)
                        .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            });
        }

        protected bool ValidateRecordDateLaterThanToday(DisasterStatisticsViewModel model, DateTime recordDate)
        {
            if (recordDate.Date > DateTime.Today.Date)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateRecordDateWithinMasterBeginEndDate(DisasterStatisticsViewModel model, DateTime recordDate)
        {
            var disasterMaster = _disasterMasterService.GetDisasterMasterById(Convert.ToInt32(model.DisasterMasterId));

            if (disasterMaster.EndDate != null)
            {
                if (recordDate >= disasterMaster.BeginDate.Date && recordDate <= disasterMaster.EndDate.Value.Date)
                {
                    return true;
                }
            }
            else
            {
                if (recordDate >= disasterMaster.BeginDate.Date)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool ValidateReportingDateWithDisasterRecord(DisasterStatisticsViewModel model, DateTime recordDate)
        {
            return _disasterStatisticsService.GetDisasterStatisticsByPostIdRecordDate(model.DisasterMasterId,model.RecordPostId,model.RecordDate.Date);
        }
    }
}
using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.FlagDay;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class FlagDayReportViewModelValidator : AbstractValidator<FlagDayReportViewModel>
    {
        public FlagDayReportViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);

            RuleSet("R2", () =>
            {
                RuleFor(x => x.R2_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R2_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R2_YearTo).GreaterThanOrEqualTo(x => x.R2_YearFrom).When(x => x.R2_YearFrom != null && x.R2_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R3", () =>
            {
                RuleFor(x => x.R3FromYear).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R3ToYear).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R3ToYear).GreaterThanOrEqualTo(x => x.R3FromYear).When(x => x.R3FromYear != null && x.R3ToYear != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R4", () =>
            {
                RuleFor(x => x.R4_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R4_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R4_YearTo).GreaterThanOrEqualTo(x => x.R4_YearFrom).When(x => x.R4_YearFrom != null && x.R4_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R24", () =>
            {
                RuleFor(x => x.R24_Year).NotNull().WithMessage(mandatoryMessage);
            });
        }
    }
}
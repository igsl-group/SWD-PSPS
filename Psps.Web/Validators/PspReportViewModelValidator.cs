using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.PSP;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class PspReportViewModelValidator : AbstractValidator<PspReportViewModel>
    {
        public PspReportViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);

            RuleSet("R1", () =>
            {
                RuleFor(x => x.R1_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R1_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R1_YearTo).GreaterThanOrEqualTo(x => x.R1_YearFrom).When(x => x.R1_YearFrom != null && x.R1_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R2", () =>
            {
                RuleFor(x => x.R2_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R2_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R2_YearTo).GreaterThanOrEqualTo(x => x.R2_YearFrom).When(x => x.R2_YearFrom != null && x.R2_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R4", () =>
            {
                RuleFor(x => x.R4_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R4_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R4_YearTo).GreaterThanOrEqualTo(x => x.R4_YearFrom).When(x => x.R4_YearFrom != null && x.R4_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("R5", () =>
            {
                RuleFor(x => x.R5_Year).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("R6", () =>
            {
                RuleFor(x => x.R6_DisasterId).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("R21", () =>
            {
                RuleFor(x => x.R21_Years).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("R27", () =>
            {
                RuleFor(x => x.R27_YearFrom).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R27_YearTo).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.R27_YearTo).GreaterThanOrEqualTo(x => x.R27_YearFrom).When(x => x.R27_YearFrom != null && x.R27_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });

            RuleSet("Raw2", () =>
            {
                RuleFor(x => x.Raw2_YearTo).GreaterThanOrEqualTo(x => x.Raw2_YearFrom).When(x => x.Raw2_YearFrom != null && x.Raw2_YearTo != null).WithMessage(fromDateLaterThanToDateMsg);
            });
        }
    }
}
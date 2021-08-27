using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Enquiry;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class EnquiryReportViewModelValidator : AbstractValidator<EnquiryReportViewModel>
    {
        public EnquiryReportViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var FromDateLaterThanToDate = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate); ;

            RuleSet("R8", () =>
            {
                RuleFor(x => x.R8_DateFrom).LessThan(x => x.R8_DateTo).When(x => x.R8_DateFrom != null && x.R8_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R9", () =>
            {
                RuleFor(x => x.R9_DateFrom).LessThan(x => x.R9_DateTo).When(x => x.R9_DateFrom != null && x.R9_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R10", () =>
            {
                RuleFor(x => x.R10_DateFrom).LessThan(x => x.R10_DateTo).When(x => x.R10_DateFrom != null && x.R10_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R11", () =>
            {
                RuleFor(x => x.R11_DateFrom).LessThan(x => x.R11_DateTo).When(x => x.R11_DateFrom != null && x.R11_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R12", () =>
            {
                RuleFor(x => x.R12_DateFrom).LessThan(x => x.R12_DateTo).When(x => x.R12_DateFrom != null && x.R12_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R13", () =>
            {
                RuleFor(x => x.R13_DateFrom).LessThan(x => x.R13_DateTo).When(x => x.R13_DateFrom != null && x.R13_DateTo != null).WithMessage(FromDateLaterThanToDate);
            });

            RuleSet("R14", () =>
            {
                RuleFor(x => x.R14_ComplaintSource).NotEmpty().WithMessage(mandatoryMessage);
            });

            //RuleSet("R15", () =>
            //{
            //    RuleFor(x => x.R15_DateFrom).LessThan(x => x.R15_DateTo).When(x => x.R15_DateFrom != null && x.R15_DateTo != null).WithMessage(FromDateLaterThanToDate);
            //});
        }
    }
}
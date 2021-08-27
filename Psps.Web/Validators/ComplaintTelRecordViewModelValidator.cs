using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class ComplaintTelRecordViewModelValidator : AbstractValidator<ComplaintTelRecordViewModel>
    {
        public ComplaintTelRecordViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.TelRecordComplaintDate).NotNull().WithMessage(mandatoryMessage);
                //RuleFor(x => x.ComplaintTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.TelRecordComplainantName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.ComplainantTelNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingLocation).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OfficerName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OfficerPost).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.TelRecordComplaintDate).NotNull().WithMessage(mandatoryMessage);
                //RuleFor(x => x.ComplaintTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.TelRecordComplainantName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.ComplainantTelNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.FundRaisingLocation).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OfficerName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OfficerPost).NotEmpty().WithMessage(mandatoryMessage);
            });
        }
    }
}
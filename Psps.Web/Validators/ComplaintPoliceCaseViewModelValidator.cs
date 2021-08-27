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
    public class ComplaintPoliceCaseViewModelValidator : AbstractValidator<ComplaintPoliceCaseViewModel>
    {
        public ComplaintPoliceCaseViewModelValidator(IMessageService messageService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.PoliceCaseConcernOrgRef).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseFundRaisingDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseFundRaisingTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseFundRaisingLocation).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseConvictedPersonName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseCourtRefNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseCourtHearingDate).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.PoliceCaseConcernOrgRef).NotEmpty().WithMessage(mandatoryMessage);

                //RuleFor(x => x.PoliceCaseFundRaisingDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseFundRaisingTime).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseFundRaisingLocation).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseConvictedPersonName).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseCourtRefNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.PoliceCaseCourtHearingDate).NotEmpty().WithMessage(mandatoryMessage);
            });
        }
    }
}
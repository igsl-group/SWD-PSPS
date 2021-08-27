using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.LegalAdvices;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.LegalAdvice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class LegalAdviceViewModelValidator : AbstractValidator<LegalAdviceViewModel>
    {
        protected readonly ILegalAdviceService _legalAdviceService;

        public LegalAdviceViewModelValidator(IMessageService messageService, ILegalAdviceService legalAdviceService)
        {
            this._legalAdviceService = legalAdviceService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
           {
               RuleFor(x => x.LegalAdviceTypeHeadId).NotEmpty().WithMessage(mandatoryMessage);

               RuleFor(x => x.LegalAdviceTypeId).Must(TypeIncomplete).When(x => string.IsNullOrEmpty(x.LegalAdviceTypeId)).WithMessage(mandatoryMessage);

               RuleFor(x => x.LegalAdviceDescription).NotEmpty().WithMessage(mandatoryMessage);
               RuleFor(x => x.PartNum).NotEmpty().WithMessage(mandatoryMessage);
               RuleFor(x => x.EnclosureNum).NotEmpty().WithMessage(mandatoryMessage);
               RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage(mandatoryMessage);
               RuleFor(x => x.PSPRequiredId).NotEmpty().WithMessage(mandatoryMessage);

               RuleFor(x => x.RelatedLegalAdviceTypeId).Must(RelatedTypeIncomplete).When(x => string.IsNullOrEmpty(x.RelatedLegalAdviceTypeId)).WithMessage(mandatoryMessage);

           });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.LegalAdviceTypeHeadId).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.LegalAdviceTypeId).Must(TypeIncomplete).When(x => string.IsNullOrEmpty(x.LegalAdviceTypeId)).WithMessage(mandatoryMessage);

                RuleFor(x => x.LegalAdviceDescription).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.PartNum).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.EnclosureNum).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.PSPRequiredId).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.RelatedLegalAdviceTypeId).Must(RelatedTypeIncomplete).When(x => string.IsNullOrEmpty(x.RelatedLegalAdviceTypeId)).WithMessage(mandatoryMessage);

            });
        }

        private bool TypeIncomplete(LegalAdviceViewModel model, string m)
        {

            bool value = true;
            if (model.LegalAdviceTypeHeadId == "01")
            {
                value = false;
            }
            return value;
        }

        private bool RelatedTypeIncomplete(LegalAdviceViewModel model, string m)
        {

            bool value = true;
            if (model.RelatedLegalAdviceTypeId == "01")
            {
                value = false;
            }

            return value;
        }

    }
}
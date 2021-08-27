using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Lookup;

namespace Psps.Web.Validators
{
    public class LookupViewModelValidator : AbstractValidator<LookupViewModel>
    {
        private readonly ILookupService _lookupService;

        public LookupViewModelValidator(IMessageService messageService, ILookupService lookupService)
        {
            _lookupService = lookupService;
            var NotThanOrEqualOthersMsg = messageService.GetMessage(SystemMessage.Error.Lookup.NotThanOrEqualOthers);
            var ShouldThanOrEqualOthersMsg = messageService.GetMessage(SystemMessage.Error.Lookup.ShouldThanOrEqualOthers);
            RuleFor(x => x.Type)
                .NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

            //To mark the "jQuery.Validation.Unobtrusive.Native" works, we cannot use "Cascade" here
            //RuleFor(x => x.Code).Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory))
            //    .Must(UniqueLookupCode).When(x => !x.LookupId.HasValue && !string.IsNullOrEmpty(x.Code)).WithMessage(messageService.GetMessage(SystemMessage.Error.Unique));

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.Code)
                .Must(UniqueLookupCode).When(x => !string.IsNullOrEmpty(x.Code)).WithMessage(messageService.GetMessage(SystemMessage.Error.Lookup.Unique));

            RuleFor(x => x.EngDescription)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.DisplayOrder)
                .NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.DisplayOrder)
                .Must(ValMaxDisplayOrder).When(x => x.Code != "Others").WithMessage(NotThanOrEqualOthersMsg);

            RuleFor(x => x.DisplayOrder)
                .Must(ValMaxDisplayOrder).When(x => x.Code == "Others").WithMessage(ShouldThanOrEqualOthersMsg);
        }

        private bool UniqueLookupCode(LookupViewModel model, string code)
        {
            return _lookupService.IsUniqueLookupCode(model.Type,
                model.LookupId.HasValue ? model.LookupId.Value : -1,
                model.Code);
        }

        private bool ValMaxDisplayOrder(LookupViewModel model, int displayOrder)
        {
            return _lookupService.IsMaxDisplayOrder(model.Type,model.Code, model.DisplayOrder);
        }

        private bool UniqueDisplayOrder(LookupViewModel model, int displayOrder)
        {
            return _lookupService.IsUniqueDisplayOrder(model.Type,
                model.LookupId.HasValue ? model.LookupId.Value : -1,
                model.DisplayOrder);
        }
    }
}
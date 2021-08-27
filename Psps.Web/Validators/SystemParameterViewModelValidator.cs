using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.ViewModels.SystemParameters;

namespace Psps.Web.Validators
{
    public class SystemParameterViewModelValidator : AbstractValidator<SystemParameterViewModel>
    {
        private readonly IParameterService _parameterService;

        public SystemParameterViewModelValidator(IMessageService messageService, IParameterService parameterService)
        {
            this._parameterService = parameterService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(mandatoryMessage);

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(mandatoryMessage);
        }
    }
}
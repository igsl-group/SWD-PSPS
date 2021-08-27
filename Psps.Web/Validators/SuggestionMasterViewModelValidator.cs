using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Suggestion;
using System;

namespace Psps.Web.Validators
{
    public class SuggestionMasterViewModelValidator : AbstractValidator<SuggestionMasterViewModel>
    {
        private string mandatoryMessage;
        private string enclNoMustBeInputtedMsg;
        private string partNoMustBeInputtedMsg;

        public SuggestionMasterViewModelValidator(IMessageService messageService)
        {
            mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            enclNoMustBeInputtedMsg = messageService.GetMessage(SystemMessage.Error.Suggestion.EnclNoMustBeInputted);
            partNoMustBeInputtedMsg = messageService.GetMessage(SystemMessage.Error.Suggestion.PartNoMustBeInputted);

            RuleSet("Create", () =>
            {
                CreateUpdateRules();
            });

            RuleSet("Update", () =>
            {
                CreateUpdateRules();
            });

            RuleSet("CreateSuggestionAttachment", () =>
            {
                RuleFor(x => x.AttachmentName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentRemark).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentDocument).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdateSuggestionAttachment", () =>
            {
                RuleFor(x => x.AttachmentName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentRemark).NotEmpty().WithMessage(mandatoryMessage);
            });
        }

        private void CreateUpdateRules()
        {
            RuleFor(x => x.SuggestionSourceId).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.SuggestionSourceId).Must(SourceIncomplete).When(x => string.IsNullOrEmpty(x.SuggestionSourceOther)).WithMessage(mandatoryMessage);

            RuleFor(x => x.ActivityConcernId).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.ActivityConcernId).Must(ConcernedIncomplete).When(x => string.IsNullOrEmpty(x.SuggestionActivityConcernOther)).WithMessage(mandatoryMessage);

            RuleFor(x => x.NatureId).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.SuggestionDate).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.SuggestionDescription).NotEmpty().WithMessage(mandatoryMessage);

            RuleFor(x => x.PartNum).NotEmpty().When(x => !string.IsNullOrEmpty(x.EnclosureNum)).WithMessage(partNoMustBeInputtedMsg);
            RuleFor(x => x.EnclosureNum).NotEmpty().When(x => !string.IsNullOrEmpty(x.PartNum)).WithMessage(enclNoMustBeInputtedMsg);
        }

        private bool SourceIncomplete(SuggestionMasterViewModel model, string m)
        {
            bool value = true;
            if (model.SuggestionSourceId == "08")
            {
                value = false;
            }
            return value;
        }

        private bool ConcernedIncomplete(SuggestionMasterViewModel model, string m)
        {
            bool value = true;
            if (model.ActivityConcernId == "Others")
            {
                value = false;
            }
            return value;
        }

        private bool FileNoIncomplete(SuggestionMasterViewModel model, string m)
        {
            bool flag = true;
            if (String.IsNullOrEmpty(m))
            {
                flag = false;
            }
            return flag;
        }
    }
}
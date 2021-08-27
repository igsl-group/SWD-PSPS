using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.Suggestions;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Suggestion;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class SuggestionDocViewModelValidator : AbstractValidator<SuggestionDocViewModel>
    {
        protected readonly ISuggestionDocService _suggestionDocService;

        public SuggestionDocViewModelValidator(IMessageService messageService, ISuggestionDocService suggestionDocService)
        {
            _suggestionDocService = suggestionDocService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.DocNum).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Version).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Description).NotEmpty().WithMessage(mandatoryMessage);
            RuleSet("CreateSuggestionDoc", () =>
            {
                RuleFor(x => x.DocNum).Must(UniqueDocNum).When(x => !string.IsNullOrEmpty(x.DocNum)).WithMessage(uniqueMessage);
                RuleFor(x => x.Version).Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.File)
                   .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Suggestion.FileFormat));
            });

            RuleSet("NewVersion", () =>
            {
                RuleFor(x => x.File)
                      .NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.Version)
                    .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);

                RuleFor(x => x.File)
                     .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Suggestion.FileFormat));
            });

            RuleSet("UpdateVersion", () =>
            {
                RuleFor(x => x.Version)
                .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File)
                    .Must(FileFormat).When(x => x.File != null).WithMessage(messageService.GetMessage(SystemMessage.Error.Suggestion.FileFormat));
            });
        }

        private bool UniqueVersion(SuggestionDocViewModel model, string version)
        {
            return this._suggestionDocService.IsUniqueSuggestionDocVersion(model.SuggestionDocId, model.DocNum, version);
        }

        private bool UniqueDocNum(SuggestionDocViewModel model, string name)
        {
            return _suggestionDocService.IsUniqueSuggestionDocNum(model.SuggestionDocId, model.DocNum);
        }

        private bool FileFormat(SuggestionDocViewModel model, HttpPostedFileBase fileName)
        {
            if (model.File != null)
            {
                return ".docx".Equals(Path.GetExtension(model.File.FileName));
            }
            else
            {
                return false;
            }
        }
    }
}
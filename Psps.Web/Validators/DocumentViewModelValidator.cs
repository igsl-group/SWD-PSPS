using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.DocumentLibrary;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.DocumentLibraries;

namespace Psps.Web.Validators
{
    public class DocumentViewModelValidator : AbstractValidator<DocumentViewModel>
    {
        protected readonly IMessageService _messageService;
        protected readonly IDocumentService _documentService;

        public DocumentViewModelValidator(IMessageService messageService, IDocumentService documentService)
        {
            _messageService = messageService;
            _documentService = documentService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(mandatoryMessage);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.Name)
                    .Must(UniqueName).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(uniqueMessage);

                RuleFor(x => x.Document)
                    .NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Name)
                    .Must(UniqueNameForUpdate).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(uniqueMessage);
            });
        }

        private bool UniqueName(DocumentViewModel model, string name)
        {
            return _documentService.IsUniqueDocumentName(model.DocumentLibraryId, name);
        }

        private bool UniqueNameForUpdate(DocumentViewModel model, string name)
        {
            return _documentService.IsUniqueDocumentName(model.DocumentLibraryId, model.DocumentId.Value, name);
        }
    }
}
using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.DocumentLibrary;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.DocumentLibraries;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
    public class DocumentLibraryViewModelValidator : AbstractValidator<DocumentLibraryViewModel>
    {
        protected readonly IMessageService _messageService;
        protected readonly IDocumentLibraryService _documentLibraryService;

        public DocumentLibraryViewModelValidator(IMessageService messageService, IDocumentLibraryService documentLibraryService)
        {
            _messageService = messageService;
            _documentLibraryService = documentLibraryService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);
            var folderCannotBeDeletedMsg = messageService.GetMessage(SystemMessage.Error.DocumentLibrary.FolderCannotBeDeleted);
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.Name)
                    .Must(UniqueName).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(uniqueMessage);
            });

            RuleSet("Delete", () =>
            {
                RuleFor(x => x.SelectedDocumentLibraryId)
                    .NotNull().WithMessage(mandatoryMessage);

                RuleFor(x => x.SelectedDocumentLibraryId)
                    .Must(IsNotContainDocumentOrSubDocumentLibrary).When(x => x.SelectedDocumentLibraryId.HasValue).WithMessage(folderCannotBeDeletedMsg);
            });
        }

        private bool UniqueName(DocumentLibraryViewModel model, string name)
        {
            return _documentLibraryService.IsUniqueDocumentLibraryName(model.SelectedDocumentLibraryId, name);
        }

        private bool IsNotContainDocumentOrSubDocumentLibrary(int? selectedDocumentLibrary)
        {
            return !_documentLibraryService.IsContainDocumentOrSubDocumentLibrary(selectedDocumentLibrary.Value);
        }
    }
}
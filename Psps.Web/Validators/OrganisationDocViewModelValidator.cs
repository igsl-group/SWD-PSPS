using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.Posts;
using Psps.Services.Suggestions;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Organisation;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class OrganisationDocViewModelValidator : AbstractValidator<OrganisationDocViewModel>
    {
        protected readonly IOrganisationDocService _organisationDocService;

        public OrganisationDocViewModelValidator(IMessageService messageService, IOrganisationDocService organisationDocService)
        {
            _organisationDocService = organisationDocService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.DocNum).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Version).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Description).NotEmpty().WithMessage(mandatoryMessage);
            RuleSet("CreateOrgDoc", () =>
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

            RuleSet("UpdateOrgVersion", () =>
            {

                RuleFor(x => x.Version)
                .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File)
                     .Must(FileFormat).When(x => x.File != null).WithMessage(messageService.GetMessage(SystemMessage.Error.Suggestion.FileFormat));
            });
        }

        private bool UniqueVersion(OrganisationDocViewModel model, string version)
        {
            return this._organisationDocService.IsUniqueOrgDocVersion(model.OrgDocId, model.DocNum, version);
        }

        private bool UniqueDocNum(OrganisationDocViewModel model, string name)
        {
            return _organisationDocService.IsUniqueOrgDocNum(model.OrgDocId, model.DocNum);
        }

        private bool FileFormat(OrganisationDocViewModel model, HttpPostedFileBase fileName)
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
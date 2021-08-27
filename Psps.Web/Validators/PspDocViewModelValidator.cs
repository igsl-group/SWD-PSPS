using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.PSPs;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.PSP;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class PspDocViewModelValidator : AbstractValidator<PspDocViewModel>
    {
        protected readonly IPspDocService _pspDocService;

        public PspDocViewModelValidator(IMessageService messageService, IPspDocService pspDocService)
        {
            _pspDocService = pspDocService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.DocNum).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Version).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Description).NotEmpty().WithMessage(mandatoryMessage);
            RuleSet("CreatePspDoc", () =>
            {
                RuleFor(x => x.DocNum).Must(UniqueDocNum).When(x => !string.IsNullOrEmpty(x.DocNum)).WithMessage(uniqueMessage);
                RuleFor(x => x.Version).Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.File)
                   .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Psps.FileFormat));
            });

            RuleSet("NewVersion", () =>
            {
                RuleFor(x => x.File)
                      .NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.Version)
                    .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);

                RuleFor(x => x.File)
                     .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Psps.FileFormat));
            });

            RuleSet("UpdateVersion", () =>
            {
                RuleFor(x => x.Version)
                .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File)
                     .Must(FileFormat).When(x => x.File != null).WithMessage(messageService.GetMessage(SystemMessage.Error.Psps.FileFormat));
            });
        }

        private bool UniqueVersion(PspDocViewModel model, string version)
        {
            return this._pspDocService.IsUniquePspDocVersion(model.PspDocId, model.DocNum, version);
        }

        private bool UniqueDocNum(PspDocViewModel model, string name)
        {
            return _pspDocService.IsUniquePspDocNum(model.PspDocId, model.DocNum);
        }

        private bool FileFormat(PspDocViewModel model, HttpPostedFileBase fileName)
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
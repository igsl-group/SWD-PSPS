using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.FlagDays;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.FlagDay;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class FlagDayDocViewModelValidator : AbstractValidator<FlagDayDocViewModel>
    {
        protected readonly IFlagDayDocService _flagDayDocService;

        public FlagDayDocViewModelValidator(IMessageService messageService, IFlagDayDocService flagDayDocService)
        {
            _flagDayDocService = flagDayDocService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.DocNum).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Version).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Description).NotEmpty().WithMessage(mandatoryMessage);
            RuleSet("CreateFlagDayDoc", () =>
            {
                RuleFor(x => x.DocNum).Must(UniqueDocNum).When(x => !string.IsNullOrEmpty(x.DocNum)).WithMessage(uniqueMessage);
                RuleFor(x => x.Version).Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.File)
                   .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.FlagDay.FileFormat));
            });

            RuleSet("NewVersion", () =>
            {
                RuleFor(x => x.File)
                      .NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.Version)
                    .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);

                RuleFor(x => x.File)
                     .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.FlagDay.FileFormat));
            });

            RuleSet("UpdateVersion", () =>
            {
                RuleFor(x => x.Version)
                .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File)
                     .Must(FileFormat).When(x => x.File != null).WithMessage(messageService.GetMessage(SystemMessage.Error.FlagDay.FileFormat));
            });
        }

        private bool UniqueVersion(FlagDayDocViewModel model, string version)
        {
            return this._flagDayDocService.IsUniqueFdDocVersion(model.FlagDayDocId, model.DocNum, version);
        }

        private bool UniqueDocNum(FlagDayDocViewModel model, string name)
        {
            return _flagDayDocService.IsUniqueFdDocNum(model.FlagDayDocId, model.DocNum);
        }

        private bool FileFormat(FlagDayDocViewModel model, HttpPostedFileBase fileName)
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
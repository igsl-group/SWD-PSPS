using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Complaints;
using Psps.Services.Posts;
using Psps.Services.Suggestions;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class ComplaintDocViewModelValidator : AbstractValidator<ComplaintDocViewModel>
    {
        protected readonly IComplaintDocService _complaintDocService;

        public ComplaintDocViewModelValidator(IMessageService messageService, IComplaintDocService complaintDocService)
        {
            _complaintDocService = complaintDocService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.DocNum).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Version).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.Description).NotEmpty().WithMessage(mandatoryMessage);
            RuleSet("CreateComplaintDoc", () =>
            {
                RuleFor(x => x.DocNum).Must(UniqueDocNum).When(x => !string.IsNullOrEmpty(x.DocNum)).WithMessage(uniqueMessage);
                RuleFor(x => x.Version).Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.File)
                   .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Compliant.FileFormat));
            });

            RuleSet("NewVersion", () =>
            {
                RuleFor(x => x.File)
                      .NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.Version)
                    .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);

                RuleFor(x => x.File)
                     .Must(FileFormat).WithMessage(messageService.GetMessage(SystemMessage.Error.Compliant.FileFormat));
            });

            RuleSet("UpdateVersion", () =>
            {
                RuleFor(x => x.Version)
                .Must(UniqueVersion).When(x => !string.IsNullOrEmpty(x.Version)).WithMessage(uniqueMessage);
                RuleFor(x => x.File)
                    .Must(FileFormat).When(x=>x.File!=null).WithMessage(messageService.GetMessage(SystemMessage.Error.Compliant.FileFormat));
            });
        }

        private bool UniqueVersion(ComplaintDocViewModel model, string version)
        {
            return this._complaintDocService.IsUniqueComplaintDocVersion(model.ComplaintDocId, model.DocNum, version);
        }

        private bool UniqueDocNum(ComplaintDocViewModel model, string name)
        {
            return _complaintDocService.IsUniqueComplaintDocNum(model.ComplaintDocId, model.DocNum);
        }

        private bool FileFormat(ComplaintDocViewModel model, HttpPostedFileBase fileName)
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
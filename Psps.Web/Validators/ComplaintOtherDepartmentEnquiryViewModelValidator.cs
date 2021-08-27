using FluentValidation;
using Psps.Core.Common;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Complaint;
using System.IO;
using System.Web;

namespace Psps.Web.Validators
{
    public class ComplaintOtherDepartmentEnquiryViewModelValidator : AbstractValidator<ComplaintOtherDepartmentEnquiryViewModel>
    {
        private ILookupService _lookupService;

        public ComplaintOtherDepartmentEnquiryViewModelValidator(IMessageService messageService, ILookupService lookupService)
        {
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            _lookupService = lookupService;
            RuleSet("Create", () =>
            {
                //RuleFor(x => x.OtherDepartmentEnquiryReferralDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryMemoDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryMemoFromPoliceDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OtherDepartmentEnquiryOrgInvolved).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryEnclosureNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherEnquiryDepartment).Must(MustBeInputted).When(x => !string.IsNullOrEmpty(x.EnquiryDepartment)).WithMessage(SystemMessage.Error.Compliant.OtherEnquiryDepartmentMustBeInputted);
            });

            RuleSet("Update", () =>
            {
                //RuleFor(x => x.OtherDepartmentEnquiryReferralDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryMemoDate).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryMemoFromPoliceDate).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OtherDepartmentEnquiryOrgInvolved).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherDepartmentEnquiryEnclosureNum).NotEmpty().WithMessage(mandatoryMessage);
                //RuleFor(x => x.OtherEnquiryDepartment).Must(MustBeInputted).When(x => !string.IsNullOrEmpty(x.EnquiryDepartment)).WithMessage(SystemMessage.Error.Compliant.OtherEnquiryDepartmentMustBeInputted);
            });
        }

        private bool MustBeInputted(ComplaintOtherDepartmentEnquiryViewModel model, string OtherEnquiryDepartment)
        {
            var description = _lookupService.GetDescription(LookupType.Department, model.EnquiryDepartment);
            if (description.Equals("others", System.StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(OtherEnquiryDepartment))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
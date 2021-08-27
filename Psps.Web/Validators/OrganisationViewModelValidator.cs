using FluentValidation;
using Psps.Core.Helper;
using Psps.Models.Domain;
using Psps.Services.Organisations;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class OrganisationViewModelValidator : AbstractValidator<OrganisationViewModel>
    {
        protected readonly IOrganisationService _organisationService;
        protected readonly IMessageService _messageService;

        public OrganisationViewModelValidator(IMessageService messageService, IOrganisationService organisationService)
        {
            this._organisationService = organisationService;
            this._messageService = messageService;

            var mandatoryMessage = _messageService.GetMessage(SystemMessage.Error.Mandatory);
            var invalidEmailMsg = _messageService.GetMessage(SystemMessage.Error.InvalidEmail);

            RuleSet("Create", () =>
            {
                RuleFor(x => x.OrganisationName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OrganisationChiName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.EngOrgNameSorting).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.Email).Must(IsValidEmail).When(x => !String.IsNullOrEmpty(x.Email)).WithMessage(invalidEmailMsg);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.OrganisationName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.OrganisationChiName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.EngOrgNameSorting).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.Email).Must(IsValidEmail).When(x => !String.IsNullOrEmpty(x.Email)).WithMessage(invalidEmailMsg);
            });

            RuleSet("ReferenceGuideInsert", () =>
            {   
                ReferenceGuideCommonRule();
            });

            RuleSet("ReferenceGuideUpdate", () =>
            {   
                ReferenceGuideCommonRule();
            });

            RuleSet("CreateOrgAttachment", () =>
            {
                RuleFor(x => x.AttachmentName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentRemark).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentDocument).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdateOrgAttachment", () =>
            {
                RuleFor(x => x.AttachmentName).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentRemark).NotEmpty().WithMessage(mandatoryMessage);
                RuleFor(x => x.AttachmentDocument).NotEmpty().WithMessage(mandatoryMessage);
            });
        }

        private bool IsValidEmail(OrganisationViewModel model, string email)
        {
            bool value = false;
            if (CommonHelper.IsValidEmail(email))
            {
                value = true;
            }
            return value;
        }

        #region ReferenceGuideUpdate

        private void ReferenceGuideCommonRule()
        {
            var mandatoryMessage = _messageService.GetMessage(SystemMessage.Error.Mandatory);
            var replySlipDateMutualMessage = _messageService.GetMessage(SystemMessage.Error.Organisation.ReplySlipDateMutual);
            var replySlipReceiveDateMutualMessage = _messageService.GetMessage(SystemMessage.Error.Organisation.ReplySlipReceiveDateMutual);
            var mustBeEarlierOrEqualMessage = _messageService.GetMessage(SystemMessage.Error.Organisation.MustBeEarlierOrEqual);
            var sendDateEarlierReplySlipDateMessage = _messageService.GetMessage(SystemMessage.Error.Organisation.SendDateEarlierReplySlipDate);
            var sendDateEarlierReplySlipReceiveDateMessage = _messageService.GetMessage(SystemMessage.Error.Organisation.SendDateEarlierReplySlipReceiveDate);

//            RuleFor(x => x.SendDate).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.SendDate).LessThanOrEqualTo(x => x.ReplySlipDate).When(x => x.SendDate.HasValue && x.ReplySlipDate.HasValue).WithMessage(sendDateEarlierReplySlipDateMessage);
            RuleFor(x => x.SendDate).LessThanOrEqualTo(x => x.ReplySlipReceiveDate).When(x => x.SendDate.HasValue && x.ReplySlipReceiveDate.HasValue).WithMessage(sendDateEarlierReplySlipReceiveDateMessage);

            RuleFor(x => x.ReplyFromId).NotEmpty().WithMessage(mandatoryMessage);

            RuleFor(x => x.ReplySlipDate).NotEmpty().When(x => x.ReplySlipReceiveDate.HasValue).WithMessage(replySlipDateMutualMessage);
            RuleFor(x => x.ReplySlipReceiveDate).NotEmpty().When(x => x.ReplySlipDate.HasValue).WithMessage(replySlipReceiveDateMutualMessage);
            RuleFor(x => x.ReplySlipReceiveDate).GreaterThanOrEqualTo(x => x.ReplySlipDate).When(x => x.ReplySlipReceiveDate.HasValue && x.ReplySlipDate.HasValue).WithMessage(mustBeEarlierOrEqualMessage);
        }

        //private bool SendDateEarlier(OrganisationViewModel model, DateTime? checkDate)
        //{
        //    if (model.SendDate.Value >= checkDate.Value)
        //        return false;

        //    return true;
        //}

        //private bool Mustbeearlierorequal(OrganisationViewModel model, DateTime? m)
        //{
        //    bool value = true;
        //    if (model.ReplySlipReceiveDate.HasValue && model.ReplySlipDate.HasValue)
        //        if (model.ReplySlipReceiveDate > model.ReplySlipDate)
        //            return false;
        //    return value;
        //}

        //private bool ReplySlipReceiveDateMutual(OrganisationViewModel model, DateTime? m)
        //{
        //    bool value = true;
        //    if (model.ReplySlipReceiveDate.HasValue && !model.ReplySlipDate.HasValue)
        //        return false;

        //    return value;
        //}

        //private bool ReplySlipDateMutual(OrganisationViewModel model, DateTime? m)
        //{
        //    bool value = true;
        //    if (!model.ReplySlipReceiveDate.HasValue && model.ReplySlipDate.HasValue)
        //        return false;

        //    return value;
        //}

        #endregion ReferenceGuideUpdate
    }
}
using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.Suggestion
{
    [Validator(typeof(SuggestionMasterViewModelValidator))]
    public partial class SuggestionMasterViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionSource")]
        public string SuggestionSourceId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionSource")]
        public IDictionary<string, string> SuggestionSources { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionSource")]
        public string SuggestionSourceOther { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_ActivityConcern")]
        public string ActivityConcernId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_ActivityConcern")]
        public IDictionary<string, string> ActivityConcerns { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_ActivityConcern")]
        public string SuggestionActivityConcernOther { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_FileRefNum")]
        public string FileRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionRefNum")]
        public string SuggestionRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionRefNum")]
        public string SuggestionRefNumPR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionDate")]
        public DateTime? SuggestionDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionDate")]
        public DateTime? SuggestionDateStart { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SuggestionDate")]
        public DateTime? SuggestionDateEnd { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_AcknowledgementSentDate")]
        public DateTime? AcknowledgementSentDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_Nature")]
        public string NatureId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_Nature")]
        public IDictionary<string, string> Natures { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_SenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_Content")]
        public string SuggestionDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_PartNum")]
        public string PartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_PartNum")]
        public string EnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Suggestion_Remark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentDocument")]
        public HttpPostedFileBase AttachmentDocument { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentRemark")]
        public string AttachmentRemark { get; set; }

        #region R20

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RPT_R20_DateFrom")]
        public DateTime? R20_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RPT_R20_DateTo")]
        public DateTime? R20_DateTo { get; set; }

        #endregion R20

        public string AttachmentId { get; set; }

        public string SuggestionMasterId { get; set; }
    }
}
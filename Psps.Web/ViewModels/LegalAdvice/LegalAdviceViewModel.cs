using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Psps.Web.ViewModels.LegalAdvice
{
    [Validator(typeof(LegalAdviceViewModelValidator))]
    public partial class LegalAdviceViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceCode")]
        public string LegalAdviceCode { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceType")]
        public string LegalAdviceTypeHeadId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceType")]
        public IDictionary<string, string> LegalAdviceTypeHeads { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceType")]
        public string LegalAdviceTypeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceType")]
        public IDictionary<string, string> LegalAdviceTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceDescription")]
        public string LegalAdviceDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdviceDescription")]
        public string SearchLegalAdviceDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdvice_PartNum_EnclosureNum")]
        public string PartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdvice_PartNum_EnclosureNum")]
        public string EnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LegalAdvice_PartNum_EnclosureNum")]
        public string SearchEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FileNo")]
        public string BoxFile { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FileNo")]
        public string FileNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSPRequired")]
        public string PSPRequiredId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSPRequired")]
        public IDictionary<string, string> PSPRequireds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EffectiveDate")]
        public DateTime? EffectiveDateStart { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EffectiveDate")]
        public DateTime? EffectiveDateEnd { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public string RelatedLegalAdviceCode { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public string RelatedLegalAdviceTypeId { get; set; }

        public string RelatedLegalAdviceDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public IDictionary<string, string> RelatedLegalAdviceTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public string RelatedVenueTypeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public IDictionary<string, string> RelatedVenueTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public string RelatedLegalAdviceId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "RelatedLegalAdvice")]
        public IDictionary<string, string> RelatedLegalAdvices { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Active")]
        public bool Active { get; set; }

        public string LegalAdviceMasterId { get; set; }
    }
}
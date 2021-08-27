using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Complaint
{
    [Validator(typeof(ComplaintPoliceCaseViewModelValidator))]
    public class ComplaintPoliceCaseViewModel : BaseViewModel
    {
        public int? ComplaintPoliceCaseId { get; set; }

        public int ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CaseInvestigateRefNum")]
        public string PoliceCaseCaseInvestigateRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ReferralDate")]
        public DateTime? PoliceCaseReferralDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_MemoDate")]
        public DateTime? PoliceCaseMemoDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConcernOrgName")]
        public string PoliceCaseConcernOrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConcernOrgName")]
        public int? PoliceCaseOrgId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CorrespondenceEnclosureNum")]
        public string PoliceCaseCorrespondenceEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CorrespondenceEnclosureNum")]
        public string PoliceCaseCorrespondencePartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_InvestigationResult")]
        public string PoliceCaseInvestigationResult { get; set; }

        public IDictionary<string, string> PoliceCaseInvestigationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_PoliceRefNum")]
        public string PoliceCasePoliceRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CaseNature")]
        public string PoliceCaseCaseNature { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CaseNature")]
        public string PoliceCaseCaseNatureHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ResultDetail")]
        public string PoliceCaseResultDetail { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ResultDetail")]
        public string PoliceCaseResultDetailHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_EnclosureNum")]
        public string PoliceCaseEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_EnclosureNum")]
        public string PoliceCasePartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_FundRaisingDate")]
        public DateTime? PoliceCaseFundRaisingDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_FundRaisingTime")]
        public string PoliceCaseFundRaisingTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_FundRaisingLocation")]
        public string PoliceCaseFundRaisingLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConvictedPersonName")]
        public string PoliceCaseConvictedPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConvictedPersonPosition")]
        public string PoliceCaseConvictedPersonPosition { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_OffenceDetail")]
        public string PoliceCaseOffenceDetail { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_OffenceDetail")]
        public string PoliceCaseOffenceDetailHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_SentenceDetail")]
        public string PoliceCaseSentenceDetail { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_SentenceDetail")]
        public string PoliceCaseSentenceDetailHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CourtRefNum")]
        public string PoliceCaseCourtRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CourtHearingDate")]
        public DateTime? PoliceCaseCourtHearingDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_Remark")]
        public string Remark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_Remark")]
        public string RemarkHtml { get; set; }
    }
}
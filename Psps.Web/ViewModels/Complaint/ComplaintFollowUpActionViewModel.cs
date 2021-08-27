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
    [Validator(typeof(ComplaintFollowUpActionViewModelValidator))]
    public class ComplaintFollowUpActionViewModel : BaseViewModel
    {
        public int? ComplaintFollowUpActionId { get; set; }

        public int ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_EnclosureNum")]
        public string FollowUpEnclosureNum { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpIndicator")]
        public bool FollowUpIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ContactOrgName")]
        public string FollowUpContactOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ContactPersonName")]
        public string FollowUpContactPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ContactDate")]
        public DateTime? FollowUpContactDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OrgRemark")]
        public string FollowUpOrgRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OrgRemark")]
        public string FollowUpOrgRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterType")]
        public string FollowUpFollowUpLetterType { get; set; }

        public IDictionary<string, string> FollowUpFollowUpLetterTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterIssueDate")]
        public DateTime? FollowUpLetterIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterReceiver")]
        public string FollowUpLetterReceiver { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterRemark")]
        public string FollowUpLetterRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterRemark")]
        public string FollowUpLetterRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterActionFileRef")]
        public string FollowUpLetterActionFileRefEnclosureNum { get; set; }

        public string FollowUpLetterActionFileRefPartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterActionFileRefRemark")]
        public string FollowUpLetterActionFileRefRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpLetterActionFileRefRemark")]
        public string FollowUpLetterActionFileRefRemarkHtml { get; set; }

        //
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpOrgReply")]
        public string FollowUpLetterActionFollowUpOrgReply { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpOrgReply")]
        public string FollowUpLetterActionFollowUpOrgReplyHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpOrgReplyDate")]
        public DateTime? FollowUpLetterActionFollowUpOrgReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpOfficerName")]
        public string FollowUpLetterActionFollowUpOfficerName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_FollowUpOfficerPosition")]
        public string FollowUpLetterActionFollowUpOfficerPosition { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ReportPoliceIndicator")]
        public bool FollowUpReportPoliceIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_VerbalReportDate")]
        public DateTime? FollowUpVerbalReportDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_PoliceStation")]
        public string FollowUpPoliceStation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_PoliceOfficerName")]
        public string FollowUpPoliceOfficerName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_RnNum")]
        public string FollowUpRnNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_RnRemark")]
        public string FollowUpRnRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_RnRemark")]
        public string FollowUpRnRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_WrittenReferralDate")]
        public DateTime? FollowUpWrittenReferralDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ReferralPoliceStation")]
        public string FollowUpReferralPoliceStation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ActionFileRef")]
        public string FollowUpActionFileRefEnclosureNum { get; set; }

        public string FollowUpActionFileRefPartNum { get; set; }

        //
        public IDictionary<string, string> PoliceCaseInvestigationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_InvestigationResult")]
        public string FollowUpLetterActionPoliceInvestigation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_PoliceInvestigationResult")]
        public string FollowUpLetterActionPoliceInvestigationResult { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_PoliceInvestigationResult")]
        public string FollowUpLetterActionPoliceInvestigationResultHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_PoliceReplyDate")]
        public DateTime? FollowUpLetterActionPoliceReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ReferralPoliceOfficerName")]
        public string FollowUpLetterActionReferralPoliceOfficerName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_ReferralPoliceOfficerPosition")]
        public string FollowUpLetterActionReferralPoliceOfficerPosition { get; set; }

        //TIR #: PSUAT00136
        //--- Begin ---
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConvictedPersonName")]
        public string FollowUpConvictedPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ConvictedPersonPosition")]
        public string FollowUpConvictedPersonPosition { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_OffenceDetail")]
        public string FollowUpOffenceDetail { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_OffenceDetail")]
        public string FollowUpOffenceDetailHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_SentenceDetail")]
        public string FollowUpSentenceDetail { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_SentenceDetail")]
        public string FollowUpSentenceDetailHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CourtRefNum")]
        public string FollowUpCourtRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_CourtHearingDate")]
        public DateTime? FollowUpCourtHearingDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_Remark")]
        public string FollowUpPoliceRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_Remark")]
        public string FollowUpPoliceRemarkHtml { get; set; }

        //--- End  ---

        //
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpIndicator")]
        public bool FollowUpLetterActionOtherFollowUpIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpPartyName")]
        public string FollowUpLetterActionOtherFollowUpPartyName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpContactDate")]
        public DateTime? FollowUpLetterActionOtherFollowUpContactDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpRemark")]
        public string FollowUpLetterActionOtherFollowUpRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpRemark")]
        public string FollowUpLetterActionOtherFollowUpRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpFileRef")]
        public string FollowUpLetterActionOtherFollowUpFileRefEnclosureNum { get; set; }

        public string FollowUpLetterActionOtherFollowUpFileRefPartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpOfficerName")]
        public string FollowUpLetterActionOtherFollowUpOfficerName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintFollowUpAction_OtherFollowUpOfficerPosition")]
        public string FollowUpLetterActionOtherFollowUpOfficerPosition { get; set; }
    }
}
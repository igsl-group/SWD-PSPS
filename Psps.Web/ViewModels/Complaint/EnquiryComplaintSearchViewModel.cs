using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Complaint
{
    [Validator(typeof(EnquiryComplaintSearchViewModelValidator))]
    public class EnquiryComplaintSearchViewModel : BaseViewModel
    {
        public bool isFirstSearch { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgName")]
        public string OrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OrgStatus")]
        public string OrgStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OrgStatus")]
        public IDictionary<string, string> OrgStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_SubventedIndicator")]
        public string SubventedIndicatorId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_SubventedIndicator")]
        public IDictionary<string, string> SubventedIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RegistrationType")]
        public string RegistrationTypeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RegistrationType")]
        public IDictionary<string, string> RegistrationTypes { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RegistrationOtherName")]
        public string RegistrationOtherName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintRecordType")]
        public string RecordTypeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintRecordType")]
        public string SearchRecordTypeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintRecordType")]
        public IDictionary<string, string> RecordTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RefNo")]
        public string PrefixComplaintRefNo { get; set; }

        public string SuffixComplaintRefNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RefNo")]
        public string ComplaintRefNo { get; set; }

        public string EnquiryRefNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Date")]
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_EnquiryComplaintDate")]
        public DateTime? ComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FirstComplaintDate")]
        public DateTime? FirstComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public string ComplaintSourceId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public string SearchComplaintSourceId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSourceRemark")]
        public string ComplaintSourceRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public IDictionary<string, string> ComplaintSources { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActivityConcern")]
        public string ActivityConcernId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActivityConcern")]
        public string SearchActivityConcernId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActivityConcern")]
        public IDictionary<string, string> ActivityConcerns { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OtherActivityConcern")]
        public string OtherActivityConcern { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ReplyDueDate")]
        public DateTime? ReplyDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_LfpsReceiveDate")]
        public DateTime? LfpsReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_SwdUnit")]
        public string SwdUnit { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FundRaisingDate")]
        public DateTime? FundRaisingDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FundRaisingTime")]
        public string FundRaisingTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FundRaisingLocation")]
        public string FundRaisingLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FundRaiserInvolve")]
        public decimal? FundRaiserInvolve { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_CollectionMethod")]
        public string CollectionMethod { get; set; }

        public IDictionary<string, string> CollectionMethods { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OtherCollectionMethod")]
        public string OtherCollectionMethod { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ConcernedOrgRef")]
        public string ConcernedOrgId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ConcernedOrgRef")]
        public string ConcernedOrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplainantName")]
        public string ComplainantName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OrgName")]
        public string ConcernedOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_DcLcContent")]
        public string DcLcContent { get; set; }

        [AllowHtml]
        public string DcLcContentHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string[] NonComplianceNatureId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string[] NonComplianceNature { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNatures { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OtherNonComplianceNature")]
        public string OtherNonComplianceNature { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintEnclosureNum")]
        public string ComplaintPartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintEnclosureNum")]
        public string ComplaintEnclosureNum { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_GovernmentHotlineIndicator")]
        public bool GovernmentHotlineIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ProcessStatus")]
        public string ProcessStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ProcessStatus")]
        public IDictionary<string, string> ProcessStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintResult")]
        public string ComplaintResultId { get; set; }

        public IDictionary<string, string> ComplaintResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintResultRemark")]
        public string ComplaintResultRemark { get; set; }

        [AllowHtml]
        public string ComplaintResultRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemarkId { get; set; }

        public IDictionary<string, string> WithholdingRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OtherWithholdingRemark")]
        public string OtherWithholdingRemark { get; set; }

        [AllowHtml]
        public string OtherWithholdingRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActionFileEnclosureNum")]
        public string ActionFilePartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActionFileEnclosureNum")]
        public string ActionFileEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintRemarks")]
        public string ComplaintRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_PspPermitNum")]
        public string PspPermitNum { get; set; }

        public string PspApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FdPermitNum")]
        public string FdPermitNum { get; set; }

        public string FdEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RelatedComplaintRef")]
        public string RelatedComplaintRef { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingListIndicator")]
        public bool WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingBeginDate")]
        public DateTime? WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingEndDate")]
        public DateTime? WithholdingEndDate { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_IsFollowUp")]
        public bool IsFollowUp { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FollowUpIndicator")]
        public bool FollowUpIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ReportPoliceIndicator")]
        public bool ReportPoliceIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OthersFollowUpIndicator")]
        public bool OthersFollowUpIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ProcessStatus")]
        public string SearchProcessStatusId { get; set; }

        public string RelatedComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_PoliceCaseIndicator")]
        public string PoliceCaseIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingIndicator")]
        public string WithholdingIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OrgRefIndicator")]
        public string OrgRefIndicator { get; set; }

        public IDictionary<string, string> YesNos { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
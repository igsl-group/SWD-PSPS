using FluentValidation.Attributes;
using Psps.Models.Domain;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Complaint
{
    [Validator(typeof(EditEnquiryComplaintViewModelValidator))]
    public class EditEnquiryComplaintViewModel : BaseViewModel
    {
        public string PrePage { get; set; }

        public string OrgMasterId { get; set; }

        public int ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintRef")]
        public string ComplaintRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintRecordType")]
        public string ComplaintRecordType { get; set; }

        public IDictionary<string, string> ComplaintRecordTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintSource")]
        public string ComplaintSource { get; set; }

        public IDictionary<string, string> ComplaintSources { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintSourceRemark")]
        public string ComplaintSourceRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ActivityConcern")]
        public string ActivityConcern { get; set; }

        public IDictionary<string, string> ActivityConcerns { get; set; }

        public string OtherActivityConcern { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintDate")]
        public DateTime? ComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FirstComplaintDate")]
        public DateTime? FirstComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ReplyDueDate")]
        public DateTime? ReplyDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_LfpsReceiveDate")]
        public DateTime? LfpsReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_SwdUnit")]
        public string SwdUnit { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FundRaisingDate")]
        public DateTime? FundRaisingDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FundRaisingTime")]
        public string FundRaisingTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FundRaisingLocation")]
        public string FundRaisingLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FundRaiserInvolve")]
        public decimal? FundRaiserInvolve { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_CollectionMethod")]
        public string CollectionMethod { get; set; }

        public IDictionary<string, string> CollectionMethods { get; set; }

        public string OtherCollectionMethod { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OrgRef")]
        public string OrgId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplainantName")]
        public string ComplainantName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OrgName")]
        public string ConcernedOrgName { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_GovernmentHotlineIndicator")]
        public bool GovernmentHotlineIndicator { get; set; }

        public int? PspApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_PspPermitNum")]
        public string PspPermitNum { get; set; }

        public int? FdEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FdPermitNum")]
        public string FdPermitNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_DcLcContent")]
        public string DcLcContent { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_DcLcContent")]
        public string DcLcContentHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_NonComplianceNature")]
        public string NonComplianceNatureId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_NonComplianceNature")]
        public string[] NonComplianceNature { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNatures { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OtherNonComplianceNature")]
        public string OtherNonComplianceNature { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintEnclosureNum")]
        public string ComplaintPartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintEnclosureNum")]
        public string ComplaintEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ProcessStatus")]
        public string ProcessStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ProcessStatus")]
        public IDictionary<string, string> ProcessStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintResult")]
        public string ComplaintResult { get; set; }

        public IDictionary<string, string> ComplaintResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintResultRemark")]
        public string ComplaintResultRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintResultRemark")]
        public string ComplaintResultRemarkHtml { get; set; }

        public IDictionary<string, string> WithholdingRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OtherWithholdingRemark")]
        public string OtherWithholdingRemark { get; set; }

        [AllowHtml] 
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_OtherWithholdingRemark")]
        public string OtherWithholdingRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ActionFileEnclosureNum")]
        public string ActionFilePartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ActionFileEnclosureNum")]
        public string ActionFileEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_RelatedComplaintRef")]
        public string RelatedComplaintRef { get; set; }

        public int? RelatedComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintRemarks")]
        public string ComplaintRemarks { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingListIndicator")]
        public bool WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingBeginDate")]
        public DateTime? WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_WithholdingEndDate")]
        public DateTime? WithholdingEndDate { get; set; }

        public byte[] RowVersion { get; set; }

        #region org info table

        public string OrgEngName { get; set; }

        public string OrgChiName { get; set; }

        public string LblWithholdingBeginDate { get; set; }

        public string LblWithholdingEndDate { get; set; }

        public string LblPspRef { get; set; }

        public string LblPspContactPersonName { get; set; }

        public string LblPspContactPersonEmailAddress { get; set; }

        public string LblFdRef { get; set; }

        public string LblFdContactPersonName { get; set; }

        public string LblFdContactPersonEmailAddress { get; set; }

        #endregion org info table

        #region Enquiry and Complaint  Edit

        public string complaintAttachmentId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_FileDescription")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_AttachmentFile")]
        public HttpPostedFileBase AttachmentFile { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        #endregion Enquiry and Complaint  Edit
    }
}
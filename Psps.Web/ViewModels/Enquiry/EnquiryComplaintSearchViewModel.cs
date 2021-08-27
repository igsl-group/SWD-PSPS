using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Enquiry
{
    [Validator(typeof(EnquiryComplaintSearchViewModelValidator))]
    public class EnquiryComplaintSearchViewModel : BaseViewModel
    {

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_OrgName")]
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

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_EnquiryComplaintDate")]
        public string ComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FirstComplaintDate")]
        public string FirstComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public string ComplaintSourceId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public string SearchComplaintSourceId { get; set; }

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

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FundRaisingLocation")]
        public string FundRaisingLocation{ get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string NonComplianceNatureId { get; set; }

        public string NonComplianceNatureOther { get; set; }
        
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNatures { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ReplyDueDate")]
        public string ReplyDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_LfpsReceiveDate")]
        public string LfpsReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_SwdUnit")]
        public string SwdUnit { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ConcernedOrgRef")]
        public string ConcernedOrgId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ConcernedOrgRef")]
        public string ConcernedOrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplainantName")]
        public string ComplainantName { get; set; }
        
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_DcLcContent")]
        public string DcLcContent { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string NonComplianceNature1Id { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNature1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string OtherNonComplianceNature1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string NonComplianceNature2Id { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNature2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string OtherNonComplianceNature2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string NonComplianceNature3Id { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNature3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_NonComplianceNature")]
        public string OtherNonComplianceNature3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintEnclosureNum")]
        public string ComplaintEnclosureNum { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_GovernmentHotlineIndicator")]
        public bool GovernmentHotlineIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ProcessStatus")]
        public string ProcessStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ProcessStatus")]
        public IDictionary<string, string> ProcessStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ActionFileEnclosureNum")]
        public string ActionFileEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintRemarks")]
        public string ComplaintRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_PspPermitNum")]
        public string PspPermitNum { get; set; }

        public string PspApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_FdPermitNum")]
        public string FdPermitNum { get; set; }

        public string FdApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_RelatedComplaintRef")]
        public string RelatedComplaintRef { get; set; }

        public string RelatedComplaintMasterId { get; set; }
        
        public byte[] RowVersion { get; set; }
    }
}
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
    [Validator(typeof(ComplaintTelRecordViewModelValidator))]
    public class ComplaintTelRecordViewModel : BaseViewModel
    {
        public int? ComplaintTelRecordId { get; set; }

        public int ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_TelComplaintRef")]
        public string TelComplaintRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_TelRecordComplaintDate")]
        public DateTime TelRecordComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_ComplaintTime")]
        public string ComplaintTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_TelRecordComplainantName")]
        public string TelRecordComplainantName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_ComplainantTelNum")]
        public string ComplainantTelNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_OrgName")]
        public string ComplaintTelRecordOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_FundRaisingLocation")]
        public string FundRaisingLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_PspPermitNum")]
        public int? PspApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_FdPermitNum")]
        public int? FdEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_ComplaintContentRemark")]
        public string ComplaintContentRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_ComplaintContentRemark")]
        public string ComplaintContentRemarkHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_OfficerName")]
        public string OfficerName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintTelRecord_ApproverUserId")]
        public string OfficerPost { get; set; }
    }
}
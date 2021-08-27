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
    [Validator(typeof(ComplaintOtherDepartmentEnquiryViewModelValidator))]
    public class ComplaintOtherDepartmentEnquiryViewModel : BaseViewModel
    {
        public string ComplaintOtherDeptEnquiryId { get; set; }

        public string ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_RefNum")]
        public string OtherDepartmentEnquiryRefNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_ReferralDate")]
        public DateTime? OtherDepartmentEnquiryReferralDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_MemoDate")]
        public DateTime? OtherDepartmentEnquiryMemoDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_MemoFromPoliceDate")]
        public DateTime? OtherDepartmentEnquiryMemoFromPoliceDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_EnquiryDepartment")]
        public string EnquiryDepartment { get; set; }

        public IDictionary<string, string> EnquiryDepartments { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_OtherEnquiryDepartment")]
        public string OtherEnquiryDepartment { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_OrgInvolved")]
        public string OtherDepartmentEnquiryOrgInvolved { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_EnquiryContent")]
        public string OtherDepartmentEnquiryEnquiryContent { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_EnquiryContent")]
        public string OtherDepartmentEnquiryEnquiryContentHtml { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_EnclosureNum")]
        public string OtherDepartmentEnquiryEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_Remark")]
        public string OtherDepartmentEnquiryRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintOtherDepartmentEnquiry_Remark")]
        public string OtherDepartmentEnquiryRemarkHtml { get; set; }
    }
}
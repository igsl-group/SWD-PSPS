using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Complaint
{
    [Validator(typeof(ComplaintResultViewModelValidator))]
    public class ComplaintResultViewModel : BaseViewModel
    {
        public int? ComplaintResultId { get; set; }

        public int? ComplaintMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_NonComplianceNature")]
        public IList<string> NonComplianceNature { get; set; }

        public IDictionary<string, string> NonComplianceNatures { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_OtherNonComplianceNature")]
        public string OtherNonComplianceNature { get; set; }

        public IDictionary<string, string> Results { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintPoliceCase_ResultDetail")]
        public string Result { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintRemarks")]
        public string ResultRemark { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_ComplaintRemarks")]
        public string ResultRemarkHtml { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
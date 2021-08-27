using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Psps.Web.ViewModels.Enquiry
{
    [Validator(typeof(EnquiryReportViewModelValidator))]
    public partial class EnquiryReportViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R8_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R8_DateTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R9_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R9_DateTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R10_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R10_DateTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R11_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R11_DateTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R12_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R12_DateTo { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        //public string EffectiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R13_DateFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R13_DateTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R14_FromYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R14_ToYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public string R14_ComplaintSource { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EnquiryComplaint_ComplaintSource")]
        public IDictionary<string, string> R14_ComplaintSources { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R16_Date { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        //public DateTime? R16DateTo { get; set; }
    }
}
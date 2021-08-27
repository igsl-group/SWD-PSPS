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
    public partial class LegalAdviceReportViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public string EffectiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public string EffectiveTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? EffectiveDateStart { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? EffectiveDateEnd { get; set; }
    }
}
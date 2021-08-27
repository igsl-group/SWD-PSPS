using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.FlagDay
{
    [Validator(typeof(FlagDayReportViewModelValidator))]
    public class FlagDayReportViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R2_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R2_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R3FromYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R3ToYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R4_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R4_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdYear")]
        public string R24_Year { get; set; }

        public IDictionary<string, string> R24_FdYears { get; set; }

        public IDictionary<string, string> FdYears { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string Raw1_OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdYear")]
        public string Raw3_Year { get; set; }
    }
}
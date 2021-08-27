using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.PSP
{
    [Validator(typeof(PspReportViewModelValidator))]
    public class PspReportViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R1_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R1_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R2_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R2_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R4_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R4_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R5_years")]
        public int? R5_Year { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public int? R6_DisasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public IDictionary<int, string> R6_Disaster { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R7_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R7_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R21_years")]
        public int? R21_Years { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R22_FromDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R22_ToDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R23_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R23_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public int? R27_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public int? R27_YearTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string Raw1_OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? Raw2_YearFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? Raw2_YearTo { get; set; }
    }
}
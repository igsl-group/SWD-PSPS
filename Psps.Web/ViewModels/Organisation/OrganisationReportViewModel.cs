using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.Organisation
{
    public class OrganisationReportViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriod")]
        public DateTime? R25_DateFrom { get; set; }
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReportPeriodTo")]
        public DateTime? R25_DateTo{ get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string Raw1_OrgRef { get; set; }
    }
}
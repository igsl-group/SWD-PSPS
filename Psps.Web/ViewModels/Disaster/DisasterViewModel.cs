using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Disaster
{
    [Validator(typeof(DisasterViewModelValidator))]
    public partial class DisasterViewModel : BaseViewModel
    {
        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_MasterId")]
        //public string DisasterMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterMasterId")]
        public int? DisasterMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterStatistcs")]
        public IDictionary<int, string> DisasterStatistics { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterName")]
        public string DisasterName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_ChiDisasterName")]
        public string ChiDisasterName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterDate")]
        public DateTime? DisasterDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_BeginDate")]
        public DateTime BeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal PspApplicationProcedurePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal PspApplicationProcedureOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal PspScopePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal PspScopeOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal PspApplicationStatusPublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal PspApplicationStatusOthersCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal PspPermitConditionCompliancePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal PspPermitConditionComplianceOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal OtherEnquiryPublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal OtherEnquiryOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_SubTotal")]
        public int SubTotal { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
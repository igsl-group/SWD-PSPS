using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Models.Dto.Disaster;
using Psps.Services.Disaster;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.DisasterStatistics
{
    [Validator(typeof(DisasterStatisticsViewModelValidator))]
    public class DisasterStatisticsViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterStatisticsId")]
        public int? DisasterStatisticsId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterMasterId")]
        public int DisasterMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterName")]
        public string DisasterName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterName")]
        public string DisasterNameForPopUp { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_ReportingOfficer")]
        public string RecordPostId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_ReportingOfficer")]
        public string RecordPostIdForDropDown { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_ReportingDate")]
        public DateTime RecordDate { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal? PspApplicationProcedurePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal? PspApplicationProcedureOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal? PspScopePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal? PspScopeOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal? PspApplicationStatusPublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal? PspApplicationStatusOthersCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal? PspPermitConditionCompliancePublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal? PspPermitConditionComplianceOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Public")]
        public decimal? OtherEnquiryPublicCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Others")]
        public decimal? OtherEnquiryOtherCount { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_SubTotal")]
        public decimal? SubTotal { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_RecordPostIdType")]
        public IDictionary<string, string> RecordPostIds { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Disaster_DisasterNames")]
        public IDictionary<int, string> DisasterNames { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
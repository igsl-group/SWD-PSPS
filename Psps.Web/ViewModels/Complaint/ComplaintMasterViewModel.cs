using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Psps.Web.ViewModels.Account
{
    [Validator(typeof(ComplaintMasterViewModelValidator))]
    public partial class ComplaintMasterViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplaintSource")]
        public IDictionary<string, string> ComplaintSource { get; set; }

        public string ComplaintSourceId { get; set; }

        public string ComplaintSourceRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ReplyDueDate")]
        public string ReplyDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ActivityConcern")]
        public IDictionary<string, string> ActivityConcern { get; set; }

        public string ActivityConcernId { get; set; }

        public string OtherActivityConcern { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplaintDate")]
        public string ComplaintDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplaintDate1stReceived")]
        public string ComplaintDate1stReceivedBySWD { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplaintDateReceivedByLFPS")]
        public string ComplaintDateReceivedByLFPS { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_OrganisationConcerned")]
        public string OrganisationConcerned { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplainantName")]
        public string ComplainantName { get; set; }
        
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ReferFrom1823Indicator")]
        public bool ReferFrom1823Indicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_PermitConcerned")]
        public string PermitConcerned { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_DcLcContent")]
        public string DcLcContent { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_NonComplianceNature")]
        public IDictionary<string, string> NonComplianceNature1 { get; set; }

        public string NonComplianceNature1Id { get; set; }

        public string OtherNonComplianceNature1 { get; set; }

        public IDictionary<string, string> NonComplianceNature2 { get; set; }

        public string NonComplianceNature2Id { get; set; }

        public string OtherNonComplianceNature2 { get; set; }

        public IDictionary<string, string> NonComplianceNature3 { get; set; }

        public string NonComplianceNature3Id { get; set; }

        public string OtherNonComplianceNature3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_EnclosureNum")]
        public string EnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ProcessingStatus")]
        public IDictionary<string, string> ProcessingStatus { get; set; }

        public string ProcessingStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ActionFileEnclosureNum")]
        public string ActionFileEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_RelatedCase")]
        public string RelatedCase { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ComplaintMaster_ComplaintRemarks")]
        public string ComplaintRemarks { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
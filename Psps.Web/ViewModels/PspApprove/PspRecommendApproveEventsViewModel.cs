using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Psps.Web.ViewModels.PspApprove
{
    //[Validator(typeof(PspApproveViewModelValidator))]
    public class PspRecommendApproveEventsViewModel
    {
        /// <summary>
        /// // Below 's navigation properties are for Pspmaster summarize details about its belonging events. (for grid table)
        /// </summary>

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventId")]
        public int PspEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_BatchNum")]
        public int BatchNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventStartDate")]
        public DateTime EventStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventEndDate")]
        public DateTime EventEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_Time")]
        public string Time { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_District")]
        public string District { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_Location")]
        public string ChiLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_MethodOfCollection")]
        public string MethodOfCollection { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventStatus")]
        public string EventStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_Remarks")]
        public string Remark { get; set; }

        /// <summary>
        /// // Below 's navigation properties are for Pspmaster summarize details about its belonging events. (for PspEvents approval)
        /// </summary>
        ///

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_PspApprovalHistoryId")]
        public int PspApprovalHistoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_PermitIssuedDate")]
        public DateTime? PermitIssueDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RejectionLetterDate")]
        //public DateTime? RejectionLetterDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RepresentationReceiveDate")]
        //public DateTime? RepresentationReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_CancelReason")]
        public string CancelReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventIds")]
        public string[] EventIds { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
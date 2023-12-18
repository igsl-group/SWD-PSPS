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

namespace Psps.Web.ViewModels.PSP
{
    [Validator(typeof(PspEventViewModelValidator))]
    public partial class PspEventViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_No")]
        public int? PspEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_BatchNum")]
        public int BatchNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventType")]
        public string EventType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventStartDate")]
        public DateTime EventDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventStartDate")]
        public DateTime? EventStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventEndDate")]
        public DateTime? EventEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventStartTime")]
        public string EventStartTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventEndTime")]
        public string EventEndTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_Time")]
        public string Time { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_District")]
        public string District { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_District")]
        public IDictionary<string, string> Districts { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_ChiLocation")]
        public string ChiLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_SimpChiLocation")]
        public string SimpChiLocation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_PublicPlaceIndicator")]
        public bool? PublicPlaceIndicator { get; set; }

        public IDictionary<string, string> PublicPlaceIndicatorSearchDict { get; set; }
        public IDictionary<string, string> PublicPlaceIndicatorDict { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_CharitySalesItem")]
        public string CharitySalesItem { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_OtherMethodOfCollection")]
        public string OtherMethodOfCollection { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_MethodOfCollection")]
        public IDictionary<string, string> MethodOfCollections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_MethodOfCollection")]
        public List<string> MethodOfCollection { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventStatus")]
        public string EventStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_Remarks")]
        public string Remarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_Remarks")]
        public IDictionary<string, string> EditEventRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_Remarks")]
        public List<string> EditEventRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_ApprovalType")]
        public string ApprovalType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_ApprovalStatus")]
        public string ApprovalStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_TotalEvents")]
        public int TotalEvents { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_ProcessEvents")]
        public int? ProcessEvents { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_EventIds")]
        public string[] EventIds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

        //approvalHist
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_PermitNo")]
        public string PermitNo { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_CancelReason")]
        public string CancelReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_PermitIssueDate")]
        public DateTime? PermitIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_RejectionLetterDate")]
        public DateTime? RejectionLetterDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_RepresentationReceiveDate")]
        public DateTime? RepresentationReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_ValidationMessage")]
        public string ValidationMessage { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspRead_CutOffDate")]
        public DateTime? CutOffDateFrom { get; set; }

        public DateTime? CutOffDateTo { get; set; }

        public bool BypassValidation { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
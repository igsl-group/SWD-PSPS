using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.FlagDay
{
    [Validator(typeof(FlagDayApproveViewModelValidator))]
    public class FlagDayApproveViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_YearofFlagDay")]
        public string YearofFlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationReceivedNum")]
        public string ApplicationReceivedNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationApprovedNum")]
        public string ApplicationApprovedNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationWithdrawNum")]
        public string ApplicationWithdrawNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PostOfApprover")]
        public string PostOfApprover { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApproverId")]
        public string ApproverId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApprovalDate")]
        public DateTime? ApprovalDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Remarks")]
        public string SummaryRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationResult")]
        public string ApplicationResult { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagDay")]
        public string FlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TerritoryRegion")]
        public string TWR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Remarks")]
        public string ApplicationListRemarks { get; set; }
    }
}
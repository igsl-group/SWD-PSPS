using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Psps.Web.ViewModels.FlagDayList
{
    [Validator(typeof(FlagDayListViewModelValidator))]
    public partial class FlagDayListViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_FlagDayListId")]
        public int? FlagDayListId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_Year")]
        public string FlagDayYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_Type")]
        public string FlagDayType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_Type")]
        public string SearchFlagDayType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_FlagDayListDate")]
        public DateTime? FlagDayDate { get; set; }

        // TIR#: PSUAT00035-2  Import or amend Flag Day List is assigned to FD Approver only.
        public bool IsFdApprover { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_Year")]
        public IDictionary<string, string> FlagDayYears { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDL_Type")]
        public IDictionary<string, string> FlagDayTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
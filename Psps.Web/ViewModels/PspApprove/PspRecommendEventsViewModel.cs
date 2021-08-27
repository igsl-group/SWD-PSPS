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
    //[Validator(typeof(PspRecommendEventsViewModelValidator))]
    public partial class PspRecommendEventsViewModel : BaseViewModel
    {
        /// <summary>
        /// // Below 's navigation properties are for Pspmaster summarize detials about its belonging events.
        /// </summary>
        ///
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_PspMasterId")]
        public string PspMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_OrganisationName")]
        public string OrganisationName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_FileRef")]
        public string PspFileRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_PermitNo")]
        public string PspPermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventStartDate")]
        public DateTime? PspEventStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_EventEndDate")]
        public DateTime? PspEventEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_Type")]
        public string PspType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_TotalToBeApprove")]
        public string PspToBeApproveTotal { get; set; }

        public bool DisableInd { get; set; }

        public bool WithHoldInd { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
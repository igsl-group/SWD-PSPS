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
using Psps.Web.ViewModels.PSP;

namespace Psps.Web.ViewModels.PspApprove
{
    [Validator(typeof(PspApproveViewModelValidator))]
    public partial class PspApproveViewModel : BaseViewModel
    {
        public PspApproveViewModel()
        {
            PspRecommendEventsViewModel = new PspRecommendEventsViewModel();
            PspRecommendApproveEventsViewModel = new PspRecommendApproveEventsViewModel();
            PspRecommendCancelEventsViewModel = new PspRecommendCancelEventsViewModel();
            PSPViewModel = new PSPViewModel();
        }

        /// <summary>
        /// // Below 's navigation properties are for Pspmaster summarize detials about its belonging events.
        /// </summary>

        public PspRecommendEventsViewModel PspRecommendEventsViewModel { get; set; }

        /// <summary>
        /// // Below 's navigation properties are for approve Event and Approval for Event Cancellation
        /// </summary>

        public PspRecommendApproveEventsViewModel PspRecommendApproveEventsViewModel { get; set; }

        public PspRecommendCancelEventsViewModel PspRecommendCancelEventsViewModel { get; set; }

        public PSPViewModel PSPViewModel { get; set; }

    }
}
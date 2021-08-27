using Psps.Models.Domain;
using Psps.Models.Dto.OGCIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspEventScheduleDto : BaseDto
    {
        public PspEventScheduleDto(bool isSsaf)
        {
            PspEvents = new List<PspEvent>();

            SendParam = new ActivitySendParam();
            SendParam.InputBy = "";
            SendParam.ApprovedBy = "";
            SendParam.EnquiryContact = "";
            //SendParam.ActivityId = 3;
            long activityId = 3;
            if (isSsaf)
                activityId = 19;
            SendParam.ActivityId = activityId;
            SendParam.Charitable = 1;
        }

        /// <summary>
        ///
        /// </summary>
        public string CharityEventId { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>FRAS action status</remarks>
        public string Status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<PspEvent> PspEvents { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Send Param</remarks>
        public ActivitySendParam SendParam { get; set; }
    }
}
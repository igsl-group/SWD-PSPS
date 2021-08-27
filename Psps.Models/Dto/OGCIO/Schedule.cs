using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class Schedule
    {
        /// <summary>
        /// [Required] Start date of an activity, in yyyy-MM-dd format
        /// </summary>
        /// <remarks>yyyy-MM-dd format</remarks>
        public string DateFrom { get; set; }

        /// <summary>
        /// [Required] End date of an activity, in yyyy-MM-dd format
        /// </summary>
        /// <remarks>yyyy-MM-dd format</remarks>
        public string DateTo { get; set; }

        /// <summary>
        /// [Required] Start time of an activity, in HH:mm format
        /// </summary>
        /// <remarks>HH: mm format</remarks>
        public string TimeFrom { get; set; }

        /// <summary>
        /// [Required] End time of an activity, in HH:mm format
        /// </summary>
        /// <remarks>HH: mm format</remarks>
        public string TimeTo { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspReadEventDto : BaseDto
    {
        public int PspEventId { get; set; }

        public decimal? BatchNum { get; set; }

        public DateTime? EventStartDate { get; set; }

        public DateTime? EventEndDate { get; set; }

        public DateTime? EventStartTime { get; set; }

        public DateTime? EventEndTime { get; set; }

        //public int EventCount
        //{
        //    get
        //    {
        //        if (EventStartDate != null && EventEndDate != null)
        //        {
        //            return Convert.ToInt32((EventEndDate.Value - EventStartDate.Value).TotalDays + 1);
        //        }
        //        return 0;
        //    }
        //}

        public int EventCount { get; set; }

        public string Time { get; set; }

        public string District { get; set; }

        public string Location { get; set; }

        public string ChiLocation { get; set; }

        public string CollectionMethod { get; set; }

        public bool? PublicPlaceIndicator { get; set; }

        public string EventStatus { get; set; }

        public string EventType { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string ValidationMessage { get; set; }

        public byte[] RowVersion { get; set; }

        public int Id
        {
            get
            {
                return PspEventId;
            }
            set
            {
                PspEventId = value;
            }
        }
    }
}
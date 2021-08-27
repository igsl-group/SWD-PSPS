using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdMaster
{
    public partial class FdReadEventDto : BaseDto
    {
        public int FdEventId { get; set; }

        public DateTime? FlagDay { get; set; }

        public DateTime? EventStartTime { get; set; }

        public DateTime? EventEndTime { get; set; }

        public string Time { get; set; }

        public string TWR { get; set; }

        public string District { get; set; }

        public string PermitNum { get; set; }

        public string BagColour { get; set; }

        public string FlagColour { get; set; }

        public string CollectionMethod { get; set; }

        public string Remark { get; set; }

        public byte[] RowVersion { get; set; }

        public bool? PermitRevokeIndicator { get; set; }

        public int Id
        {
            get
            {
                return FdEventId;
            }
            set
            {
                FdEventId = value;
            }
        }
    }
}
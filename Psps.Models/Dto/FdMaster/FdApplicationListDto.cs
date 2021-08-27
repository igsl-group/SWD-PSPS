using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdMaster
{
    public partial class FdApplicationListDto : BaseDto
    {
        public string FdRef { get; set; }

        public string OrgName { get; set; }

        public string ApplicationResult { get; set; }

        public DateTime? FlagDay { get; set; }

        public string TWR { get; set; }

        public string TwrDistrict { get; set; }

        public string PermitNo { get; set; }

        public string ApproveRemarks { get; set; }

        public string FrasResponse { get; set; }

        public bool Approve { get; set; }

        public int FdEventId { get; set; }

        public byte[] RowVersion { get; set; }

        public bool? PermitRevokeIndicator { get; set; }

        public int FdMasterId { get; set; } 
    }
}
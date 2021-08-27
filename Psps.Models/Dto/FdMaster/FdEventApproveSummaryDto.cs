using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdMaster
{
    public partial class FdEventApproveSummaryDto : BaseDto
    {
        public string YearOfFlagDay { get; set; }

        public int ApplicationReceivedNum { get; set; }

        public int ApplicationApprovedNum { get; set; }

        public int ApplicationWithdrawNum { get; set; }

        public string PostOfApprover { get; set; }

        public string ApproverId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string SummaryRemarks { get; set; }

        public bool Approved { get; set; }
    }
}
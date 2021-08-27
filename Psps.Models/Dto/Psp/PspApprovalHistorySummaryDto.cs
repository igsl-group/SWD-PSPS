using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspApprovalHistorySummaryDto : BaseDto
    {
        public int PspApprovalHistoryId { get; set; }

        public string PspPermitNo { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public string EventStatus { get; set; }

        public string ApprType { get; set; }

        public string CancelReason { get; set; }

        public string Remark { get; set; }

        public DateTime? RejectionDate { get; set; }

        public int TotalEventsToBeApprove { get; set; }

        public DateTime? EventStartDate { get; set; }

        public DateTime? EventEndDate { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Id
        {
            get
            {
                return PspApprovalHistoryId.ToString() + EventStatus + PspPermitNo;
            }
        }
    }
}
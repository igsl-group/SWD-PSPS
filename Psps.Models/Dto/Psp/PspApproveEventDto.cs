using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspApproveEventDto : BaseDto
    {
        public int PspMasterId { get; set; }

        public int PspApprovalHistoryId { get; set; }

        public int PspCancelHistoryId { get; set; }

        public string EngOrgName { get; set; }

        public string EngOrgNameSorting { get; set; }

        public string ChiOrgName { get; set; }

        public string PspRef { get; set; }

        public string PspPermitNo { get; set; }

        public DateTime? EventStartDate { get; set; }

        public DateTime? EventEndDate { get; set; }

        public string ApprType { get; set; }

        public string EventStatus { get; set; }

        public int TotalEventsToBeApprove { get; set; }

        public DateTime? RejectionDate { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public string CancelReason { get; set; }

        public string ProcessingOfficerPost { get; set; }

        public byte[] RowVersion { get; set; }

        public string Id
        {
            get
            {
                return PspMasterId.ToString() + PspApprovalHistoryId.ToString() + EventStatus + PspPermitNo;
            }
        }
    }
}
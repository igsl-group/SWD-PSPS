using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspRecommendEventsDto : BaseDto
    {
        public string PspYear { get; set; }

        public int PspMasterId { get; set; }

        public string OtherEngOrgName { get; set; }

        public string PspRef { get; set; }

        public string PermitNum { get; set; }

        public DateTime EventStartDate { get; set; }

        public DateTime EventEndDate { get; set; }

        public string ApprovalType { get; set; }

        public int TotEventsToBeApproved { get; set; }

        public DateTime? RejectionLetterDate { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public string CancelReason { get; set; }

        public int PspApprovalHistoryId { get; set; }

        public string EngOrgNameSorting { get; set; }

        public string ChiOrgName { get; set; }

        public string ProcessingOfficerPost { get; set; }

        public string Id
        {
            get
            {
                return PspMasterId.ToString() + PspApprovalHistoryId.ToString() + ApprovalType + PermitNum;
            }
        }
    }
}
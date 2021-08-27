using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Domain
{
    public partial class PspRecommendEventsView : BaseEntity<int>
    {
        public virtual int PspMasterId { get; set; }

        public virtual string OtherEngOrgName { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime EventStartDate { get; set; }

        public virtual DateTime EventEndDate { get; set; }

        public virtual string ApprovalType { get; set; }

        public virtual int TotEventsToBeApproved { get; set; }

        public virtual DateTime? RejectionLetterDate { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        public virtual string CancelReason { get; set; }

        public virtual int PspApprovalHistoryId { get; set; }

        public virtual string EngOrgNameSorting { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual string ProcessingOfficerPost { get; set; }

        public override int Id
        {
            get
            {
                return PspMasterId;
            }
            set
            {
                PspMasterId = value;
            }
        }
    }
}
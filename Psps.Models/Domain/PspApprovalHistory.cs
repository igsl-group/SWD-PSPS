using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspApprovalHistory : BaseAuditEntity<int>
    {
        public virtual int PspApprovalHistoryId { get; set; }

        public virtual PspMaster PspMaster { get; set; }

        public virtual string ApprovalType { get; set; }

        public virtual string ApprovalStatus { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        //public virtual DateTime? RejectionLetterDate { get; set; }

        //public virtual DateTime? RepresentationReceiveDate { get; set; }

        public virtual string CancelReason { get; set; }

        public virtual string Remark { get; set; }

        public virtual bool? TwoBatchApproachIndicator { get; set; }

        //public virtual bool? PermitRevokeIndicator { get; set; }

        public override int Id
        {
            get
            {
                return PspApprovalHistoryId;
            }
            set
            {
                PspApprovalHistoryId = value;
            }
        }
    }
}
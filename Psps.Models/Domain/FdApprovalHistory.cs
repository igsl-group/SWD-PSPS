using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdApprovalHistory : BaseAuditEntity<string>
    {
        public virtual string FdYear { get; set; }

        //public virtual FdMaster FdMaster { get; set; }

        public virtual string ApproverPostId { get; set; }

        public virtual string ApproverUserId { get; set; }

        public virtual DateTime? ApprovalDate { get; set; }

        public virtual string ApprovalRemark { get; set; }

        public override string Id
        {
            get
            {
                return FdYear;
            }
            set
            {
                FdYear = value;
            }
        }
    }
}
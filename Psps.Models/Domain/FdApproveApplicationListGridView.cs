using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdApproveApplicationListGridView : BaseEntity<int>
    {
        public virtual int FdMasterId { get; set; }

        public virtual int OrgId { get; set; }

        public virtual string FdYear { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string OrgName { get; set; }

        public virtual DateTime? FlagDay { get; set; }

        public virtual string TWR { get; set; }

        public virtual string TwrDistrict { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string ApproveRemarks { get; set; }

        public virtual string FrasResponse { get; set; }

        public virtual bool Approve { get; set; }

        public virtual int FdEventId { get; set; }

        public virtual byte[] RowVersion { get; set; }

        public virtual bool? PermitRevokeIndicator { get; set; }

        public override int Id
        {
            get
            {
                return FdMasterId;
            }
            set
            {
                FdMasterId = value;
            }
        }
    }
}
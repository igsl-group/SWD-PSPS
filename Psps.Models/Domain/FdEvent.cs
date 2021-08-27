using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdEvent : BaseAuditEntity<int>
    {
        public virtual int FdEventId { get; set; }

        public virtual FdMaster FdMaster { get; set; }

        public virtual DateTime? FlagDay { get; set; }

        public virtual DateTime? FlagTimeFrom { get; set; }

        public virtual DateTime? FlagTimeTo { get; set; }

        public virtual string TWR { get; set; }

        public virtual string TwrDistrict { get; set; }

        public virtual string CollectionMethod { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        public virtual DateTime? PermitAcknowledgementReceiveDate { get; set; }

        public virtual bool? PermitRevokeIndicator { get; set; }

        public virtual string FlagColour { get; set; }

        public virtual string BagColour { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string FrasCharityEventId { get; set; }

        public virtual string FrasResponse { get; set; }

        public virtual string FrasStatus { get; set; }

        public override int Id
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
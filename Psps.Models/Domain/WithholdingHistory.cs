using Psps.Core.Models;
using System;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class WithholdingHistory : BaseEntity<int>
    {
        public virtual int RowNumber { get; set; }

        public virtual int OrgId { get; set; }

        public virtual int RecordKey { get; set; }

        public virtual string WithholdSource { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }

        public virtual DateTime? EventEndDate { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string DocSubmission { get; set; }

        public virtual string OfficialReceiptReceivedDate { get; set; }

        public virtual string NewspaperCuttingReceivedDate { get; set; }

        public virtual string WithholdingReason { get; set; }

        public virtual string DocRemark { get; set; }

        public virtual string Ref { get; set; }

        public override int Id
        {
            get
            {
                return RowNumber;
            }
            set
            {
                RowNumber = value;
            }
        }
    }
}
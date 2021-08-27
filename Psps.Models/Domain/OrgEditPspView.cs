using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgEditPspView : BaseEntity<int>
    {
        public virtual int RowNum { get; set; }

        public virtual int PspMasterId { get; set; }

        public virtual int OrgId { get; set; }

        public virtual string PspRef { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual DateTime? ApplicationDisposalDate { get; set; }

        public virtual string EngFundRaisingPurpose { get; set; }

        public virtual string ApplicationResult { get; set; }

        public virtual DateTime? EventPeriodFrom { get; set; }

        public virtual DateTime? EventPeriodTo { get; set; }

        public virtual bool? TwoBatchApproachIndicator { get; set; }

        public virtual string ProcessingOfficerPost { get; set; }

        public virtual int EventApprovedNum { get; set; }

        public virtual int EventHeldNum { get; set; }

        public virtual int EventCancelledNum { get; set; }

        public virtual float EventHeldPercent { get; set; }

        public virtual string ArCheckIndicator { get; set; }

        public virtual string ArCheckIndicatorRaw { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string SpecialRemark { get; set; }

        public virtual bool IsSsaf { get; set; }
        
        public override int Id
        {
            get
            {
                return RowNum;
            }
            set
            {
                RowNum = value;
            }
        }
    }
}
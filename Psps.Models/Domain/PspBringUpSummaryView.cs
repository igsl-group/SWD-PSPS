using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspBringUpSummaryView : BaseEntity<string>
    {
        public virtual int Resubmit { get; set; }

        public virtual int PspMasterId { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual DateTime ApplicationReceiveDate { get; set; }

        public virtual DateTime PspEventDate { get; set; }

        public virtual string ProcessingOfficerPost { get; set; }

        public virtual string SpecialRemark { get; set; }

        public override string Id
        {
            get
            {
                return PspRef;
            }
            set
            {
                PspRef = value;
            }
        }
    }
}
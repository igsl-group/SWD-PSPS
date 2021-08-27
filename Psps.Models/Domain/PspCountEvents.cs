using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspCountEvents : BaseEntity<int>
    {
        public virtual int PspMasterId { get; set; }

        public virtual DateTime? EventStartDate { get; set; }

        public virtual DateTime? EventEndDate { get; set; }

        public virtual int? TotEvents { get; set; }

        public virtual string PermitNum { get; set; }

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
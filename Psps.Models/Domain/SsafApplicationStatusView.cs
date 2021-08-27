using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class SsafApplicationStatusView : BaseEntity<string>
    {
        public virtual string Type { get; set; }

        public virtual string Year { get; set; }

        public virtual int ApplicationReceived { get; set; }

        public virtual int PSPIssued { get; set; }        					

        public virtual int RejectedApplication { get; set; }

        public virtual int ApplicationWithdrawn { get; set; }

        public virtual int TwoBatchApplication { get; set; }

        public virtual int OverdueAC { get; set; }

        public override string Id
        {
            get
            {
                return Year;
            }
            set
            {
                Year = value;
            }
        }
    }
}
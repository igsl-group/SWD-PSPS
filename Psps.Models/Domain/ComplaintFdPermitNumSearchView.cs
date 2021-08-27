using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintFdPermitNumSearchView : BaseEntity<int>
    {
        public virtual int FdEventId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string OrgName { get; set; }

        public virtual bool SubventedIndicator { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime? FlagDay { get; set; }

        public virtual string TWR { get; set; }

        public virtual string TwrDistrict { get; set; }
        
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
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgEditLatestPspFdView : BaseEntity<int>
    {
        public virtual string PspRef { get; set; }

        public virtual string PspContactPersonName { get; set; }

        public virtual string PspContactPersonEmailAddress { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string FdContactPersonName { get; set; }

        public virtual string FdContactPersonEmailAddress { get; set; }

        public virtual int OrgId { get; set; }

        public override int Id
        {
            get
            {
                return OrgId;
            }
            set
            {
                OrgId = value;
            }
        }
    }
}
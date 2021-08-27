using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspReportR14View :BaseEntity<string>
    {
        public virtual int SN { get; set; }

        public virtual DateTime? ReferralDate { get; set; }

        public virtual string ConcernOrgName { get; set; }

        public virtual string CorrespondenceEnclosureNum { get; set; }

        public virtual string Reminder { get; set; }

        public virtual string C1 { get; set; }

        public virtual string C2 { get; set; }

        public virtual string C3 { get; set; }

        public virtual string C4 { get; set; }

        public virtual string C5 { get; set; }

        public virtual string C6 { get; set; }

        public virtual string C7 { get; set; }

        public virtual string C8 { get; set; }

        public virtual string EnclosureNum { get; set; }

        public override string Id
        {
            get
            {
                return "";
            }
            set
            {
                
            }
        }
    }
}
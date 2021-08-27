using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgDocSummaryView : BaseEntity<int>
    {
        public virtual int OrgDocId { get; set; }

        public virtual string DocName { get; set; }

        public virtual string DocNum { get; set; }

        public virtual string VersionNum { get; set; }

        public virtual bool Enabled { get; set; }

        public override int Id
        {
            get
            {
                return OrgDocId;
            }
            set
            {
                OrgDocId = value;
            }
        }
    }
}
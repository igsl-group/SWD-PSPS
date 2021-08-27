using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class LegalAdviceDoc : BaseAuditEntity<int>
    {
        public virtual int LegalAdviceDocId { get; set; }

        public virtual string DocType { get; set; }

        public virtual string DocNum { get; set; }

        public virtual decimal VersionNum { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual string VersionRemark { get; set; }

        public virtual string DocStatus { get; set; }

        public override int Id
        {
            get
            {
                return LegalAdviceDocId;
            }
            set
            {
                LegalAdviceDocId = value;
            }
        }
    }
}
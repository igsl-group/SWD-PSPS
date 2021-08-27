using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintResult : BaseAuditEntity<int>
    {
        public virtual int ComplaintResultId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string NonComplianceNature { get; set; }

        public virtual string OtherNonComplianceNature { get; set; }

        public virtual string Result { get; set; }

        public virtual string ResultRemark { get; set; }

        public virtual string ResultRemarkHtml { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintResultId;
            }
            set
            {
                ComplaintResultId = value;
            }
        }
    }
}
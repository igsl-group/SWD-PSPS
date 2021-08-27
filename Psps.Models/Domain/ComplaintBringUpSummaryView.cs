using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintBringUpSummaryView : BaseEntity<string>
    {
        public virtual string OrgRef { get; set; }

        public virtual string OtherEngOrgName { get; set; }

        public virtual string OtherChiOrgName { get; set; }

        public virtual int ComplaintMasterId { get; set; }

        public virtual string ComplaintRef { get; set; }

        public virtual DateTime ComplaintDate { get; set; }

        public virtual string EngDescription { get; set; }

        public virtual string PermitConcern { get; set; }

        public virtual string ActionFileEnclosureNum { get; set; }

        public override string Id
        {
            get
            {
                return ComplaintRef;
            }
            set
            {
                ComplaintRef = value;
            }
        }
    }
}
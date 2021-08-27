using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgProvisionNotAdopt : BaseAuditEntity<int>
    {
        public virtual int OrgProvisionNotAdoptId { get; set; }

        public virtual int OrgRefGuidePromulgationId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual string ProvisionId { get; set; }

        public override int Id
        {
            get
            {
                return OrgProvisionNotAdoptId;
            }
            set
            {
                OrgProvisionNotAdoptId = value;
            }
        }
    }
}
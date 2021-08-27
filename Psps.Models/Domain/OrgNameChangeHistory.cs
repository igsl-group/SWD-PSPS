using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgNameChangeHistory : BaseAuditEntity<int>
    {
        public virtual int OrgNameChangeId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual DateTime ChangeDate { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual string Remarks { get; set; }

        public override int Id
        {
            get
            {
                return OrgNameChangeId;
            }
            set
            {
                OrgNameChangeId = value;
            }
        }
    }
}
using Psps.Core.Models;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class Role : BaseAuditEntity<string>
    {
        public Role()
        {
            Posts = new List<Post>();
            Functions = new List<Function>();
        }

        public virtual string RoleId { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<Post> Posts { get; set; }

        public virtual IList<Function> Functions { get; set; }

        public override string Id
        {
            get
            {
                return RoleId;
            }
            set
            {
                RoleId = value;
            }
        }
    }
}
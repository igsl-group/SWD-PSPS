using Psps.Core.Models;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class Function : BaseAuditEntity<string>
    {
        public Function()
        {
            Roles = new List<Role>();
        }

        public virtual string FunctionId { get; set; }

        public virtual string Module { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public override string Id
        {
            get
            {
                return FunctionId;
            }
            set
            {
                FunctionId = value;
            }
        }
    }
}
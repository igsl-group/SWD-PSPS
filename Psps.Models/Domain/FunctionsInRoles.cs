using Psps.Core.Models;

namespace Psps.Models.Domain
{
    public partial class FunctionsInRoles : BaseAuditEntity<int>
    {
        public FunctionsInRoles()
        {
        }

        public virtual int FunctionsInRolesId { get; set; }

        public virtual string RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual string FunctionId { get; set; }

        public virtual Function Function { get; set; }

        public override int Id
        {
            get
            {
                return FunctionsInRolesId;
            }
            set
            {
                FunctionsInRolesId = value;
            }
        }
    }
}
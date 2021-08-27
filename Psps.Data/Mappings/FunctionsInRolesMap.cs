using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class FunctionsInRolesMap : BaseAuditEntityMap<FunctionsInRoles, int>
    {
        protected override void MapId()
        {
            Id(x => x.FunctionsInRolesId).GeneratedBy.Identity().Column("FunctionsInRolesId");
        }

        protected override void MapEntity()
        {
            References(x => x.Function).Column("FunctionId");
            References(x => x.Role).Column("RoleId");
        }
    }
}
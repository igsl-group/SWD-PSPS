using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class PostsInRolesMap : BaseAuditEntityMap<PostsInRoles, int>
    {
        protected override void MapId()
        {
            Id(x => x.PostsInRolesId).GeneratedBy.Identity().Column("PostsInRolesId");
        }

        protected override void MapEntity()
        {
            //TODO: Why comment out b4?
            References(x => x.Post).Column("PostId").ReadOnly();
            //References(x => x.Role).Column("RoleId").ReadOnly();

            Map(x => x.PostId).Length(20).Not.Nullable();
            Map(x => x.RoleId).Length(30).Not.Nullable();
        }
    }
}
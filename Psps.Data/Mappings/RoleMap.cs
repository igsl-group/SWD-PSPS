using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class RoleMap : BaseAuditEntityMap<Role, string>
    {
        protected override void MapId()
        {
            Id(x => x.RoleId).GeneratedBy.Assigned().Column("RoleId").Length(30);
        }

        protected override void MapEntity()
        {
            Map(x => x.Description).Column("Description").Length(200);
            HasManyToMany(x => x.Posts).Table("PostsInRoles").ApplyChildFilter<DeletedFilter>();
            HasManyToMany(x => x.Functions).Table("FunctionsInRoles").ApplyChildFilter<DeletedFilter>();
        }
    }
}
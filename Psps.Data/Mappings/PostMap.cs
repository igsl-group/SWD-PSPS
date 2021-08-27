using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class PostMap : BaseAuditEntityMap<Post, string>
    {
        protected override void MapId()
        {
            Id(x => x.PostId).GeneratedBy.Assigned().Column("PostId").Length(20);
        }

        protected override void MapEntity()
        {
            References(x => x.Rank).Column("RankId");
            References(x => x.Owner).Column("OwnerUserId").Unique();
            References(x => x.Supervisor).Column("SupervisorPostId");
            HasMany(x => x.Supervisees).KeyColumn("SupervisorPostId").Inverse();
            HasMany(x => x.ActedOn).KeyColumn("PostIdToBeActed").Inverse();
            HasMany(x => x.ActedBy).KeyColumn("PostId").Inverse();
            HasManyToMany(x => x.Roles).Table("PostsInRoles").ApplyChildFilter<DeletedFilter>();
        }
    }
}
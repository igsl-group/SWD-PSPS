using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class RankMap : BaseAuditEntityMap<Rank, string>
    {
        protected override void MapId()
        {
            Id(x => x.RankId).GeneratedBy.Assigned().Column("RankId").Length(20).Length(20);
        }

        protected override void MapEntity()
        {
            Map(x => x.RankLevel).Column("RankLevel").Not.Nullable().Precision(10);
            HasMany(x => x.Posts).KeyColumn("RankId").Inverse();
        }
    }
}
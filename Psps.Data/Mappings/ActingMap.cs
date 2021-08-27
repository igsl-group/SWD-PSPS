using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ActingMap : BaseAuditEntityMap<Acting, int>
    {
        protected override void MapId()
        {
            Id(x => x.ActingId).GeneratedBy.Identity().Column("ActingId");
        }

        protected override void MapEntity()
        {
            References(x => x.User).Column("UserId");
            References(x => x.Post).Column("PostId");
            References(x => x.PostToBeActed).Column("PostIdToBeActed");
            Map(x => x.EffectiveFrom).Column("EffectiveFrom").Not.Nullable();
            Map(x => x.EffectiveTo).Column("EffectiveTo");
        }
    }
}
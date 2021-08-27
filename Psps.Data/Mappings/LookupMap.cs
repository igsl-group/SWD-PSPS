using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class LookupMap : BaseAuditEntityMap<Lookup, int>
    {
        protected override void MapId()
        {
            Id(x => x.LookupId).GeneratedBy.Identity().Column("LookupId");
        }

        protected override void MapEntity()
        {
            Map(x => x.Type).Column("Type").Not.Nullable().Length(30);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(20);
            Map(x => x.EngDescription).Column("EngDescription").Not.Nullable().Length(200);
            Map(x => x.ChiDescription).Column("ChiDescription").Length(200);
            Map(x => x.DisplayOrder).Column("DisplayOrder").Not.Nullable().Precision(10);
            Map(x => x.IsActive).Column("IsActive");
        }
    }
}
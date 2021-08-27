using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class SystemMessageMap : BaseAuditEntityMap<SystemMessage, int>
    {
        protected override void MapId()
        {
            Id(x => x.SystemMessageId).GeneratedBy.Identity().Column("SystemMessageId");
        }

        protected override void MapEntity()
        {
            Map(x => x.Code).Column("Code").Not.Nullable().Length(100);
            Map(x => x.Description).Column("Description").Not.Nullable().Length(200);
            Map(x => x.Value).Column("Value").Not.Nullable().Length(255);
        }
    }
}
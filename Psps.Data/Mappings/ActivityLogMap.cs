using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ActivityLogMap : BaseEntityMap<ActivityLog, int>
    {
        protected override void MapId()
        {
            Id(x => x.LogId).GeneratedBy.Identity().Column("LogId");
        }

        protected override void MapEntity()
        {
            References(x => x.User).Column("ActionedById");
            References(x => x.Post).Column("ActionedByPost");
            Map(x => x.RecordKey).Column("RecordKey").Length(10);
            Map(x => x.Activity).Column("Activity").Not.Nullable().Length(30);
            Map(x => x.Action).Column("Action").Not.Nullable().Length(20); ;
            Map(x => x.Remark).Column("Remark").Length(1000);
            Map(x => x.ActionedOn).Column("ActionedOn").Not.Nullable();
        }
    }
}
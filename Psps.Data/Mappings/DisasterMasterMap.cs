using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class DisasterMasterMap : BaseAuditEntityMap<DisasterMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.DisasterMasterId).GeneratedBy.Identity().Column("DisasterMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.DisasterName).Column("DisasterName").Not.Nullable().Length(250);
            Map(x => x.ChiDisasterName).Column("ChiDisasterName").Length(100);
            Map(x => x.DisasterDate).Column("DisasterDate");
            Map(x => x.BeginDate).Column("BeginDate").Not.Nullable();
            Map(x => x.EndDate).Column("EndDate");
            HasMany(x => x.DisasterStatistics).KeyColumn("DisasterMasterId").Inverse();
        }
    }
}
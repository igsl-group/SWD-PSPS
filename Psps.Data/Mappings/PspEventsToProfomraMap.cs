using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    //public partial class PspEventsToProfomraMap : BaseAuditEntityMap<PspEventsToProforma, string>
    public partial class PspEventsToProfomraMap : BaseEntityMap<PspEventsToProforma, string>
    {
        protected override void MapId()
        {
            CompositeId().KeyProperty(x => x.EventStartYear, "EventStartYear")
                         .KeyProperty(x => x.EventStartMonth, "EventStartMonth")
                         .KeyProperty(x => x.EventDays, "EventDays")
                         .KeyProperty(x => x.EventStartTime, "EventStartTime")
                         .KeyProperty(x => x.EventEndTime, "EventEndTime")
                         .KeyProperty(x => x.District, "District")
                         .KeyProperty(x => x.Location, "Location")
                         .KeyProperty(x => x.ChiLocation, "ChiLocation")
                         .KeyProperty(x => x.CollectionMethod, "CollectionMethod")
                         .KeyReference(x => x.PspMaster, "PspMasterId")
                         ;
        }

        protected override void MapEntity()
        {
            //Map(x => x.PspMasterId).Column("PspMasterId");
            References(x => x.PspMaster).Column("PspMasterId");
            Map(x => x.EventStartYear).Column("EventStartYear");
            Map(x => x.EventStartMonth).Column("EventStartMonth");
            Map(x => x.EventDays).Column("EventDays");
            Map(x => x.EventStartTime).Column("EventStartTime");
            Map(x => x.EventEndTime).Column("EventEndTime");
            Map(x => x.Location).Column("Location");
            Map(x => x.ChiLocation).Column("ChiLocation");
            Map(x => x.District).Column("District");
            Map(x => x.CollectionMethod).Column("CollectionMethod");
        }
    }
}
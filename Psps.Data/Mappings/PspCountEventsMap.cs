using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspCountEventsMap : BaseEntityMap<PspCountEvents, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspMasterId).GeneratedBy.Identity().Column("PspMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.PspMasterId).Column("PspMasterId").Not.Nullable();
            Map(x => x.EventStartDate).Column("EventStartDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.TotEvents).Column("TotEvents");
            Map(x => x.PermitNum).Column("PermitNum");
        }
    }
}
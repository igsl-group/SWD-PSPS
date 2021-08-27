using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspApplicationStatusViewMap : BaseEntityMap<PspApplicationStatusView, string>
    {
        protected override void MapId()
        {
            Id(x => x.Year).Column("Year");
        }

        protected override void MapEntity()
        {
            Map(x => x.Type).Column("Type");
           // Map(x => x.Year).Column("Year");
            Map(x => x.ApplicationReceived).Column("ApplicationReceived");
            Map(x => x.PSPIssued).Column("PSPIssued");
            Map(x => x.RejectedApplication).Column("RejectedApplication");
            Map(x => x.ApplicationWithdrawn).Column("ApplicationWithdrawn");
            Map(x => x.TwoBatchApplication).Column("TwoBatchApplication");
            Map(x => x.OverdueAC).Column("OverdueAC");
        }
    }
}
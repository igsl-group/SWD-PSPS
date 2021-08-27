using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspBringUpSummaryViewMap : BaseEntityMap<PspBringUpSummaryView, string>
    {
        protected override void MapId()
        {
            Id(x => x.PspRef).Column("PspRef");
        }

        protected override void MapEntity()
        {
            Map(x => x.Resubmit).Column("Resubmit");
            Map(x => x.PspMasterId).Column("PspMasterId");
            Map(x => x.EngOrgName).Column("EngOrgName");
            // Map(x => x.Year).Column("Year");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.PspEventDate).Column("PspEventDate");
            Map(x => x.ProcessingOfficerPost).Column("ProcessingOfficerPost");
            Map(x => x.SpecialRemark).Column("SpecialRemark");
        }
    }
}
using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintBringUpSummaryViewMap : BaseEntityMap<ComplaintBringUpSummaryView, string>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintRef).Column("ComplaintRef");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgRef).Column("OrgRef");
            Map(x => x.OtherEngOrgName).Column("OtherEngOrgName");
            Map(x => x.OtherChiOrgName).Column("OtherChiOrgName");
            Map(x => x.ComplaintMasterId).Column("ComplaintMasterId");
            Map(x => x.ComplaintDate).Column("ComplaintDate");
            Map(x => x.EngDescription).Column("EngDescription");
            Map(x => x.PermitConcern).Column("PermitConcern");
            Map(x => x.ActionFileEnclosureNum).Column("ActionFileEnclosureNum");
        }
    }
}
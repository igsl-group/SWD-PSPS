using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintFdPermitNumSearchViewMap : BaseEntityMap<ComplaintFdPermitNumSearchView, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdEventId).GeneratedBy.Identity().Column("FdEventId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgRef).Column("OrgRef");
            Map(x => x.FdRef).Column("FdRef");
            Map(x => x.OrgName).Column("OrgName");
            Map(x => x.SubventedIndicator).Column("SubventedIndicator");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.FlagDay).Column("FlagDay");
            Map(x => x.TWR).Column("TWR");
            Map(x => x.TwrDistrict).Column("TwrDistrict");
        }
    }
}
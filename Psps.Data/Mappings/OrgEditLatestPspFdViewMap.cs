using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgEditLatestPspFdViewMap : BaseEntityMap<OrgEditLatestPspFdView, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgId).GeneratedBy.Identity().Column("OrgId");
        }

        protected override void MapEntity()
        {
            Map(x => x.PspRef).Column("PspRef");
            Map(x => x.PspContactPersonName).Column("PspContactPersonName");
            Map(x => x.PspContactPersonEmailAddress).Column("PspContactPersonEmailAddress");
            Map(x => x.FdRef).Column("FdRef");
            Map(x => x.FdContactPersonName).Column("FdContactPersonName");
            Map(x => x.FdContactPersonEmailAddress).Column("FdContactPersonEmailAddress");
        }
    }
}
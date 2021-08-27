using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdListMap : BaseAuditEntityMap<FdList, int>
    {
        protected override void MapId()
        {
            Id(x => x.FlagDayListId).GeneratedBy.Identity().Column("FlagDayListId");
        }

        protected override void MapEntity()
        {
            Map(x => x.FlagDayYear).Column("FlagDayYear").Length(5).Not.Nullable();
            Map(x => x.FlagDayType).Column("FlagDayType").Length(1);
            Map(x => x.FlagDayDate).Column("FlagDayDate").Not.Nullable();
        }
    }
}
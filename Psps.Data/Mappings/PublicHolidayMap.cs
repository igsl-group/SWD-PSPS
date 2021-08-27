using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PublicHolidayMap : BaseAuditEntityMap<PublicHoliday, int>
    {
        protected override void MapId()
        {
            Id(x => x.HolidayId).GeneratedBy.Identity().Column("HolidayId");
        }

        protected override void MapEntity()
        {
            Map(x => x.HolidayYear).Column("HolidayYear").Not.Nullable().Precision(10);
            Map(x => x.HolidayDate).Column("HolidayDate").Not.Nullable();
        }
    }
}
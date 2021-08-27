using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspReportR14ViewMap : BaseEntityMap<PspReportR14View, string>
    {
        protected override void MapId()
        {
            Id(x => x.SN).GeneratedBy.Identity().Column("SN");
        }
        protected override void MapEntity()
        {
            Map(x => x.ReferralDate).Column("ReferralDate");
            Map(x => x.ConcernOrgName).Column("ConcernOrgName").Length(300);
            Map(x => x.CorrespondenceEnclosureNum).Column("CorrespondenceEnclosureNum").Length(100);
            Map(x => x.Reminder).Column("Reminder").Not.Nullable().Length(1);
            Map(x => x.C1).Column("C1").Not.Nullable().Length(1);
            Map(x => x.C2).Column("C2").Not.Nullable().Length(1);
            Map(x => x.C3).Column("C3").Not.Nullable().Length(1);
            Map(x => x.C4).Column("C4").Not.Nullable().Length(1);
            Map(x => x.C5).Column("C5").Not.Nullable().Length(1);
            Map(x => x.C6).Column("C6").Not.Nullable().Length(1);
            Map(x => x.C7).Column("C7").Not.Nullable().Length(1);
            Map(x => x.C8).Column("C8").Not.Nullable().Length(1);
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(100);
        }
    }
}
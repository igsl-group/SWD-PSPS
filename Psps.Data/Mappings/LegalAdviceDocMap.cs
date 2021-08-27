using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class LegalAdviceDocMap : BaseAuditEntityMap<LegalAdviceDoc, int>
    {
        protected override void MapId()
        {
            Id(x => x.LegalAdviceDocId).GeneratedBy.Identity().Column("LegalAdviceDocId");
        }

        protected override void MapEntity()
        {
            Map(x => x.DocType).Column("DocType").Not.Nullable().Length(6);
            Map(x => x.DocNum).Column("DocNum").Not.Nullable().Length(6);
            Map(x => x.VersionNum).Column("VersionNum").Not.Nullable().Precision(3).Scale(1);
            Map(x => x.FileLocation).Column("FileLocation").Not.Nullable().Length(400);
            Map(x => x.VersionRemark).Column("VersionRemark").Length(1000);
            Map(x => x.DocStatus).Column("DocStatus").Not.Nullable().Length(1);
        }
    }
}
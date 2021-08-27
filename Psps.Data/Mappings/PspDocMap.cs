using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspDocMap : BaseAuditEntityMap<PspDoc, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspDocId).GeneratedBy.Identity().Column("PspDocId");
        }

        protected override void MapEntity()
        {
            Map(x => x.DocNum).Column("DocNum").Not.Nullable().Length(6);
            Map(x => x.DocName).Column("DocName").Not.Nullable().Length(256);
            Map(x => x.VersionNum).Column("VersionNum").Not.Nullable().Length(10);
            Map(x => x.FileLocation).Column("FileLocation").Not.Nullable().Length(400);
            Map(x => x.DocStatus).Column("DocStatus").Not.Nullable();
        }
    }
}
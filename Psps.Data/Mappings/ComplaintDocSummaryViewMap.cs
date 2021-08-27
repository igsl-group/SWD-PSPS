using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintDocSummaryViewMap : BaseEntityMap<ComplaintDocSummaryView, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintDocId).GeneratedBy.Identity().Column("ComplaintDocId");
        }

        protected override void MapEntity()
        {
            Map(x => x.DocNum).Column("DocNum").Not.Nullable().Length(6);
            Map(x => x.DocName).Column("DocName").Not.Nullable().Length(256);
            Map(x => x.VersionNum).Column("VersionNum").Not.Nullable().Length(10);
            Map(x => x.Enabled).Column("Enabled").Not.Nullable();
        }
    }
}
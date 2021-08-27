using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class LetterMap : BaseAuditEntityMap<Letter, int>
    {
        protected override void MapId()
        {
            Id(x => x.LetterId).GeneratedBy.Identity().Column("LetterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.Name).Column("Name").Not.Nullable().Length(100);
            Map(x => x.Description).Column("Description").Not.Nullable().Length(200);
            Map(x => x.Path).Column("Path").Not.Nullable().Length(400);
            Map(x => x.Version).Column("Version").Not.Nullable().Length(10);
            Map(x => x.IsActive).Column("IsActive").Not.Nullable();
        }
    }
}
using FluentNHibernate.Mapping;
using Lfpis.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Mappings
{
    public partial class RevChangeMap : ClassMap<RevChange>
    {
        public RevChangeMap()
            : base()
        {
            Id(x => x.RevChangeId).Column("RevChangeId").GeneratedBy.Identity();
            References(x => x.RevInfo);
            Map(x => x.RevisionType).Column("RevType").CustomType<int>().Not.Nullable();
            Map(x => x.EntityId).Column("EntityId").Not.Nullable();
            Map(x => x.EntityName).Column("EntityName").Not.Nullable();
        }
    }
}
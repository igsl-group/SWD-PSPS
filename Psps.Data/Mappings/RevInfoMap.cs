using FluentNHibernate.Mapping;
using Lfpis.Models.Domain;
using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class RevInfoMap : ClassMap<RevInfo>
    {
        public RevInfoMap()
            : base()
        {
            Id(x => x.RevInfoId).GeneratedBy.Identity().Column("RevInfoId");
            Map(x => x.RevisionedOn).Column("RevisionedOn").Not.Nullable();
            Map(x => x.RevisionedById).Column("RevisionedById").Not.Nullable().Length(20);
            Map(x => x.RevisionedByPost).Column("RevisionedByPost").Not.Nullable().Length(20);
            HasMany(x => x.RevisionChanges).KeyColumn("RevInfoId").Cascade.Persist();
        }
    }
}
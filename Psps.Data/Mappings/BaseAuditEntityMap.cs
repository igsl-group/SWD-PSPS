using FluentNHibernate.Mapping;
using Psps.Core.Common;
using Psps.Core.Models;

namespace Psps.Data.Mappings
{
    public abstract class BaseAuditEntityMap<T, TPk> : ClassMap<T> where T : IAuditable, IEntity<TPk>
    {
        public BaseAuditEntityMap()
        {
            ApplyFilter();
            MapId();
            MapEntity();
            Map(x => x.IsDeleted).Column("IsDeleted").Not.Nullable();
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.CreatedById).Column("CreatedById").Not.Nullable().Length(20);
            Map(x => x.CreatedByPost).Column("CreatedByPost").Not.Nullable().Length(20);
            Map(x => x.UpdatedOn).Column("UpdatedOn");
            Map(x => x.UpdatedById).Column("UpdatedById").Length(20);
            Map(x => x.UpdatedByPost).Column("UpdatedByPost").Length(20);
            Version(x => x.RowVersion).Column("RowVersion").Not.Nullable();
        }

        protected void ApplyFilter()
        {
            ApplyFilter<DeletedFilter>();
        }

        protected abstract void MapId();

        protected abstract void MapEntity();
    }
}
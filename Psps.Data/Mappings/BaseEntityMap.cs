using FluentNHibernate.Mapping;
using Psps.Core.Models;

namespace Psps.Data.Mappings
{
    public abstract class BaseEntityMap<T, TPk> : ClassMap<T> where T : IEntity<TPk>
    {
        public BaseEntityMap()
        {
            MapId();
            MapEntity();
        }

        protected abstract void MapId();

        protected abstract void MapEntity();
    }
}
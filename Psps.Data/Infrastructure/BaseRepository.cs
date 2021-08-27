using NHibernate;
using NHibernate.Linq;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Psps.Data.Infrastructure
{
    public abstract class BaseRepository<T, TPk> : IRepository<T, TPk> where T : BaseEntity<TPk>
    {
        private readonly ISession _session;

        public BaseRepository(ISession session)
        {
            this._session = session;
            this._session.FlushMode = FlushMode.Auto;
        }

        public ISession Session { get { return this._session; } }

        public IQueryable<T> Table
        {
            get
            {
                return this._session.Query<T>();
            }
        }

        public bool Add(T entity)
        {
            this._session.Save(entity);
            return true;
        }

        public bool Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                this._session.Save(item);
            }
            return true;
        }

        public bool Update(T entity)
        {
            this._session.Update(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            this._session.Delete(entity);
            this._session.Flush();
            return true;
        }

        public bool Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                this.Delete(entity);
            }
            return true;
        }

        public T GetById(TPk id, bool includeDeleted = false)
        {
            var entity = this._session.Get<T>(id);

            if (!includeDeleted && entity as IAuditable != null)
            {
                if ((entity as IAuditable).IsDeleted) return null;
            }

            return entity;
        }

        public IEnumerable<T> GetAll(params string[] includeProperties)
        {
            var query = this.Table;

            if (includeProperties != null && includeProperties.Any())
                query = query.Include(includeProperties);

            return query.ToList();
        }

        public T Get(Expression<System.Func<T, bool>> expression, params string[] includeProperties)
        {
            return GetMany(expression, includeProperties).SingleOrDefault();
        }

        public IEnumerable<T> GetMany(Expression<System.Func<T, bool>> expression, params string[] includeProperties)
        {
            var query = this.Table;

            if (includeProperties != null && includeProperties.Any())
                query = query.Include(includeProperties);

            return query.Where(expression).ToList();
        }

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <param name="gridSettings">jqGrid Parameters</param>
        /// <returns></returns>
        public virtual IPagedList<T> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<T>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<T>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public virtual IQueryOver<T, T> QueryOver()
        {
            return this._session.QueryOver<T>();
        }
    }
}
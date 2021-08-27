using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Psps.Data.Infrastructure
{
    public interface IRepository<T, TPk> where T : BaseEntity<TPk>
    {
        ISession Session { get; }

        IQueryable<T> Table { get; }

        T Get(Expression<Func<T, bool>> expression, params string[] includeProperties);

        IEnumerable<T> GetMany(Expression<Func<T, bool>> expression, params string[] includeProperties);

        bool Add(T entity);

        bool Add(IEnumerable<T> items);

        bool Update(T entity);

        bool Delete(T entity);

        bool Delete(IEnumerable<T> entities);

        T GetById(TPk id, bool includeDeleted = false);

        IEnumerable<T> GetAll(params string[] includeProperties);

        IPagedList<T> GetPage(GridSettings grid);

        IQueryOver<T, T> QueryOver();
    }
}
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IOrgFdTabGridViewRepository : IRepository<OrgFdTabGridView, int>
    {
        //IPagedList<OrgFdTabGridView> GetPage(GridSettings grid);
    }

    public class OrgFdTabGridViewRepository : BaseRepository<OrgFdTabGridView, int>, IOrgFdTabGridViewRepository
    {
        public OrgFdTabGridViewRepository(ISession session)
            : base(session)
        {
        }

        //public IPagedList<OrgFdTabGridView> GetPage(GridSettings grid)
        //{
        //    var query = this.Table;

        //    //filtring
        //    if (grid.IsSearch)
        //        query = query.Where(grid.Where);

        //    //sorting
        //    if (!string.IsNullOrEmpty(grid.SortColumn))
        //        query = query.OrderBy<OrgFdTabGridView>(grid.SortColumn, grid.SortOrder);

        //    var page = new PagedList<OrgFdTabGridView>(query, grid.PageIndex, grid.PageSize);

        //    return page;
        //}
    }
}
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IActingRepository : IRepository<Acting, int>
    {

    }

    public class ActingRepository : BaseRepository<Acting, int>, IActingRepository
    {
        public ActingRepository(ISession session)
            : base(session)
        {
        }

        public override IPagedList<Acting> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //eager fetching
            query = query.Fetch(x => x.User);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<Acting>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<Acting>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

    }
}
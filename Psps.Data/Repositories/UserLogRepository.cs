using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Psps.Core.JqGrid.Extensions;

namespace Psps.Data.Repositories
{
    public interface IUserLogRepository : IRepository<ActivityLog, int>
    {
    }

    public class UserLogRepository : BaseRepository<ActivityLog, int>, IUserLogRepository
    {
        public UserLogRepository(ISession session)
            : base(session)
        {
        }


        public override IPagedList<ActivityLog> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //eager fetching
            query = query.Fetch(x => x.User);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ActivityLog>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ActivityLog>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
      
    }
}
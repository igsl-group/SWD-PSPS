using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
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
    public interface IFlagDayListRepository : IRepository<FdList, int>
    {
        FdList GetFlagDayListId(int FdListId);
    }

    public class FlagDayListRepository : BaseRepository<FdList, int>, IFlagDayListRepository
    {
        public FlagDayListRepository(ISession session)
            : base(session)
        {
        }

        public FdList GetFlagDayListId(int FlagDayListId)
        {
            return this.Table.Where(x => x.FlagDayListId == FlagDayListId).FirstOrDefault();
        }

        public override IPagedList<FdList> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
            {
                query = query.Where(grid.Where);
            }

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdList>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdList>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
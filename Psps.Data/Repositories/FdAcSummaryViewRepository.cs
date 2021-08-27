using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Core.Helper;
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
    public interface IFdAcSummaryViewRepository : IRepository<FdAcSummaryView, int>
    {
        IPagedList<FdAcSummaryView> GetPage(GridSettings grid, string permitNum, string flagDay);
    }

    public class FdAcSummaryViewRepository : BaseRepository<FdAcSummaryView, int>, IFdAcSummaryViewRepository
    {
        public FdAcSummaryViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<FdAcSummaryView> GetPage(GridSettings grid, string permitNum, string flagDay)
        {
            var query = this.Table;

            //filtring
            if (!String.IsNullOrEmpty(permitNum))
            {
                query = query.Where(x => x.FdEvent.Any(y => SqlMethods.Like(y.PermitNum, "%" + permitNum + "%") && y.IsDeleted == false));
            }
            if (!String.IsNullOrEmpty(flagDay))
            {
                query = query.Where(x => x.FdEvent.Any(y => y.FlagDay == CommonHelper.ConvertStringToDateTime(flagDay) && y.IsDeleted == false));
            }
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdAcSummaryView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdAcSummaryView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
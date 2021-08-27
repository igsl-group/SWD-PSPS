using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IPspAcSummaryViewRepository : IRepository<PspAcSummaryView, int> 
    {
        IPagedList<PspAcSummaryView> GetPage(GridSettings grid, string permitNum);
    }

    public class PspAcSummaryViewRepository : BaseRepository<PspAcSummaryView, int>, IPspAcSummaryViewRepository
    {
        public PspAcSummaryViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<PspAcSummaryView> GetPage(GridSettings grid, string permitNum)
        {
            var query = this.Table;
            //filtring
            var currentDateTime = DateTime.Now; ;
            if (!String.IsNullOrEmpty(permitNum))
            {
                query = query.Where(x => x.PspApprovalHistory.Any(y => SqlMethods.Like(y.PermitNum, "%" + permitNum + "%")));
            }

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspAcSummaryView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspAcSummaryView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
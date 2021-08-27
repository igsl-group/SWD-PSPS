using NHibernate;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IOrgEditPspViewRepository : IRepository<OrgEditPspView, int> { }

    public class OrgEditPspViewRepository : BaseRepository<OrgEditPspView, int>, IOrgEditPspViewRepository
    {
        public OrgEditPspViewRepository(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <param name="gridSettings">jqGrid Parameters</param>
        /// <returns></returns>
        public override IPagedList<OrgEditPspView> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<OrgEditPspView>(grid.SortColumn, grid.SortOrder);

            if (string.IsNullOrEmpty(grid.SortColumn) || grid.SortColumn != "pspRef")
                query = query.OrderByDescending(x => x.PspRef);

            var page = new PagedList<OrgEditPspView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
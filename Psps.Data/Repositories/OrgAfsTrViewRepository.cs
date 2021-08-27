using NHibernate;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IOrgAfsTrViewRepository : IRepository<OrgAfsTrView, string>
    {
    }

    public class OrgAfsTrViewRepository : BaseRepository<OrgAfsTrView, string>, IOrgAfsTrViewRepository
    {
        public OrgAfsTrViewRepository(ISession session)
            : base(session)
        {
        }

        //public IPagedList<OrgAfsTrView> GetPage(GridSettings grid)
        //{
        //    var query = this.Table;

        //    //eager fetching
        //    query = query.Fetch(x => x.User);

        //    //filtring
        //    if (grid.IsSearch)
        //        query = query.Where(grid.Where);

        //    //sorting
        //    if (!string.IsNullOrEmpty(grid.SortColumn))
        //        query = query.OrderBy<Acting>(grid.SortColumn, grid.SortOrder);

        //    var page = new PagedList<Acting>(query, grid.PageIndex, grid.PageSize);

        //    return page;
        //}
    }
}
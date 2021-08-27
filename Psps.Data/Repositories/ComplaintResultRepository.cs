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
using Psps.Models.Dto.Disaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IComplaintResultRepository : IRepository<ComplaintResult, int>
    {
        IPagedList<ComplaintResult> GetPageByComplaintMasterId(GridSettings grid, int complaintMasterId);
    }

    public class ComplaintResultRepository : BaseRepository<ComplaintResult, int>, IComplaintResultRepository
    {
        public ComplaintResultRepository(ISession session)
            : base(session)
        {
            //this.Session = session;
        }

        public IPagedList<ComplaintResult> GetPageByComplaintMasterId(GridSettings grid, int complaintMasterId)
        {
            var query = this.Table.Where(x => x.ComplaintMaster.ComplaintMasterId == complaintMasterId);

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintResult>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintResult>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
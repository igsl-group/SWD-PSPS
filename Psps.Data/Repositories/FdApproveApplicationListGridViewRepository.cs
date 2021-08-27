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
using Psps.Models.Dto.FdMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IFdApproveApplicationListGridViewRepository : IRepository<FdApproveApplicationListGridView, int>
    {
        IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear);
    }

    public class FdApproveApplicationListGridViewRepository : BaseRepository<FdApproveApplicationListGridView, int>, IFdApproveApplicationListGridViewRepository
    {
        public FdApproveApplicationListGridViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear)
        {
            var query = (from u in this.Table.Where(x => x.FdYear == fdYear)
                         select new FdApplicationListDto
                         {
                             FdMasterId = u.FdMasterId,  
                             FdRef = u.FdRef,
                             OrgName = u.OrgName,
                             FlagDay = u.FlagDay,
                             TWR = u.TWR,
                             TwrDistrict = u.TwrDistrict,
                             PermitNo = u.PermitNum,
                             ApproveRemarks = u.ApproveRemarks,
                             FrasResponse = u.FrasResponse,
                             Approve = u.Approve,
                             FdEventId = u.FdEventId,
                             RowVersion = u.RowVersion,
                             PermitRevokeIndicator = u.PermitRevokeIndicator
                         }).AsQueryable();

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdApplicationListDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdApplicationListDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
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
using Psps.Models.Dto.FdStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IFdApprovalHistoryRepository : IRepository<FdApprovalHistory, string>
    {
        FdApprovalHistory GetFdApprovalHistoryById(string fdYear);

        IPagedList<FdStatusSummary> GetFdStatus(GridSettings grid);
    }

    public class FdApprovalHistoryRepository : BaseRepository<FdApprovalHistory, string>, IFdApprovalHistoryRepository
    {
        public FdApprovalHistoryRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<FdStatusSummary> GetFdStatus(GridSettings grid)
        {
            string sql = "SELECT FM.FdYear, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' then FE.FlagDay end) As OrgTWR, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' then FE.FlagDay end) As OrgRFD, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' then 1 end) as ApplyTWR, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' then 1 end) as ApplyRFD, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' and FM.ApplicationResult IN ('1', '3', '4') and FdGroup = '1' then FM.FdMasterId end) as EligibleTWR_A, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' and FM.ApplicationResult IN ('1', '3', '4') and FdGroup = '2' then FM.FdMasterId end) as EligibleTWR_B, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' and FM.ApplicationResult IN ('1', '3', '4') and FdGroup = '1' then FM.FdMasterId end) as EligibleRFD_A, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' and FM.ApplicationResult IN ('1', '3', '4') and FdGroup = '2' then FM.FdMasterId end) as EligibleRFD_B, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' and FM.ApplicationResult = '2' then FM.FdMasterId end) as IneligibleTWR, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' and FM.ApplicationResult = '2' then FM.FdMasterId end) as IneligibleRFD, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '1' and FM.ApplicationResult = '5' then FM.FdMasterId end) as LateTWR, " + Environment.NewLine +
                         "       count(case when FM.ApplyForTwr = '2' and FM.ApplicationResult = '5' then FM.FdMasterId end) as LateRFD " + Environment.NewLine +
                         "FROM FdMaster FM " + Environment.NewLine +
                         "LEFT JOIN FdEvent FE on FM.FdMasterId = FE.FdMasterId and FE.FlagDay IS NOT NULL and ISNULL(FE.PermitRevokeIndicator, 0) != 1 and FE.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE FM.IsDeleted = 0 " + Environment.NewLine +
                         "GROUP BY FM.FdYear " + Environment.NewLine +
                         "ORDER BY FM.FdYear DESC";

            var query = this.Session.CreateSQLQuery(sql);

            IList<FdStatusSummary> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(FdStatusSummary)))               
               .List<FdStatusSummary>();

            if (!string.IsNullOrEmpty(grid.SortColumn))
                list = list.AsQueryable().OrderBy<FdStatusSummary>(grid.SortColumn, grid.SortOrder).ToList();

            return new PagedList<FdStatusSummary>(list.AsQueryable(), grid.PageIndex, grid.PageSize);
        }

        public FdApprovalHistory GetFdApprovalHistoryById(string fdYear)
        {
            return this.Table.Where(x => x.FdYear == fdYear).FirstOrDefault();
        }

        public override IPagedList<FdApprovalHistory> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdApprovalHistory>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdApprovalHistory>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
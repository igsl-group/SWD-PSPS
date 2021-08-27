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
using Psps.Models.Dto.Psp;
using Psps.Models.Dto.PspMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IPspRecommendEventsViewRepository : IRepository<PspRecommendEventsView, int>
    {
        IPagedList<PspRecommendEventsDto> GetPagePspRecommendDto(GridSettings grid);
    }

    public class PspRecommendEventsViewRepository : BaseRepository<PspRecommendEventsView, int>, IPspRecommendEventsViewRepository
    {
        public PspRecommendEventsViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<PspRecommendEventsDto> GetPagePspRecommendDto(GridSettings grid)
        {
            var query = from u in this.Table
                        select new PspRecommendEventsDto
                        {
                            PspYear = u.PspRef.Substring(4, 4),
                            PspMasterId = u.PspMasterId,
                            OtherEngOrgName = u.OtherEngOrgName,
                            PspRef = u.PspRef,
                            PermitNum = u.PermitNum,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            ApprovalType = u.ApprovalType,
                            TotEventsToBeApproved = u.TotEventsToBeApproved,
                            RejectionLetterDate = u.RejectionLetterDate,
                            PermitIssueDate = u.PermitIssueDate,
                            CancelReason = u.CancelReason,
                            PspApprovalHistoryId = u.PspApprovalHistoryId,
                            EngOrgNameSorting = u.EngOrgNameSorting,
                            ChiOrgName = u.ChiOrgName,
                            ProcessingOfficerPost = u.ProcessingOfficerPost,
                        };

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspRecommendEventsDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspRecommendEventsDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
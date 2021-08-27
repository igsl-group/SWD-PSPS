using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IReferenceGuideSearchViewRepository : IRepository<ReferenceGuideSearchView, int>
    {
        IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid, 
                                                                               String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                               String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate,
                                                                               List<string> appliedFDBeforeFdYears, IList<string> fdIssuedBeforeFdYears, string appliedFDBeforeId, string fdIssuedBeforeId,
                                                                               String appliedSSAFBefore, DateTime? fromSSAFApplicationDate, DateTime? toSSAFApplicationDate,
                                                                               String ssafIssuedBefore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate,
                                                                               String referenceGuideActivityConcern);
    }

    public class ReferenceGuideSearchViewRepository : BaseRepository<ReferenceGuideSearchView, int>, IReferenceGuideSearchViewRepository
    {
        public ReferenceGuideSearchViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid, 
                                                                                      String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                                      String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate,
                                                                                      List<string> appliedFDBeforeFdYears, IList<string> fdIssuedBeforeFdYears, string appliedFDBeforeId, string fdIssuedBeforeId,
                                                                                      String appliedSSAFBefore, DateTime? fromSSAFApplicationDate, DateTime? toSSAFApplicationDate,
                                                                                      String ssafIssuedBefore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate,
                                                                                      String referenceGuideActivityConcern)
        {
            var query = this.Table;

            //filtring
            if (!String.IsNullOrEmpty(appliedPspBefore))
            {
                if (fromPspAppRecDate.HasValue && toPspAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.ApplicationReceiveDate >= fromPspAppRecDate && y.ApplicationReceiveDate <= toPspAppRecDate) == (appliedPspBefore.Equals("1") ? true : false));
                else if (fromPspAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.ApplicationReceiveDate >= fromPspAppRecDate) == (appliedPspBefore.Equals("1") ? true : false));
                else if (toPspAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.ApplicationReceiveDate <= toPspAppRecDate) == (appliedPspBefore.Equals("1") ? true : false));
            }

            if (!String.IsNullOrEmpty(pspIssuedBfore))
            {
                if (fromPspPermitIssueDate.HasValue && toPspPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromPspPermitIssueDate && z.PermitIssueDate.Value <= toPspPermitIssueDate)) == (pspIssuedBfore.Equals("1") ? true : false));
                else if (fromPspPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromPspPermitIssueDate)) == (pspIssuedBfore.Equals("1") ? true : false));
                else if (toPspPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => !y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value <= toPspPermitIssueDate)) == (pspIssuedBfore.Equals("1") ? true : false));
            }

            //// The ( == "2") (No) part is not correct
            if (!String.IsNullOrEmpty(appliedFDBeforeId))
            {
                if (appliedFDBeforeFdYears != null)
                {
                    if (appliedFDBeforeId == "1")
                    {
                        foreach (string fdYear in appliedFDBeforeFdYears)
                        {
                            query = query.Where(x => x.AppliedFDBeforeYears.Contains(fdYear));
                        }
                    }
                    else if (appliedFDBeforeId == "2")
                    {
                        foreach (string fdYear in appliedFDBeforeFdYears)
                        {
                            query = query.Where(x => !x.AppliedFDBeforeYears.Contains(fdYear));
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(fdIssuedBeforeId))
            {
                if (fdIssuedBeforeFdYears != null)
                {
                    if (fdIssuedBeforeId == "1")
                    {
                        foreach (string fdYear in fdIssuedBeforeFdYears)
                        {
                            query = query.Where(x => x.FDPermitIssuedBeforeYears.Contains(fdYear));
                        }
                    }
                    else if (fdIssuedBeforeId == "2")
                    {
                        foreach (string fdYear in fdIssuedBeforeFdYears)
                        {
                            query = query.Where(x => !x.FDPermitIssuedBeforeYears.Contains(fdYear));
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(appliedSSAFBefore))
            {
                if (fromSSAFApplicationDate.HasValue && toSSAFApplicationDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate >= fromSSAFApplicationDate && y.ApplicationReceiveDate <= toSSAFApplicationDate) == (appliedSSAFBefore.Equals("1") ? true : false));
                else if (fromSSAFApplicationDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate >= fromSSAFApplicationDate) == (appliedSSAFBefore.Equals("1") ? true : false));
                else if (toSSAFApplicationDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate <= toSSAFApplicationDate) == (appliedSSAFBefore.Equals("1") ? true : false));
            }

            if (!String.IsNullOrEmpty(ssafIssuedBefore))
            {
                if (fromSSAFPermitIssueDate.HasValue && toSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromSSAFPermitIssueDate && z.PermitIssueDate.Value <= toSSAFPermitIssueDate)) == (ssafIssuedBefore.Equals("1") ? true : false));
                else if (fromSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromSSAFPermitIssueDate)) == (ssafIssuedBefore.Equals("1") ? true : false));
                else if (toSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value <= toSSAFPermitIssueDate)) == (ssafIssuedBefore.Equals("1") ? true : false));
            }

            if (!String.IsNullOrEmpty(referenceGuideActivityConcern))
            {
                query = query.Where(x => x.ActivityConcern.Equals(referenceGuideActivityConcern));
            }
            //if (fdIssuedBeforeId == "1")
            //{
            //    query = query.Where(z => z.FdMasters.Any(fm => fdIssuedBeforeFdYears.Contains(fm.FdYear) && fm.IsDeleted == false &&
            //                                             fm.FdEvent.Any(fe =>
            //                                                            (fe.PermitNum != null && fe.PermitNum != "") &&
            //                                                            fe.PermitRevokeIndicator == false &&
            //                                                            fe.IsDeleted == false)
            //                                              )
            //                       );
            //}
            //else if (fdIssuedBeforeId == "2")
            //{
            //    query = query.Where(z => z.FdMasters.Any(fm => (fdIssuedBeforeFdYears.Contains(fm.FdYear) && fm.IsDeleted == false &&
            //                                                     fm.FdEvent.Any(fe =>
            //                                                                    (fe.PermitNum == null || fe.PermitNum == "" ||
            //                                                                     fe.PermitRevokeIndicator == true) &&
            //                                                                    fe.IsDeleted == false)
            //                                                     ) ||
            //                                                     (!fdIssuedBeforeFdYears.Contains(fm.FdYear) && fm.IsDeleted == false)
            //                                              ));
            //}

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ReferenceGuideSearchView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ReferenceGuideSearchView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
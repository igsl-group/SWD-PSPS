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
using Psps.Models.Dto.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Data.Repositories
{
    public interface IOrgMasterSearchViewRepository : IRepository<OrgMasterSearchView, int>
    {
        IPagedList<OrgMasterSearchView> GetPageByOrgMasterSearchView(GridSettings grid, String withHoldInd, String receivedComplaintBefore, String receivedEnquiryBefore,
                                                                     String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                     String appliedFdBefore, IList<string> appliedFDBeforeFdYears,
                                                                     String appliedSSAFBefore, DateTime? fromSSAFAppRecDate, DateTime? toSSAFAppRecDate,
                                                                     String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate, 
                                                                     String fdIssuedBefore, IList<string> fdIssuedBeforeFdYears,
                                                                     String ssafIssuedBfore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate);
    }

    public class OrgMasterSearchViewRepository : BaseRepository<OrgMasterSearchView, int>, IOrgMasterSearchViewRepository
    {
        private readonly IWithholdingHistoryRepository _withholdingHistoryRepository;

        public OrgMasterSearchViewRepository(ISession session, IWithholdingHistoryRepository withholdingHistoryRepository)
            : base(session)
        {
            this._withholdingHistoryRepository = withholdingHistoryRepository;
        }

        public IPagedList<OrgMasterSearchView> GetPageByOrgMasterSearchView(GridSettings grid, String withHoldInd, String receivedComplaintBefore, String receivedEnquiryBefore,
                                                                            String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                            String appliedFdBefore, IList<string> appliedFDBeforeFdYears,
                                                                            String appliedSSAFBefore, DateTime? fromSSAFAppRecDate, DateTime? toSSAFAppRecDate,
                                                                            String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate, 
                                                                            String fdIssuedBefore, IList<string> fdIssuedBeforeFdYears,
                                                                            String ssafIssuedBfore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate)
        {
            var query = this.Table;

            //filtring
            if (!String.IsNullOrEmpty(receivedComplaintBefore))
            {
                if (receivedComplaintBefore.Equals("1"))
                {
                    query = query.Where(x => x.ComplaintMaster.Any(y => y.ComplaintRecordType == "02"));
                }
            }
            if (!String.IsNullOrEmpty(receivedEnquiryBefore))
            {
                if (receivedEnquiryBefore.Equals("1"))
                {
                    query = query.Where(x => x.ComplaintMaster.Any(y => y.ComplaintRecordType == "01"));
                }
            }

            //if (!String.IsNullOrEmpty(withHoldInd))
            //{
            //    if (withHoldInd.Equals("1"))
            //    {
            //        var queryW = (from u in _withholdingHistoryRepository.Table
            //                      where u.OrgId != null
            //                      select u.OrgId
            //                      ).ToList();

            //        query = query.Where(x => queryW.Contains(x.OrgId));
            //    }
            //    if (withHoldInd.Equals("2"))
            //    {
            //        var queryW = (from u in _withholdingHistoryRepository.Table
            //                      where u.OrgId != null
            //                      select u.OrgId).ToList();

            //        query = query.Where(x => !queryW.Contains(x.OrgId));
            //    }
            //}

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

            if (!String.IsNullOrEmpty(appliedFdBefore) && appliedFDBeforeFdYears != null)
            {
                query = query.Where(x => x.FdMaster.Any(y => appliedFDBeforeFdYears.Contains(y.FdYear)) == (appliedFdBefore.Equals("1") ? true : false));                
            }

            if (!String.IsNullOrEmpty(fdIssuedBefore) && fdIssuedBeforeFdYears != null)
            {
                query = query.Where(x => x.FdMaster.Any(y => fdIssuedBeforeFdYears.Contains(y.FdYear) &&
                                                             y.FdEvent.Any(z => (z.PermitNum != null && !z.PermitNum.Equals(string.Empty)) && z.PermitRevokeIndicator == false)) == (fdIssuedBefore.Equals("1") ? true : false));
                
            }

            if (!String.IsNullOrEmpty(appliedSSAFBefore))
            {
                if (fromSSAFAppRecDate.HasValue && toSSAFAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate >= fromSSAFAppRecDate && y.ApplicationReceiveDate <= toSSAFAppRecDate) == (appliedSSAFBefore.Equals("1") ? true : false));
                else if (fromSSAFAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate >= fromSSAFAppRecDate) == (appliedSSAFBefore.Equals("1") ? true : false));
                else if (toSSAFAppRecDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.ApplicationReceiveDate <= toSSAFAppRecDate) == (appliedSSAFBefore.Equals("1") ? true : false));
            }

            if (!String.IsNullOrEmpty(ssafIssuedBfore))
            {
                if (fromSSAFPermitIssueDate.HasValue && toSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromSSAFPermitIssueDate && z.PermitIssueDate.Value <= toSSAFPermitIssueDate)) == (ssafIssuedBfore.Equals("1") ? true : false));
                else if (fromSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value >= fromSSAFPermitIssueDate)) == (ssafIssuedBfore.Equals("1") ? true : false));
                else if (toSSAFPermitIssueDate.HasValue)
                    query = query.Where(x => x.PspMasters.Any(y => y.IsSsaf.Value && y.PspApprovalHistory.Any(z => z.PermitIssueDate != null && z.PermitIssueDate.Value <= toSSAFPermitIssueDate)) == (ssafIssuedBfore.Equals("1") ? true : false));
            }

            //    if (fromFdAppRecDate.HasValue && toFdAppRecDate.HasValue)
            //        query = query.Where(x => x.FdMaster.Any(y => y.ApplicationReceiveDate >= fromFdAppRecDate && y.ApplicationReceiveDate <= toFdAppRecDate && y.IsDeleted == false) == (appliedFdBefore.Equals("1") ? true : false));
            //    else if (fromFdAppRecDate.HasValue)
            //        query = query.Where(x => x.FdMaster.Any(y => y.ApplicationReceiveDate >= fromFdAppRecDate && y.IsDeleted == false) == (appliedFdBefore.Equals("1") ? true : false));
            //    else if (toFdAppRecDate.HasValue)
            //        query = query.Where(x => x.FdMaster.Any(y => y.ApplicationReceiveDate <= toFdAppRecDate && y.IsDeleted == false) == (appliedFdBefore.Equals("1") ? true : false));
            //

            //if (!String.IsNullOrEmpty(flagYear))
            //{
            //    query = query.Where(x => x.FdMaster.Any(y => y.FdYear == flagYear && y.IsDeleted == false));
            //}

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<OrgMasterSearchView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<OrgMasterSearchView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
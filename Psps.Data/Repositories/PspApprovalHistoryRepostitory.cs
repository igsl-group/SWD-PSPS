using NHibernate;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IPspApprovalHistoryRepository : IRepository<PspApprovalHistory, int>
    {
        PspApprovalHistory GetPspApprovalHistByPermitNo(string permitNo);

        string GetNewCaseMaxSeq(string pspYear);

        string GetOldCaseMaxSeq(int prevPspMasterId, string pspYear);

        IPagedList<PspApprovalHistorySummaryDto> GetPspApprovHistSummary(GridSettings grid, int pspMasterId);
    }

    public class PspApprovalHistoryRepository : BaseRepository<PspApprovalHistory, int>, IPspApprovalHistoryRepository
    {
        public PspApprovalHistoryRepository(ISession session)
            : base(session)
        {
        }

        public PspApprovalHistory GetPspApprovalHistByPermitNo(string permitNo)
        {
            return this.Table.Where(x => x.PermitNum == permitNo).FirstOrDefault();
        }

        public string GetNewCaseMaxSeq(string pspYear)
        {
            //var table = this.Table.Where(x => x.PspYear == pspYear);
            var query = from u in this.Table.Where(x => x.PermitNum.Substring(0, 4) == pspYear)
                        select new
                        {
                            permitNum = u.PermitNum.Substring(5, 3)
                            //z.PermitNum.Substring(1, 3)
                        };
            
            var midNum = Convert.ToInt32(query.Max(x => x.permitNum)) + 1;

            if (!string.IsNullOrEmpty(Convert.ToString(midNum)))
            {
                string midLeadZero = Convert.ToString(midNum);

                while (midLeadZero.Length < 3)
                {
                    midLeadZero = "0" + midLeadZero;
                }

                var rtnReuslt = "";

                rtnReuslt = pspYear + "/" + midLeadZero + "/" + "1";

                return rtnReuslt;
            }
            else
                return pspYear + "/" + "001" + "/1";
        }

        public string GetOldCaseMaxSeq(int prevPspMasterId, string pspYear)
        {
            //var table = this.Table.Where(x => x.PspYear == pspYear);
            var query = from u in this.Table.Where(x => x.PspMaster.PspMasterId == prevPspMasterId && x.PermitNum != null)
                        select new
                        {
                            Val = u.PermitNum.Substring(0, 8) // get first 8 digit
                        }
                        ;

            string p = query.Distinct().ToList().FirstOrDefault().Val.ToString();

            var query2 = from u in this.Table.Where(x => x.PermitNum.Contains(p) && x.PermitNum != null)
                         select new
                         {
                             midNum = u.PermitNum.Substring(5, 3),
                             endNum = u.PermitNum.Substring(9, 1),
                             year = u.PermitNum.Substring(0, 4)
                         };

            var endNum = Convert.ToInt32(query2.Max(x => x.endNum)) + 1;
            var midNum = query2.First().midNum;

            pspYear = query2.First().year;

            if (!string.IsNullOrEmpty(Convert.ToString(endNum)))
            {
                var rtnReuslt = "";

                rtnReuslt = pspYear + "/" + Convert.ToString(midNum) + "/" + Convert.ToString(endNum);

                return rtnReuslt;
            }
            else
                return pspYear + "/" + "001" + "/1";
        }

        public IPagedList<PspApprovalHistorySummaryDto> GetPspApprovHistSummary(GridSettings grid, int pspMasterId)
        {
            string sql = "SELECT pa.PspApprovalHistoryId, pa.ApprovalStatus as EventStatus, pa.PermitNum as PspPermitNo, pa.PermitIssueDate, pa.ApprovalType as ApprType, " + Environment.NewLine +
                         "       pa.CancelReason, pa.Remark, pm.RejectionLetterDate as RejectionDate, pe.TotalEventsToBeApprove, pe.EventStartDate, pe.EventEndDate, pa.UpdatedOn " + Environment.NewLine +
                         "FROM PspMaster pm " + Environment.NewLine +
                         "INNER JOIN PspApprovalHistory pa on pm.PspMasterId = pa.PspMasterId and pa.IsDeleted = 0 " + Environment.NewLine +
                         "CROSS APPLY ( " + Environment.NewLine +
                         "   SELECT count(pe.PspEventId) as TotalEventsToBeApprove, min(pe.EventStartDate) as EventStartDate, max(pe.EventEndDate) as EventEndDate " + Environment.NewLine +
                         "   FROM PspEvent pe  " + Environment.NewLine +
                         "   WHERE (pe.PspCancelHistoryId = pa.PspApprovalHistoryId) or (pe.PspApprovalHistoryId = pa.PspApprovalHistoryId) and pe.IsDeleted = 0 " + Environment.NewLine +
                         ") pe " + Environment.NewLine +
                         "WHERE pm.PspMasterId = :pspMasterId and pm.IsDeleted = 0";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("pspMasterId", pspMasterId);

            var result = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(PspApprovalHistorySummaryDto)))
               .List<PspApprovalHistorySummaryDto>().AsQueryable();

            if (grid.IsSearch)
                result = result.Where(grid.Where);

            if (!string.IsNullOrEmpty(grid.SortColumn))
                result = result.OrderBy(grid.SortColumn, grid.SortOrder);

            return new PagedList<PspApprovalHistorySummaryDto>(result, grid.PageIndex, grid.PageSize);
        }
    }
}
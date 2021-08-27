using NHibernate;
using NHibernate.Transform;
using Psps.Core;
using Psps.Core.Common;
using Psps.Data;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Reports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IReportRepository : IRepository<LegalAdviceMaster, int>
    {
        IList<R01SummaryDto> GetR1SummaryViewReport(int? fromYear, int? toYear);

        IList<R01SummaryDto> GetR1SummaryFinViewReport(int? fromYear, int? toYear);

        IList<R2SummaryDisasterDto> GetR1SummaryDisasterViewReport(int? fromYear, int? toYear);

        IList<R01RawDto> GetR1RawViewReport(int? fromYear, int? toYear);

        IList<R01RawDto> GetR1RawSummaryViewReport(int? fromYear, int? toYear);

        IList<R2SummaryComplaintDto> GetR1SummaryComplaintViewReport(int? fromYear, int? toYear);

        IList<R2SummaryDto> GetR2SummaryViewReport(int? fromYear, int? toYear);

        IList<R2SummaryDisasterDto> GetR2SummaryDisasterViewReport(int? fromYear, int? toYear);

        IList<R2SummaryComplaintDto> GetR2SummaryComplaintViewReport(int? fromYear, int? toYear);

        IList<R3BreakdownDto> GetR2BreakdownViewReport(int? fromYear, int? toYear);

        IList<R3BreakdownDto> GetR2BreakdownSummaryViewReport(int? fromYear, int? toYear);

        IList<R3SummaryDto> GetR3SummaryViewReport(int? fromYear, int? toYear);

        //IList<R3BreakdownDto> GetR3BreakdownViewReport(int? fromYear, int? toYear);

        IList<R4MainDto> GetR4MainViewReport(int? fromYear, int? intYear);

        IList<R05_PspSummaryDto> GetR5PspViewReport(int year, bool isSsaf = false);

        IList<R05_FdSummaryDto> GetR5FdViewReport(int year);

        IList<R05_SummaryDto> GetR5SummaryViewReport(int year);

        IList<R05_ComplaintDto> GetR5ComplaintViewReport(int year);

        IList<R05_SentenceDto> GetR5SentenceViewReport(int year);

        IList<R6SummaryDto> GetR6Summary(int disasterMasterId);

        IList<R6MonthlyDto> GetR6Monthly(int disasterMasterId, int yearFrom, int yearTo);

        IList<R7Dto> GetR7Data(int? fromYear, int? toYear);

        //IList<R7FdViewDto> GetR7FdViewReport(int? fromDate, int? toDate);

        //IList<R7PspViewDto> GetR7PspViewReport(int? fromDate, int? toDate);

        //IList<string> GetR7FdViewReport();

        IList<R8Dto> GetR8Report(DateTime? fromDate, DateTime? toDate);

        IList<R9Dto> GetR9Report(DateTime? fromDate, DateTime? toDate);

        IList<R10Dto> GetR10Report(DateTime? fromDate, DateTime? toDate);

        IList<R11Dto> GetR11Report(DateTime? fromDate, DateTime? toDate);

        IList<R12Dto> GetR12Report(DateTime? fromDate, DateTime? toDate);

        IList<R13Dto> GetR13Report(DateTime? fromDate, DateTime? toDate);

        IList<R14Dto> GetR14Report(int? fromYear, int? toYear, string complaintSource);

        IList<R15Dto> GetR15Report();

        IList<R16PspDto> GetR16PspReport(DateTime? criteriaDate);

        IList<R16FdDto> GetR16FdReport(DateTime? criteriaDate);

        IList<R16ComplaintDto> GetR16ComplaintReport(DateTime? criteriaDate);

        IList<R17MainDto> GetR17MainReport(int ComplaintMasterId);

        IList<R17ResultDto> GetR17ResultReport(int ComplaintMasterId);

        IList<R17TelDto> GetR17TelReport(int ComplaintMasterId);

        IList<R17FollowUpDto> GetR17FollowUpReport(int ComplaintMasterId);

        IList<R17FollwUpPoliceDto> GetR17FollowUpPoliceReport(int ComplaintMasterId);

        IList<R17FollowUpOtherDto> GetR17FollowUpOtherReport(int ComplaintMasterId);

        IList<R17FollowUpLetterDto> GetR17FollowUpLetterReport(int ComplaintMasterId);

        IList<R17PoliceCaseDto> GetR17PoliceCaseReport(int ComplaintMasterId);

        IList<R17FromOthersDto> GetR17FromOthersReport(int ComplaintMasterId);

        IList<R18Dto> GetR18Report();

        IList<R19Dto> GetR19Report();

        IList<R21SummaryDto> GetR21SummaryReport(int? year);

        IList<R21ARDto> GetR21ARReport(int? year);

        IList<R22Dto> GetR22Report(DateTime? fromDate, DateTime? toDate);

        IList<R23Dto> GetR23Report(int? from, int? to);

        IList<R24Dto> GetR24Report(string from, int twr);

        IList<R25AllDto> GetR25AllReport(DateTime? sendDate, DateTime? toDate);

        IList<R25SubNGOs> GetR25SubNGOs();

        IList<R25NonSubNGOs> GetR25NonSubNGOs();

        IList<R25SummaryDto> GetR25SummaryReport();

        IList<R26Dto> GetR26Report(DateTime? fromDate, DateTime? toDate);

        IList<R27SummaryDto> GetR27SummaryViewReport(int? fromYear, int? toYear);

        IList<R27SummaryFinDto> GetR27SummaryFinViewReport(int? fromYear, int? toYear);

        IList<R2SummaryComplaintDto> GetR27SummaryComplaintViewReport(int? fromYear, int? toYear);

        IList<R2SummaryDisasterDto> GetR27SummaryDisasterViewReport(int? fromYear, int? toYear);

        IList<R2SummaryDisasterDto> GetR27SummaryFinDisasterViewReport(int? fromYear, int? toYear);

        IList<R01RawDto> GetR27RawViewReport(int? fromYear, int? toYear);

        IList<R01RawDto> GetR27RawSummaryViewReport(int? fromYear, int? toYear);

        IList<object[]> GetObjListFromSql(string strSql, ref List<string> colNames);
    }

    public class ReportRepository : BaseRepository<LegalAdviceMaster, int>, IReportRepository
    {
        public ReportRepository(ISession session)
            : base(session)
        {
        }

        #region R1

        public IList<R01SummaryDto> GetR1SummaryFinViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, PspApply, PspApproval, PspFail, PspEvent, PspGrossProceedM, PspNetProceedM "
                       + "from PspReportR01_SummaryFinView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01SummaryDto)))
               .List<R01SummaryDto>();
            return list;
        }

        public IList<R01SummaryDto> GetR1SummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, PspApply, PspApproval, PspFail, PspEvent, PspGrossProceedM, PspNetProceedM "
                       + "from PspReportR01_SummaryView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01SummaryDto)))
               .List<R01SummaryDto>();
            return list;
        }

        public IList<R2SummaryDisasterDto> GetR1SummaryDisasterViewReport(int? fromYear, int? toYear)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string strCond = string.Empty;

            if (fromYear.HasValue)
            {
                strCond = "PspYear >= :fromYear ";
                paramMap.Add("fromYear", fromYear.Value);
            }

            if (toYear.HasValue)
            {
                strCond += (strCond.IsNotNullOrEmpty() ? " AND " : "") + "PspYear <= :toYear";
                paramMap.Add("toYear", toYear.Value);
            }

            string sql = ("SELECT PspYear, DisasterName, sum(Approval) As Approval, sum(Event) As Event, " + Environment.NewLine +
                          "       round(sum(NetProceed)/ 1000000, 2) As NetProceed " + Environment.NewLine +
                          "FROM PspReportR01_SummaryDisasterView " + Environment.NewLine +
                          "{0} " +
                          "GROUP BY PspYear, DisasterMasterId, DisasterName " + Environment.NewLine +
                          "ORDER BY DisasterMasterId").FormatWith(strCond.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(strCond + Environment.NewLine));

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R2SummaryDisasterDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryDisasterDto)))
               .List<R2SummaryDisasterDto>();
            return list;
        }

        public IList<R2SummaryComplaintDto> GetR1SummaryComplaintViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, Police, Convicted, NFA, VerbalWarning, WrittenWarning, VerbalAdvice, WrittenAdvice, NoResult "
                       + "from PspReportR01_SummaryComplaintView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R2SummaryComplaintDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryComplaintDto)))
               .List<R2SummaryComplaintDto>();
            return list;
        }

        public IList<R01RawDto> GetR1RawViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, Level, PspReceived, PspNotRequired, PspRejected, EventCancelled, AppWithdrawn, CaseClose, "
                       + "       CaseCloseOthers, PspApproved, PspSubvented, NoOfEvent, GrossProceed, NetProceed "
                       + "from PspReportR01_RawView "
                       + "where PspYear between :fromYear and :toYear "
                       //+ "and IsSsaf = 0 " /*To be confirmed: filter out isSsaff case?*/
                       + "order by PspYear, Level";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01RawDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01RawDto)))
               .List<R01RawDto>();
            return list;
        }

        public IList<R01RawDto> GetR1RawSummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "SELECT * " + Environment.NewLine +
                         "FROM ( " + Environment.NewLine +
                         "    SELECT CASE WHEN Level < 3 THEN PspYear - 1 ELSE PspYear END AS PspYear, " + Environment.NewLine +
                         "           sum(PspReceived) As PspReceived, sum(PspNotRequired) As PspNotRequired, sum(PspRejected) As PspRejected, sum(EventCancelled) As EventCancelled, sum(AppWithdrawn) As AppWithdrawn, sum(CaseClose) As CaseClose, sum(CaseCloseOthers) As CaseCloseOthers,  " + Environment.NewLine +
                         "           sum(PspApproved) As PspApproved, sum(PspSubvented) As PspSubvented, sum(NoOfEvent) As NoOfEvent, sum(GrossProceed) As GrossProceed, sum(NetProceed) As NetProceed " + Environment.NewLine +
                         "    FROM PspReportR01_RawView " + Environment.NewLine +
                         //"          WHERE IsSsaf = 0 " + Environment.NewLine + /*To be confirmed: filter out isSsaff case?*/
                         "    GROUP BY CASE WHEN Level < 3 THEN PspYear - 1 ELSE PspYear END " + Environment.NewLine +
                         ") R " + Environment.NewLine +
                         "WHERE PspYear between :fromYear and :toYear " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01RawDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01RawDto)))
               .List<R01RawDto>();
            return list;
        }

        #endregion R1

        #region R27
        public IList<R27SummaryDto> GetR27SummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, PspApplyParent, PspApprovalParent, PspFailParent, PspEventParent, PspGrossProceedMParent, PspNetProceedMParent, "
                       + "PspApplyAmend, PspApprovalAmend, PspFailAmend, PspEventAmend, PspGrossProceedMAmend, PspNetProceedMAmend "
                       + "from PspReportR27_SummaryView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R27SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R27SummaryDto)))
               .List<R27SummaryDto>();
            return list;
        }

        public IList<R27SummaryFinDto> GetR27SummaryFinViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, "
                       + "[SsafApplyNormal],[SsafApplyAmend],[SsafApprovalNormal],[SsafApprovalAmend],[SsafFailNormal],[SsafFailAmend],"
                       + "[SsafEventNormal],[SsafEventAmend],[SsafGrossProceedMNormal],[SsafGrossProceedMAmend],[SsafNetProceedMNormal],[SsafNetProceedMAmend] "
                       + "from PspReportR02_SsafSummaryFinView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R27SummaryFinDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R27SummaryFinDto)))
               .List<R27SummaryFinDto>();
            return list;
        }

        public IList<R2SummaryComplaintDto> GetR27SummaryComplaintViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, Police, Convicted, NFA, VerbalWarning, WrittenWarning, VerbalAdvice, WrittenAdvice, NoResult "
                       + "from PspReportR27_SummaryComplaintView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R2SummaryComplaintDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryComplaintDto)))
               .List<R2SummaryComplaintDto>();
            return list;
        }

        public IList<R2SummaryDisasterDto> GetR27SummaryDisasterViewReport(int? fromYear, int? toYear)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string strCond = string.Empty;

            if (fromYear.HasValue)
            {
                strCond = "PspYear >= :fromYear ";
                paramMap.Add("fromYear", fromYear.Value);
            }

            if (toYear.HasValue)
            {
                strCond += (strCond.IsNotNullOrEmpty() ? " AND " : "") + "PspYear <= :toYear";
                paramMap.Add("toYear", toYear.Value);
            }

            string sql = ("SELECT PspYear, DisasterName, sum(Approval) As Approval, sum(Event) As Event, " + Environment.NewLine +
                          "       round(sum(NetProceed)/ 1000000, 2) As NetProceed " + Environment.NewLine +
                          "FROM PspReportR27_SummaryDisasterView " + Environment.NewLine +
                          "{0} " +
                          "GROUP BY PspYear, DisasterMasterId, DisasterName " + Environment.NewLine +
                          "ORDER BY DisasterMasterId").FormatWith(strCond.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(strCond + Environment.NewLine));

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R2SummaryDisasterDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryDisasterDto)))
               .List<R2SummaryDisasterDto>();
            return list;
        }

        public IList<R2SummaryDisasterDto> GetR27SummaryFinDisasterViewReport(int? fromYear, int? toYear)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string strCond = string.Empty;

            if (fromYear.HasValue)
            {
                strCond = "CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END >= :fromYear ";
                paramMap.Add("fromYear", fromYear.Value);
            }

            if (toYear.HasValue)
            {
                strCond += (strCond.IsNotNullOrEmpty() ? " AND " : "") + "CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END <= :toYear";
                paramMap.Add("toYear", toYear.Value);
            }

            string sql = ("SELECT CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END As PspYear, DisasterName, sum(Approval) As Approval, sum(Event) As Event, " + Environment.NewLine +
                          "       round(sum(NetProceed)/ 1000000, 2) As NetProceed " + Environment.NewLine +
                          "FROM PspReportR27_SummaryDisasterView " + Environment.NewLine +
                          "{0} " +
                          "GROUP BY CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END, DisasterMasterId, DisasterName " + Environment.NewLine +
                          "ORDER BY DisasterMasterId").FormatWith(strCond.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(strCond + Environment.NewLine));

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R2SummaryDisasterDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryDisasterDto)))
               .List<R2SummaryDisasterDto>();
            return list;
        }

        public IList<R01RawDto> GetR27RawViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, Level, PspReceived, PspNotRequired, PspRejected, EventCancelled, AppWithdrawn, CaseClose, "
                       + "       CaseCloseOthers, PspApproved, PspSubvented, NoOfEvent, GrossProceed, NetProceed "
                       + "from PspReportR27_RawView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear, Level";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01RawDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01RawDto)))
               .List<R01RawDto>();
            return list;
        }

        public IList<R01RawDto> GetR27RawSummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "SELECT * " + Environment.NewLine +
                         "FROM ( " + Environment.NewLine +
                         "    SELECT CASE WHEN Level < 3 THEN PspYear - 1 ELSE PspYear END AS PspYear, " + Environment.NewLine +
                         "           sum(PspReceived) As PspReceived, sum(PspNotRequired) As PspNotRequired, sum(PspRejected) As PspRejected, sum(EventCancelled) As EventCancelled, sum(AppWithdrawn) As AppWithdrawn, sum(CaseClose) As CaseClose, sum(CaseCloseOthers) As CaseCloseOthers,  " + Environment.NewLine +
                         "           sum(PspApproved) As PspApproved, sum(PspSubvented) As PspSubvented, sum(NoOfEvent) As NoOfEvent, sum(GrossProceed) As GrossProceed, sum(NetProceed) As NetProceed " + Environment.NewLine +
                         "    FROM PspReportR27_RawView " + Environment.NewLine +                        
                         "    GROUP BY CASE WHEN Level < 3 THEN PspYear - 1 ELSE PspYear END " + Environment.NewLine +
                         ") R " + Environment.NewLine +
                         "WHERE PspYear between :fromYear and :toYear " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R01RawDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R01RawDto)))
               .List<R01RawDto>();
            return list;
        }
        #endregion

        #region R2

        public IList<R2SummaryDto> GetR2SummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, " 
                       + "PspApply, PspApproval, PspFail, PspEvent, PspGrossProceedM, PspNetProceedM, "
                       + "[SsafApplyNormal],[SsafApplyAmend],[SsafApprovalNormal],[SsafApprovalAmend],[SsafFailNormal],[SsafFailAmend],"
                       + "[SsafEventNormal],[SsafEventAmend],[SsafGrossProceedMNormal],[SsafGrossProceedMAmend],[SsafNetProceedMNormal],[SsafNetProceedMAmend],"
                       + "ApplyTWR, ApplyRFD, FlagDayTWR, FlagDayRFD, FailTWR, FailRFD, SumGrossTWRM, SumGrossRFDM, SumNetTWRM, SumNetRFDM "
                       + "from PspReportR02_SummaryView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R2SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryDto)))
               .List<R2SummaryDto>();
            return list;
        }

        public IList<R2SummaryDisasterDto> GetR2SummaryDisasterViewReport(int? fromYear, int? toYear)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string strCond = string.Empty;

            if (fromYear.HasValue)
            {
                strCond = "CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END >= :fromYear ";
                paramMap.Add("fromYear", fromYear.Value);
            }

            if (toYear.HasValue)
            {
                strCond += (strCond.IsNotNullOrEmpty() ? " AND " : "") + "CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END <= :toYear";
                paramMap.Add("toYear", toYear.Value);
            }

            string sql = ("SELECT CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END As PspYear, DisasterName, sum(Approval) As Approval, sum(Event) As Event, " + Environment.NewLine +
                          "       round(sum(NetProceed)/ 1000000, 2) As NetProceed " + Environment.NewLine +
                          "FROM PspReportR01_SummaryDisasterView " + Environment.NewLine +
                          "{0} " +
                          "GROUP BY CASE WHEN PspMonth < 4 THEN PspYear - 1 ELSE PspYear END, DisasterMasterId, DisasterName " + Environment.NewLine +
                          "ORDER BY DisasterMasterId").FormatWith(strCond.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(strCond + Environment.NewLine));

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R2SummaryDisasterDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryDisasterDto)))
               .List<R2SummaryDisasterDto>();
            return list;
        }

        public IList<R2SummaryComplaintDto> GetR2SummaryComplaintViewReport(int? fromYear, int? toYear)
        {
            string sql = "select PspYear, Police, Convicted, NFA, VerbalWarning, WrittenWarning, VerbalAdvice, WrittenAdvice, NoResult "
                       + "from PspReportR02_SummaryComplaintView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R2SummaryComplaintDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R2SummaryComplaintDto)))
               .List<R2SummaryComplaintDto>();
            return list;
        }

        public IList<R3BreakdownDto> GetR2BreakdownViewReport(int? fromYear, int? toYear)
        {
            string sql = "select Yr, Mon, Level, Telephone, Written, From1823, Mass, DC, LegC, Other, FromPolice, UnClass, Police "
                       + "from PspReportR02_BreakdownView "
                       + "where Yr between :fromYear and :toYear "
                       + "order by Yr, Level";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R3BreakdownDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R3BreakdownDto)))
               .List<R3BreakdownDto>();
            return list;
        }

        public IList<R3BreakdownDto> GetR2BreakdownSummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "SELECT * " + Environment.NewLine +
                         "FROM ( " + Environment.NewLine +
                         "    SELECT CASE WHEN Level < 3 THEN Yr - 1 ELSE Yr END As Yr, " + Environment.NewLine +
                         "           sum(Telephone) As Telephone, sum(Written) As Written, sum(From1823) As From1823, sum(Mass) As Mass, sum(DC) As DC, " + Environment.NewLine +
                         "           sum(LegC) As LegC, sum(FromPolice) As FromPolice, sum(Other) As Other, sum(UnClass) As UnClass, sum(Police) As Police " + Environment.NewLine +
                         "    FROM PspReportR02_BreakdownView " + Environment.NewLine +
                         "    GROUP BY CASE WHEN Level < 3 THEN Yr - 1 ELSE Yr END " + Environment.NewLine +
                         ") R " + Environment.NewLine +
                         "WHERE Yr between :fromYear and :toYear " + Environment.NewLine +
                         "ORDER BY Yr";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R3BreakdownDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R3BreakdownDto)))
               .List<R3BreakdownDto>();
            return list;
        }

        #endregion R2

        #region R3

        public IList<R3SummaryDto> GetR3SummaryViewReport(int? fromYear, int? toYear)
        {
            string sql = "select FdYear, FlagDayTWR, TWRSat, TWRWeekday, TWRWeekdayPledging, ApplyTWR, OrgTWR, SumTWR, FlagDayRFD, RFDSat, RFDWeekday, RFDWeekdayPledging, ApplyRFD, OrgRFD, SumRFD "
                       + "from PspReportR03_SummaryView "
                       + "where Yr between :fromYear and :toYear "
                       + "order by Yr";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", toYear.Value);

            IList<R3SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R3SummaryDto)))
               .List<R3SummaryDto>();
            return list;
        }

        //public IList<R3BreakdownDto> GetR3BreakdownViewReport(int? fromYear, int? toYear)
        //{
        //    string sql = "select Yr, Mon, Level, Telephone, Written, From1823, Mass, DC, LegC, Other, Police "
        //               + "from PspReportR03_BreakdownView "
        //               + "where Yr between :fromYear and :toYear "
        //               + "order by Yr, Level";

        //    var query = this.Session.CreateSQLQuery(sql);
        //    query.SetInt32("fromYear", fromYear.Value);
        //    query.SetInt32("toYear", toYear.Value);

        //    IList<R3BreakdownDto> list = query
        //       .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R3BreakdownDto)))
        //       .List<R3BreakdownDto>();
        //    return list;
        //}

        #endregion R3

        #region R4

        public IList<R4MainDto> GetR4MainViewReport(int? fromYear, int? intYear)
        {
            string sql = "select PspYear, PspApply, PspApproval, PspEvent, FdYear, FdApply, FdEvent, PspWritten, PspTelephone, Psp1823, PspMass, PspDC, PspLegC, PspOther, PspFromPolice, PspUnClass, PspPolice, PspNFA, PspConvicted, PspVerbalWarning, PspWrittenWarning, PspVerbalAdvice, PspWrittenAdvice, PspNoResult, "
                       + "SsafApply, SsafApproval, SsafEvent, SsafWritten, SsafTelephone, Ssaf1823, SsafMass, SsafDC, SsafLegC, SsafOther, SsafFromPolice, SsafUnClass, SsafPolice, SsafNFA, SsafConvicted, SsafVerbalWarning, SsafWrittenWarning, SsafVerbalAdvice, SsafWrittenAdvice, SsafNoResult, "
                       + "FdWritten, FdTelephone, Fd1823, FdMass, FdDC, FdLegC, FdOther, FdFromPolice, FdUnClass, FdPolice, FdNFA, FdConvicted, FdVerbalWarning, FdWrittenWarning, FdVerbalAdvice, FdWrittenAdvice, FdNoResult, "
                       + "OtherWritten, OtherTelephone, Other1823, OtherMass, OtherDC, OtherLegC, OtherOther, OtherFromPolice, OtherUnClass, OtherPolice, OtherNFA, OtherConvicted, OtherVerbalWarning, OtherWrittenWarning, OtherVerbalAdvice, OtherWrittenAdvice, OtherNoResult "
                       + "from PspReportR04_MainView "
                       + "where PspYear between :fromYear and :toYear "
                       + "order by PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("fromYear", fromYear.Value);
            query.SetInt32("toYear", intYear.Value);

            IList<R4MainDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R4MainDto)))
               .List<R4MainDto>();
            return list;
        }

        #endregion R4

        #region R5

        public IList<R05_PspSummaryDto> GetR5PspViewReport(int year, bool isSsaf = false)
        {
            string sql = "SELECT PspYear, PspApproved, PspCalApproved, PspOrgCnt, NetProceed " + Environment.NewLine +
                         "FROM PspReportR05_PspSummaryView " + Environment.NewLine +
                         "WHERE PspYear BETWEEN :from and :to AND IsSsaf = :IsSsaf " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("from", DateTime.Now.AddYears(-1 * year).Year);
            query.SetInt32("to", DateTime.Now.AddYears(-1).Year);
            query.SetBoolean("IsSsaf", isSsaf);

            IList<R05_PspSummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R05_PspSummaryDto)))
               .List<R05_PspSummaryDto>();
            return list;
        }

        public IList<R05_FdSummaryDto> GetR5FdViewReport(int year)
        {
            string sql = "SELECT FdYear, FlagDayTWR, FlagDayRFD, NetProceed " + Environment.NewLine +
                         "FROM PspReportR05_FdSummaryView " + Environment.NewLine +
                         "WHERE FdYear BETWEEN :from and :to " + Environment.NewLine +
                         "ORDER BY FdYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("from", DateTime.Now.AddYears(-1 * year).Year);
            query.SetInt32("to", DateTime.Now.AddYears(-1).Year);

            IList<R05_FdSummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R05_FdSummaryDto)))
               .List<R05_FdSummaryDto>();
            return list;
        }

        public IList<R05_SummaryDto> GetR5SummaryViewReport(int year)
        {
            string sql = "SELECT PspYear, Permit, OrgCnt, NetProceed " + Environment.NewLine +
                         "FROM PspReportR05_SummaryView " + Environment.NewLine +
                         "WHERE PspYear BETWEEN :from and :to " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("from", DateTime.Now.AddYears(-1 * year).Year);
            query.SetInt32("to", DateTime.Now.AddYears(-1).Year);

            IList<R05_SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R05_SummaryDto)))
               .List<R05_SummaryDto>();
            return list;
        }

        public IList<R05_ComplaintDto> GetR5ComplaintViewReport(int year)
        {
            string sql = "SELECT PspYear, Permit, OrgCnt, NetProceed, ComplaintCnt, ChargeCnt " + Environment.NewLine +
                         "FROM PspReportR05_ComplaintView " + Environment.NewLine +
                         "WHERE PspYear BETWEEN :from and :to " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("from", DateTime.Now.AddYears(-1 * year).Year);
            query.SetInt32("to", DateTime.Now.AddYears(-1).Year);

            IList<R05_ComplaintDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R05_ComplaintDto)))
               .List<R05_ComplaintDto>();
            return list;
        }

        public IList<R05_SentenceDto> GetR5SentenceViewReport(int year)
        {
            string sql = "SELECT PspYear, SentenceDetail, OrgName " + Environment.NewLine +
                         "FROM PspReportR05_SentenceView " + Environment.NewLine +
                         "WHERE PspYear BETWEEN :from and :to " + Environment.NewLine +
                         "ORDER BY PspYear";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("from", DateTime.Now.AddYears(-1 * year).Year);
            query.SetInt32("to", DateTime.Now.AddYears(-1).Year);

            IList<R05_SentenceDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R05_SentenceDto)))
               .List<R05_SentenceDto>();
            return list;
        }

        #endregion R5

        #region R6

        public IList<R6SummaryDto> GetR6Summary(int disasterMasterId)
        {
            string sql = "SELECT ApplicationReceiveDate, EngOrgName, ChiOrgName, " + Environment.NewLine +
                         "       PspNonSect88Ind, PspSect88Ind, OrgNonSect88Ind, OrgSect88Ind, " + Environment.NewLine +
                         "       BeneficiaryOrg, ProcessingOfficerPost, AppNum, PermitIssued, " + Environment.NewLine +
                         "       ApplicationDisposalDate, PermitNum, PspRef, EventPeriodFrom, EventPeriodTo, SpecialRemark, " + Environment.NewLine +
                         "       PspNotReqInd, AppWithdrawnInd, AppRejectInd, CloseMergeInd, CloseOtherInd, AllCheckedInd, " + Environment.NewLine +
                         "       WithholdingListIndicator, NetProceed" + Environment.NewLine +
                         "FROM PspReportR06_OrgSummary " + Environment.NewLine +
                         "WHERE DisasterMasterId = :disasterId " + Environment.NewLine +
                         "ORDER BY [DisasterName] ASC";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("disasterId", disasterMasterId);

            IList<R6SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R6SummaryDto)))
               .List<R6SummaryDto>();
            return list;
        }

        public IList<R6MonthlyDto> GetR6Monthly(int disasterMasterId, int yearFrom, int yearTo)
        {
            string sql = "SELECT PspYear, Level, PspReceived, PspNotRequired, PspRejected, PspRevoked, EventCancelled, " + Environment.NewLine +
                         "       AppWithdrawn, CaseClose, CaseCloseOthers, PspApproved, PspSubvented, NoOfEvent, " + Environment.NewLine +
                         "       UnderProcess, GrossProceed, NetProceed, " + Environment.NewLine +
                         "       PoliceCase, NFA, Verbal, Convicted " + Environment.NewLine +
                         "FROM PspReportR06_MonthSummary " + Environment.NewLine +
                         "WHERE DisasterMasterId = :disasterId and PspYear between :fromYear and :toYear " + Environment.NewLine +
                         "ORDER BY [DisasterName] ASC";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("disasterId", disasterMasterId);
            query.SetInt32("fromYear", yearFrom);
            query.SetInt32("toYear", yearTo);

            IList<R6MonthlyDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R6MonthlyDto)))
               .List<R6MonthlyDto>();
            return list;
        }

        #endregion R6

        #region R7

        public IList<R7Dto> GetR7Data(int? fromYear, int? toYear)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();

            string sql = "SELECT DisasterMasterId, DisasterName, ChiDisasterName, DisasterDate, ProcessPeriod, OrgSect88Ind, OrgNonSect88Ind, PspSect88Ind, PspNonSect88Ind, PermitSect88Issued, " + Environment.NewLine +
                         "       PermitNonSect88Issued, EventCount, NetProceed, AccRequiredCount, AllCheckedInd, WithholdingListIndicator, ComplaintSect88Count, ComplaintNonSect88Count, " + Environment.NewLine +
                         "       Cond8Sect88Count, Cond8NonSect88Count, AllDocOsSect88Count, AllDocOsNonSect88Count, AcProblemSect88Count, AcProblemNonSect88Count " + Environment.NewLine +
                         "FROM PspReportR07View " + Environment.NewLine +
                         "WHERE 1=1 {0} " + Environment.NewLine +
                         "ORDER BY DisasterDate, DisasterMasterId";

            string condition = string.Empty;

            if (fromYear.HasValue || toYear.HasValue)
            {

                if (fromYear.HasValue)
                {
                    condition += "AND DATEPART(YEAR, DisasterDate) >= :fromYear ";
                    paramMap.Add("fromYear", fromYear.Value);
                }

                if (toYear.HasValue)
                {
                    condition += "AND DATEPART(YEAR, DisasterDate) <= :toYear ";
                    paramMap.Add("toYear", toYear.Value);
                }

            }

            sql = sql.FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R7Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R7Dto)))
               .List<R7Dto>();
            return list;
        }

        //public IList<R7FdViewDto> GetR7FdViewReport(int? fromYear, int? toYear)
        //{
        //    Dictionary<string, object> paramMap = new Dictionary<string, object>();
        //    string condition = string.Empty;

        //    if (fromYear.HasValue)
        //    {
        //        condition = "left(FdYear, 2) >= :fromYear ";
        //        paramMap.Add("fromYear", fromYear.Value % 100);
        //    }

        //    if (toYear.HasValue)
        //    {
        //        condition += (condition.IsNotNullOrEmpty() ? " AND " : "") + "right(FdYear, 2) <= :toYear";
        //        paramMap.Add("toYear", toYear.Value % 100);
        //    }

        //    string sql = ("SELECT [FdYear], [TWR], [ApplicationCount], [PermitNumCount], [NetAmountRaised] " + Environment.NewLine +
        //                  "FROM PspReportR07_FDView " + Environment.NewLine +
        //                  "{0} " +
        //                  "ORDER BY [FdYear] ").FormatWith(condition.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(condition + Environment.NewLine));

        //    var query = this.Session.CreateSQLQuery(sql);
        //    foreach (KeyValuePair<string, object> kvp in paramMap)
        //        query.SetParameter(kvp.Key, kvp.Value);

        //    IList<object[]> list = query.List<object[]>();

        //    var resultList = new Dictionary<string, R7FdViewDto>();

        //    for (var i = 0; i < list.Count; i++)
        //    {
        //        object[] obj = list[i];
        //        string fdYear = Convert.ToString(obj[0]);

        //        R7FdViewDto dto = null;
        //        if (!resultList.TryGetValue(fdYear, out dto))
        //        {
        //            dto = new R7FdViewDto();
        //            resultList.Add(fdYear, dto);
        //        }

        //        dto.FdYear = fdYear;
        //        if (obj[1].ToString() == "TWFD")
        //        {
        //            dto.TWFD_TWRCount = Convert.ToInt32(obj[2]);
        //            dto.TWFD_PermitNumCount = Convert.ToInt32(obj[3]);
        //            dto.TWFD_NetAmountRaised = Convert.ToInt32(obj[4]);
        //        }
        //        else if (obj[1].ToString() == "FRD")
        //        {
        //            dto.FRD_TWRCount = Convert.ToInt32(obj[2]);
        //            dto.FRD_PermitNumCount = Convert.ToInt32(obj[3]);
        //            dto.FRD_NetAmountRaised = Convert.ToInt32(obj[4]);
        //        }
        //    }

        //    return resultList.Values.ToList();
        //}

        //public IList<R7PspViewDto> GetR7PspViewReport(int? fromYear, int? toYear)
        //{
        //    Dictionary<string, object> paramMap = new Dictionary<string, object>();
        //    string condition = string.Empty;

        //    if (fromYear.HasValue)
        //    {
        //        condition = "PspYear >= :fromYear ";
        //        paramMap.Add("fromYear", fromYear.Value);
        //    }

        //    if (toYear.HasValue)
        //    {
        //        condition += (condition.IsNotNullOrEmpty() ? " AND " : "") + "PspYear <= :toYear";
        //        paramMap.Add("toYear", toYear.Value);
        //    }

        //    string sql = ("SELECT [PspYear], [ApplicationCount], [PermitNumCount], [EventInvolveCount], [NetAmountRaised] " + Environment.NewLine +
        //                  "FROM PspReportR07_PsPView " + Environment.NewLine +
        //                  "{0} " +
        //                  "ORDER BY [PspYear] ASC").FormatWith(condition.IsNullOrEmpty() ? "" : "WHERE {0} ".FormatWith(condition + Environment.NewLine));

        //    var query = this.Session.CreateSQLQuery(sql);
        //    foreach (KeyValuePair<string, object> kvp in paramMap)
        //        query.SetParameter(kvp.Key, kvp.Value);

        //    IList<R7PspViewDto> list = query
        //       .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R7PspViewDto)))
        //       .List<R7PspViewDto>();
        //    return list;
        //}

        //public IList<string> GetR7FdViewReport()
        //{
        //    string sql = "Select ('Including ' + LTRIM(RTRIM(CAST(TabB.PermitCount as VARCHAR(10)))) + ' permits and $' + "
        //           + " LTRIM(RTRIM(CAST(TabB.NetAmountRaised as VARCHAR(15)))) + ' raised which are related to the relief work of ' + "
        //           + "TabB.DisasterName) as remark "
        //        + " from "
        //           + " (select TabA.DisasterName, sum(TabA.PermitNum_Count) as PermitCount, sum(TabA.NetAmountRaised) as NetAmountRaised "
        //           + " from PspReportR07_RemarkView TabA "
        //       + " group by TabA.DisasterName) TabB";

        //    string condition = " where 1=1 ";
        //    string orderBy = " ORDER BY [remark] ASC";
        //    sql = sql + condition + orderBy;
        //    IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
        //    var resultList = new List<string>();
        //    for (var i = 0; i < list.Count; i++)
        //    {
        //        resultList.Add(list[i].ToString());
        //    }
        //    return resultList;
        //}

        #endregion R7

        #region R8

        public IList<R8Dto> GetR8Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT FLT.ComplaintDate, ISNULL(CM.ComplaintCnt,0) ComplaintCnt, ISNULL(CM.OrgCnt, 0) OrgCnt, FLT.FollowUpLetterType, CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY FLT.ComplaintDate ORDER BY FLT.DisplayOrder)) As DisplayOrder, ISNULL(FA.Cnt, 0) Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT DISTINCT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As FollowUpLetterType, LK.DisplayOrder " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.FollowUpLetterType.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +                          
                          "   WHERE {0} " + Environment.NewLine +
                          ") FLT " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT DATEPART(YEAR, ComplaintDate) As YYYY, DATEPART(MONTH, ComplaintDate) As MM, RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, COUNT(DISTINCT CM.ComplaintMasterId) AS ComplaintCnt, COUNT(DISTINCT CASE WHEN CM.OrgId IS NOT NULL THEN CONVERT(VARCHAR, CM.OrgId) ELSE ISNULL(CM.ConcernedOrgName, '') END) As OrgCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) " + Environment.NewLine +
                          "   GROUP BY DATEPART(YEAR, ComplaintDate), DATEPART(MONTH, ComplaintDate), RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) " + Environment.NewLine +
                          ") CM on CM.ComplaintDate = FLT.ComplaintDate " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As FollowUpLetterType,  " + Environment.NewLine +
                          "          COUNT(FA.ComplaintFollowUpActionId) Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.FollowUpLetterType.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintFollowUpAction FA ON FA.ComplaintMasterId = CM.ComplaintMasterId AND FA.FollowUpLetterType = LK.Code AND FA.FollowUpIndicator = 1 AND FA.FollowUpLetterType IS NOT NULL AND FA.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) " + Environment.NewLine +
                          "   GROUP BY RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7), LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") FA ON FLT.ComplaintDate = FA.ComplaintDate AND FLT.FollowUpLetterType = FA.FollowUpLetterType " + Environment.NewLine +
                          "ORDER BY RIGHT(FLT.ComplaintDate, 4), LEFT(FLT.ComplaintDate, 2), FLT.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R8Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R8Dto)))
               .List<R8Dto>();
            return list;
        }

        #endregion R8

        #region R9

        public IList<R9Dto> GetR9Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT CM.OrgRef, CM.EngOrgName, CM.ChiOrgName, CM.ComplaintCnt, FA.FollowUpLetterType, FA.DisplayOrder, FA.Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END As EngOrgName, OM.ChiOrgName, COUNT(CM.ComplaintMasterId) As ComplaintCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   LEFT JOIN OrgMaster OM ON CM.OrgId = OM.OrgId AND OM.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END, OM.ChiOrgName " + Environment.NewLine +
                          ") CM " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END AS ConcernedOrgName, LK.Code As FollowUpLetterType, " + Environment.NewLine +
                          "          CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END ORDER BY LK.DisplayOrder)) As DisplayOrder, COUNT(FA.ComplaintFollowUpActionId) As Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.FollowUpLetterType.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintFollowUpAction FA ON FA.ComplaintMasterId = CM.ComplaintMasterId AND FA.FollowUpLetterType = LK.Code AND FA.FollowUpIndicator = 1 AND FA.FollowUpLetterType IS NOT NULL AND FA.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END, LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") FA ON (CM.OrgId IS NOT NULL AND CM.OrgId = FA.OrgId) OR (CM.OrgId IS NULL AND FA.OrgId IS NULL AND CM.EngOrgName = FA.ConcernedOrgName) " + Environment.NewLine +
                          "ORDER BY CM.OrgRef, EngOrgName, FA.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R9Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R9Dto)))
               .List<R9Dto>();
            return list;
        }

        #endregion R9

        #region R10

        public IList<R10Dto> GetR10Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT CR2.ComplaintDate, ISNULL(CM.ComplaintCnt, 0) ComplaintCnt, ISNULL(CM.OrgCnt, 0) OrgCnt, CR2.NonComplianceNature, CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY CR2.ComplaintDate ORDER BY CR2.DisplayOrder)) As DisplayOrder, ISNULL(CR.Cnt, 0) Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT DISTINCT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As NonComplianceNature, LK.DisplayOrder " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = 'ComplaintNonComplianceNature' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   --INNER JOIN ComplaintResult CR ON CR.ComplaintMasterId = CM.ComplaintMasterId AND CR.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          ") CR2 " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT DATEPART(YEAR, ComplaintDate) As YYYY, DATEPART(MONTH, ComplaintDate) As MM, RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, COUNT(DISTINCT CM.ComplaintMasterId) AS ComplaintCnt, COUNT(DISTINCT CASE WHEN CM.OrgId IS NOT NULL THEN CONVERT(VARCHAR, CM.OrgId) ELSE ISNULL(CM.ConcernedOrgName, '') END) As OrgCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL)" + Environment.NewLine +
                          "   GROUP BY DATEPART(YEAR, ComplaintDate), DATEPART(MONTH, ComplaintDate), RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) " + Environment.NewLine +
                          ") CM ON CR2.ComplaintDate = CM.ComplaintDate " + Environment.NewLine + 
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As NonComplianceNature,  " + Environment.NewLine +
                          "          --CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) ORDER BY LK.DisplayOrder)) As DisplayOrder, " + Environment.NewLine +
                          "          COUNT(CR.ComplaintResultId) As Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.ComplaintNonComplianceNature.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintResult CR ON CR.ComplaintMasterId = CM.ComplaintMasterId AND CHARINDEX(CONCAT(',', LK.Code, ','), CONCAT(',', CR.NonComplianceNature, ',')) > 0 AND CR.IsDeleted = 0 -- AND ISNULL(CR.Result, '01') != '01'" + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) " + Environment.NewLine +
                          "   GROUP BY RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7), LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") CR ON CR2.ComplaintDate = CR.ComplaintDate AND CR.NonComplianceNature = CR2.NonComplianceNature " + Environment.NewLine +
                          "ORDER BY RIGHT(CR2.ComplaintDate, 4), LEFT(CR2.ComplaintDate, 2), CR2.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R10Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R10Dto)))
               .List<R10Dto>();
            return list;
        }

        #endregion R10

        #region R11

        public IList<R11Dto> GetR11Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT CM.OrgRef, CM.EngOrgName, CM.ChiOrgName, CM.ComplaintCnt, CR.NonComplianceNature, CR.DisplayOrder, CR.Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END As EngOrgName, OM.ChiOrgName, COUNT(CM.ComplaintMasterId) As ComplaintCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   LEFT JOIN OrgMaster OM ON CM.OrgId = OM.OrgId AND OM.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END, OM.ChiOrgName " + Environment.NewLine +
                          ") CM " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END AS ConcernedOrgName, LK.Code As NonComplianceNature,  " + Environment.NewLine +
                          "          CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END ORDER BY LK.DisplayOrder)) As DisplayOrder, COUNT(CR.ComplaintResultId) As Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.ComplaintNonComplianceNature.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintResult CR ON CR.ComplaintMasterId = CM.ComplaintMasterId AND CHARINDEX(CONCAT(',', LK.Code, ','), CONCAT(',', CR.NonComplianceNature, ',')) > 0 AND CR.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END, LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") CR ON (CM.OrgId IS NOT NULL AND CM.OrgId = CR.OrgId) OR (CM.OrgId IS NULL AND CR.OrgId IS NULL AND CM.EngOrgName = CR.ConcernedOrgName) " + Environment.NewLine +
                          "ORDER BY CM.OrgRef, EngOrgName,  CR.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R11Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R11Dto)))
               .List<R11Dto>();
            return list;
        }

        #endregion R11

        #region R12

        public IList<R12Dto> GetR12Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT CR2.ComplaintDate, ISNULL(CM.ComplaintCnt, 0) ComplaintCnt, ISNULL(CM.OrgCnt, 0) OrgCnt, CR2.ComplaintResult, CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY CR2.ComplaintDate ORDER BY CR2.DisplayOrder)) As DisplayOrder, ISNULL(CR.Cnt, 0) Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT DISTINCT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As ComplaintResult, LK.DisplayOrder " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.ComplaintResult.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          ") CR2 " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT DATEPART(YEAR, ComplaintDate) As YYYY, DATEPART(MONTH, ComplaintDate) As MM, RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, COUNT(DISTINCT CM.ComplaintMasterId) AS ComplaintCnt, COUNT(DISTINCT CASE WHEN CM.OrgId IS NOT NULL THEN CONVERT(VARCHAR, CM.OrgId) ELSE ISNULL(CM.ConcernedOrgName, '') END) As OrgCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) " + Environment.NewLine +
                          "   GROUP BY DATEPART(YEAR, ComplaintDate), DATEPART(MONTH, ComplaintDate), RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) " + Environment.NewLine +
                          ") CM ON CR2.ComplaintDate = CM.ComplaintDate " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7) As ComplaintDate, LK.Code As ComplaintResult,  " + Environment.NewLine +                          
                          "          COUNT(CR.ComplaintResultId) As Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.ComplaintResult.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintResult CR ON CR.ComplaintMasterId = CM.ComplaintMasterId AND CR.Result = LK.Code AND CR.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) " + Environment.NewLine +
                          "   GROUP BY RIGHT(CONVERT(VARCHAR, CM.ComplaintDate, 105), 7), LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") CR ON CR2.ComplaintDate = CR.ComplaintDate AND CR2.ComplaintResult = CR.ComplaintResult " + Environment.NewLine +
                          "ORDER BY RIGHT(CR2.ComplaintDate, 4), LEFT(CR2.ComplaintDate, 2), CR2.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R12Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R12Dto)))
               .List<R12Dto>();
            return list;
        }

        #endregion R12

        #region R13

        public IList<R13Dto> GetR13Report(DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();
            string condition = "CM.IsDeleted = 0 AND (CM.PspApprovalHistoryId IS NOT NULL OR CM.FdEventId IS NOT NULL) ";
            if (fromDate.HasValue)
            {
                condition += "AND CM.ComplaintDate >= :fromDate ";
                paramMap.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                condition += "AND CM.ComplaintDate <= :toDate ";
                paramMap.Add("toDate", toDate.Value);
            }

            string sql = ("SELECT CM.OrgRef, CM.EngOrgName, CM.ChiOrgName, CM.ComplaintCnt, CR.ComplaintResult, CR.DisplayOrder, CR.Cnt " + Environment.NewLine +
                          "FROM ( " + Environment.NewLine +
                          "   SELECT OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END As EngOrgName, OM.ChiOrgName, COUNT(CM.ComplaintMasterId) As ComplaintCnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   LEFT JOIN OrgMaster OM ON CM.OrgId = OM.OrgId AND OM.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY OM.OrgId, OM.OrgRef, CASE WHEN CM.OrgId IS NOT NULL THEN OM.EngOrgNameSorting ELSE ISNULL(CM.ConcernedOrgName, '') END, OM.ChiOrgName " + Environment.NewLine +
                          ") CM " + Environment.NewLine +
                          "LEFT JOIN ( " + Environment.NewLine +
                          "   SELECT CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END AS ConcernedOrgName, LK.Code As ComplaintResult, " + Environment.NewLine +
                          "          CONVERT(INT, ROW_NUMBER() OVER (PARTITION BY OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END ORDER BY LK.DisplayOrder)) As DisplayOrder, COUNT(CR.ComplaintResultId) As Cnt " + Environment.NewLine +
                          "   FROM ComplaintMaster CM " + Environment.NewLine +
                          "   INNER JOIN Lookup LK ON LK.Type = '" + LookupType.ComplaintResult.ToEnumValue() + "' AND LK.IsDeleted = 0 " + Environment.NewLine +
                          "   LEFT JOIN ComplaintResult CR ON CR.ComplaintMasterId = CM.ComplaintMasterId AND CR.Result = LK.Code AND CR.IsDeleted = 0 " + Environment.NewLine +
                          "   WHERE {0} " + Environment.NewLine +
                          "   GROUP BY CM.OrgId, CASE WHEN CM.OrgId IS NOT NULL THEN '' ELSE ISNULL(CM.ConcernedOrgName, '') END, LK.Code, LK.DisplayOrder " + Environment.NewLine +
                          ") CR ON (CM.OrgId IS NOT NULL AND CM.OrgId = CR.OrgId) OR (CM.OrgId IS NULL AND CR.OrgId IS NULL AND CM.EngOrgName = CR.ConcernedOrgName) " + Environment.NewLine +
                          "ORDER BY CM.OrgRef, EngOrgName, CR.DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R13Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R13Dto)))
               .List<R13Dto>();
            return list;
        }

        #endregion R13

        #region R14

        public IList<R14Dto> GetR14Report(int? fromYear, int? toYear, string complaintSource)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();

            string sql = "SELECT ComplaintDate, ComplainantName, DcLcContent, ConcernOrgName, ConcernChiOrgName, ComplaintEnclosureNum, ComplaintPartNum, " + Environment.NewLine +
                         "       ActionFileEnclosureNum, ActionFilePartNum, RecordTypeIndicator, ComplaintResultRemark, FollowUp, ReportPolice, OtherFollowUp " + Environment.NewLine +
                         "FROM PspReportR14View " + Environment.NewLine +
                         "WHERE {0} " + Environment.NewLine +
                         "ORDER BY ComplaintDate, ComplainantName";

            string condition = string.Empty;

            if (fromYear.HasValue)
            {
                condition = "YEAR(ComplaintDate) >= :fromYear AND ";
                paramMap.Add("fromYear", fromYear.Value);
            }

            if (toYear.HasValue)
            {
                condition += "YEAR(ComplaintDate) <= :toYear AND ";
                paramMap.Add("toYear", toYear.Value);
            }

            condition += "ComplaintSource = :complaintSource ";
            paramMap.Add("complaintSource", complaintSource);

            var query = this.Session.CreateSQLQuery(sql.FormatWith(condition));
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R14Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R14Dto)))
               .List<R14Dto>();
            return list;
        }

        #endregion R14

        #region R15

        public IList<R15Dto> GetR15Report()
        {
            string sql = "SELECT InformedByPolice, ReferralDate, MemoDate, ConcernOrgName, ConcernChiOrgName, CorrespondenceEnclosureNum, CorrespondencePartNum, " + Environment.NewLine +
                         "       PoliceRefNum, CaseNature, ResultDetail, EnclosureNum, PartNum, Remark " + Environment.NewLine +
                         "FROM PspReportR15View " + Environment.NewLine +
                         "ORDER BY [SN] ASC";

            var query = this.Session.CreateSQLQuery(sql);
            IList<R15Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R15Dto)))
               .List<R15Dto>();
            return list;
        }

        #endregion R15

        #region R16

        public IList<R16PspDto> GetR16PspReport(DateTime? criteriaDate)
        {
            //TODO: To be implement the FileRefLmNum definition in view
            string sql = "SELECT OrgId, EngOrgName, EventEndDate, PermitNum, FileRef, WithholdingBeginDate, WithholdingEndDate, ComplaintIndicator, PoliceIndicator, AuditedReportIndicator, OfficialReceiptIndicator, NewspaperCuttingIndicator, QualityOpinionDetail, DocRemark " + Environment.NewLine +
                         "FROM PspReportR16_PSPView ";

            if (criteriaDate != null)
            {
                DateTime _criteriaDate = criteriaDate ?? DateTime.Now;
                sql += Environment.NewLine +
                "WHERE CONVERT(DATE, '" + _criteriaDate.ToString("yyyy.MM.dd") + "', 102) " + Environment.NewLine +
                "BETWEEN WithholdingBeginDate AND ISNULL(WithholdingEndDate, GETDATE()) ";
            }

            sql += Environment.NewLine + "order by EngOrgName ASC";

            var query = this.Session.CreateSQLQuery(sql);
            IList<R16PspDto> resultList = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R16PspDto)))
               .List<R16PspDto>();
            return resultList;
        }

        public IList<R16FdDto> GetR16FdReport(DateTime? criteriaDate)
        {
            //TODO: To be implement the FileRef definition in view
            string sql = "SELECT OrgId, EngOrgName, FlagDay, TWR, PermitNum, FileRef, WithholdingBeginDate, WithholdingEndDate, ComplaintIndicator, PoliceIndicator, ReviewReportIndicator, NewspaperIndicator, AfsReceiveIndicator, DocReceiveRemark " + Environment.NewLine +
                         "FROM PspReportR16_FDView ";

            if (criteriaDate != null)
            {
                DateTime _criteriaDate = criteriaDate ?? DateTime.Now;
                sql += Environment.NewLine +
                "WHERE CONVERT(DATE, '" + _criteriaDate.ToString("yyyy.MM.dd") + "', 102) " + Environment.NewLine +
                "BETWEEN WithholdingBeginDate AND ISNULL(WithholdingEndDate, GETDATE()) ";
            }

            sql += Environment.NewLine + "order by EngOrgName ASC";

            var query = this.Session.CreateSQLQuery(sql);
            IList<R16FdDto> resultList = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R16FdDto)))
               .List<R16FdDto>();
            return resultList;
        }

        public IList<R16ComplaintDto> GetR16ComplaintReport(DateTime? criteriaDate)
        {
            String sql =
            "SELECT " + Environment.NewLine +
            "OrgId, EngOrgName, ComplaintDate, ComplaintSource, ActivityConcern, PermitNum, ComplaintRef, " + Environment.NewLine +
            "WithholdingBeginDate, WithholdingEndDate, WithholdingRemark, OtherWithholdingRemark " + Environment.NewLine +
            "FROM PspReportR16_ComplaintView ";

            if (criteriaDate != null)
            {
                DateTime _criteriaDate = criteriaDate ?? DateTime.Now;
                sql += Environment.NewLine +
                "WHERE CONVERT(DATE, '" + _criteriaDate.ToString("yyyy.MM.dd") + "', 102) " + Environment.NewLine +
                "BETWEEN WithholdingBeginDate AND ISNULL(WithholdingEndDate, GETDATE()) ";
            }

            sql += Environment.NewLine + "ORDER BY EngOrgName ASC ";

            var query = this.Session.CreateSQLQuery(sql);
            IList<R16ComplaintDto> resultList = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R16ComplaintDto)))
               .List<R16ComplaintDto>();
            return resultList;
        }

        #endregion R16

        #region R17

        public IList<R17MainDto> GetR17MainReport(int ComplaintMasterId)
        {
            string sql = ("select cm.ComplaintRef, lkcrt.EngDescription as ComplaintRecordType, lksrc.EngDescription as ComplaintSource, cm.ComplaintSourceRemark, " + Environment.NewLine +
                          "       cm.ComplaintDate, Ctr.ComplaintTime, cm.FirstComplaintDate, cm.SwdUnit, cm.ComplainantName, ctr.ComplainantTelNum," + Environment.NewLine +
                          "       case when cm.OrgId is null then cm.ConcernedOrgName else om.EngOrgNameSorting end OrgName, pa.PermitNum as PspPermitNum , fe.PermitNum as FdPermitNum," + Environment.NewLine +
                          "       case when cm.ActivityConcern = 'Others' then cm.OtherActivityConcern else lkac.EngDescription end as ActivityConcern," + Environment.NewLine +
                          "       case when cm.CollectionMethod = 'Others' then cm.OtherCollectionMethod else lkcm.EngDescription end As CollectionMethod, " + Environment.NewLine +
                          "       cm.FundRaisingDate As RaisingDate, cm.FundRaisingTime As RaisingTime, cm.FundRaisingLocation, CONVERT(int, cm.FundRaiserInvolve) AS FundRaiserInvolve, " + Environment.NewLine +
                          "       cm.DcLcContent As ComplaintContentRemark, lkps.EngDescription as ProcessingStatus," + Environment.NewLine +
                          "       cm.ReplyDueDate, cmrel.ComplaintRef as RelatedComplaintRef, cm.ComplaintResultRemark, " + Environment.NewLine +
                          "       cm.WithholdingBeginDate, cm.WithholdingEndDate, cm.WithholdingRemark " + Environment.NewLine +
                          "from ComplaintMaster Cm " + Environment.NewLine +
                          "left join Lookup lkcrt on cm.ComplaintRecordType = lkcrt.Code and lkcrt.Type = '" + LookupType.ComplaintRecordType.ToEnumValue() + "' and lkcrt.IsDeleted = 0" + Environment.NewLine +
                          "left join Lookup lksrc on cm.ComplaintSource = lksrc.Code and lksrc.Type = '" + LookupType.ComplaintSource.ToEnumValue() + "' and lksrc.IsDeleted = 0" + Environment.NewLine +
                          "left join OrgMaster om on cm.OrgId = om.OrgId and om.IsDeleted = 0" + Environment.NewLine +
                          "left join PspApprovalHistory pa on cm.PspApprovalHistoryId = pa.PspApprovalHistoryId and pa.IsDeleted = 0" + Environment.NewLine +
                          "left join FdEvent fe on cm.FdEventId = fe.FdEventId and fe.IsDeleted = 0" + Environment.NewLine +
                          "left join Lookup lkac on cm.ActivityConcern = lkac.Code and lkac.Type = '" + LookupType.ComplaintActivityConcern.ToEnumValue() + "' and lkac.IsDeleted = 0" + Environment.NewLine +
                          "left join Lookup lkcm on cm.CollectionMethod = lkcm.Code and lkcm.Type = '" + LookupType.ComplaintCollectionMethod.ToEnumValue() + "' and lkcm.IsDeleted = 0 " + Environment.NewLine +
                          "outer apply ( " + Environment.NewLine +
                          " select top 1 ComplaintTime, ComplainantTelNum " + Environment.NewLine +
                          " from ComplaintTelRecord ctr " + Environment.NewLine +
                          " where cm.ComplaintMasterId = ctr.ComplaintMasterId and ctr.IsDeleted = 0 " + Environment.NewLine +
                          " order by ctr.ComplaintDate, ctr.ComplaintTime, ctr.TelComplaintRef " + Environment.NewLine +
                          " ) ctr " + Environment.NewLine +
                          "left join Lookup lkps on cm.ProcessStatus = lkps.Code and lkps.Type = '" + LookupType.ComplaintProcessStatus.ToEnumValue() + "' and lkps.IsDeleted = 0" + Environment.NewLine +
                          "left join ComplaintMaster cmrel on cm.RelatedComplaintMasterId = cmrel.ComplaintMasterId and cmrel.IsDeleted = 0" + Environment.NewLine +
                          "where cm.ComplaintMasterId = :ComplaintMasterId and cm.IsDeleted = 0");

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17MainDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17MainDto)))
               .List<R17MainDto>();
            return list;
        }

        public IList<R17ResultDto> GetR17ResultReport(int ComplaintMasterId)
        {
            string sql = "SELECT lncn.EngDescription as NonComplianceNature, cr.OtherNonComplianceNature, lr.EngDescription as Result " + Environment.NewLine +
                         "FROM ComplaintResult cr " + Environment.NewLine +
                         "INNER JOIN Lookup lncn on CHARINDEX(CONCAT(',', lncn.Code, ','), CONCAT(',', cr.NonComplianceNature, ',')) > 0 and lncn.Type = '" + LookupType.ComplaintNonComplianceNature.ToEnumValue() + "' and lncn.IsDeleted = 0 " + Environment.NewLine +
                         "LEFT JOIN Lookup lr on cr.Result = lr.Code and lr.Type = '" + LookupType.ComplaintResult.ToEnumValue() + "' and lr.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE ComplaintMasterId = :ComplaintMasterId " + Environment.NewLine +
                         "ORDER BY cr.ComplaintResultId, lncn.DisplayOrder";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17ResultDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17ResultDto)))
               .List<R17ResultDto>();
            return list;
        }

        public IList<R17TelDto> GetR17TelReport(int ComplaintMasterId)
        {
            string sql = "SELECT TelComplaintRef, ComplaintDate AS TelComDate, ComplaintTime AS TelComTime, ComplainantName AS TelComName, ComplainantTelNum AS TelComTelNum, " + Environment.NewLine +
                         "       OrgName AS TelOrgName, pa.PermitNum AS TelPspPermit, fe.PermitNum AS TelFdPermit, ComplaintContentRemark As TelComplaintContentRemark, OfficerName, OfficerPost " + Environment.NewLine +
                         "FROM ComplaintTelRecord ct " + Environment.NewLine +
                         "LEFT JOIN PspApprovalHistory pa ON ct.PspApprovalHistoryId = pa.PspApprovalHistoryId and pa.IsDeleted = 0 " + Environment.NewLine +
                         "LEFT JOIN FdEvent fe ON ct.FdEventId = fe.FdEventId and fe.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE ct.ComplaintMasterId = :ComplaintMasterId and ct.IsDeleted = 0 " + Environment.NewLine +
                         "ORDER BY ComplaintDate, ComplaintTime, TelComplaintRef, ComplaintMasterId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17TelDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17TelDto)))
               .List<R17TelDto>();
            return list;
        }

        public IList<R17FollowUpDto> GetR17FollowUpReport(int ComplaintMasterId)
        {
            string sql = "SELECT EnclosureNum, ContactOrgName, ContactPersonName As CPersonName, ContactDate, OrgRemark, l.EngDescription As FollowUpLetterType, FollowUpLetterReceiver, FollowUpLetterIssueDate, FollowUpLetterRemark, " + Environment.NewLine +
                         "       FollowUpLetterActionFileRefEnclosureNum + ' ' + FollowUpLetterActionFileRefPartNum  As FollowUpLetterActionFileRef, FollowUpOrgReply, FollowUpOrgReplyDate " + Environment.NewLine +
                         "FROM ComplaintFollowUpAction cfa " + Environment.NewLine +
                         "LEFT JOIN Lookup l on cfa.FollowUpLetterType = l.Code and L.Type = '" + LookupType.FollowUpLetterType.ToEnumValue() + "' and L.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE cfa.ComplaintMasterId = :ComplaintMasterId and cfa.FollowUpIndicator = 1 and cfa.IsDeleted = 0 " + Environment.NewLine +
                         "ORDER BY EnclosureNum, cfa.ContactDate, cfa.ComplaintFollowUpActionId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17FollowUpDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17FollowUpDto)))
               .List<R17FollowUpDto>();
            return list;
        }

        public IList<R17FollwUpPoliceDto> GetR17FollowUpPoliceReport(int ComplaintMasterId)
        {
            string sql = "SELECT EnclosureNum, VerbalReportDate As VDate, PoliceStation, PoliceOfficerName, RnNum, RnRemark, WrittenReferralDate As RDate, ReferralPoliceStation, ActionFileRefEnclosureNum + ' ' + ActionFileRefPartNum  As ActionFileRef, " + Environment.NewLine +
                         "        ISNULL(L.EngDescription, '') As PoliceInvestigation, PoliceInvestigationResult, PoliceReplyDate, " + Environment.NewLine +
                         "       ConvictedPersonName, ConvictedPersonPosition, OffenceDetail, SentenceDetail, CourtRefNum, CourtHearingDate, PoliceRemark " + Environment.NewLine +
                         "FROM ComplaintFollowUpAction cfa " + Environment.NewLine +
                         "LEFT JOIN Lookup L ON cfa.PoliceInvestigation = L.Code and L.Type = '" + LookupType.ComplaintInvestigationResult.ToEnumValue() + "' and L.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE cfa.ComplaintMasterId = :ComplaintMasterId and cfa.ReportPoliceIndicator = 1 and cfa.IsDeleted = 0 " + Environment.NewLine +
                         "ORDER BY EnclosureNum, VerbalReportDate, ComplaintFollowUpActionId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17FollwUpPoliceDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17FollwUpPoliceDto)))
               .List<R17FollwUpPoliceDto>();
            return list;
        }

        public IList<R17FollowUpOtherDto> GetR17FollowUpOtherReport(int ComplaintMasterId)
        {
            string sql = "SELECT EnclosureNum, OtherFollowUpPartyName, OtherFollowUpFileRefEnclosureNum + ' ' + OtherFollowUpFileRefPartNum As OtherFollowUpFileRef, OtherFollowUpContactDate, OtherFollowUpRemark " + Environment.NewLine +
                         "FROM ComplaintFollowUpAction " + Environment.NewLine +
                         "WHERE ComplaintMasterId = :ComplaintMasterId and OtherFollowUpIndicator = 1 and IsDeleted = 0 " + Environment.NewLine +
                         "ORDER BY EnclosureNum, OtherFollowUpContactDate, ComplaintFollowUpActionId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17FollowUpOtherDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17FollowUpOtherDto)))
               .List<R17FollowUpOtherDto>();
            return list;
        }

        public IList<R17FollowUpLetterDto> GetR17FollowUpLetterReport(int ComplaintMasterId)
        {
            string sql = "SELECT l.Code As FollowUpLetterType, count(cfa.ComplaintFollowUpActionId) As Cnt, " + Environment.NewLine +
                         "       ISNULL(STUFF(( " + Environment.NewLine +
                         "         SELECT ', ' + CONVERT(VARCHAR, FollowUpLetterIssueDate, 103) " + Environment.NewLine +
                         "         FROM ComplaintFollowUpAction cfa2 " + Environment.NewLine +
                         "         WHERE cfa2.ComplaintMasterId = cfa.ComplaintMasterId and cfa2.FollowUpLetterType = l.Code " + Environment.NewLine +
                         "         FOR XML PATH('') " + Environment.NewLine +
                         "		 ), 1, 2, ''), '') As IssueDates " + Environment.NewLine +
                         "FROM Lookup l " + Environment.NewLine +
                         "LEFT JOIN ComplaintFollowUpAction cfa on cfa.FollowUpLetterType = l.Code and cfa.ComplaintMasterId = :ComplaintMasterId AND cfa.FollowUpIndicator = 1 " + Environment.NewLine +
                         "WHERE l.Type = '" + LookupType.FollowUpLetterType.ToEnumValue() + "' and l.Code IN ('02', '03', '04') and L.IsDeleted = 0 " + Environment.NewLine +
                         "GROUP BY l.Code, l.DisplayOrder, cfa.ComplaintMasterId " + Environment.NewLine +
                         "ORDER BY l.DisplayOrder";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17FollowUpLetterDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17FollowUpLetterDto)))
               .List<R17FollowUpLetterDto>();
            return list;
        }

        public IList<R17PoliceCaseDto> GetR17PoliceCaseReport(int ComplaintMasterId)
        {
            string sql = "SELECT CaseInvestigateRefNum AS RefNum, ReferralDate, MemoDate, " + Environment.NewLine +
                         "       om.EngOrgNameSorting + ' ' + om.ChiOrgName AS ConcernOrgName, " + Environment.NewLine +
                         "       L.EngDescription AS InvestigationResult, " + Environment.NewLine +
                         "       CorrespondenceEnclosureNum + '-' + CorrespondencePartNum AS ActionFile, PoliceRefNum " + Environment.NewLine +
                         "FROM ComplaintPoliceCase Cpc " + Environment.NewLine +
                         "LEFT JOIN OrgMaster om ON Cpc.OrgId = om.Orgid AND om.IsDeleted = 0 " + Environment.NewLine +
                         "LEFT JOIN Lookup L ON L.Type = '" + LookupType.ComplaintInvestigationResult.ToEnumValue() + "' and L.Code = Cpc.InvestigationResult " + Environment.NewLine +
                         "WHERE Cpc.ComplaintMasterId = :ComplaintMasterId and Cpc.IsDeleted = 0";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17PoliceCaseDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17PoliceCaseDto)))
               .List<R17PoliceCaseDto>();
            return list;
        }

        public IList<R17FromOthersDto> GetR17FromOthersReport(int ComplaintMasterId)
        {
            string sql = "SELECT RefNum, ReferralDate, MemoDate, MemoFromPoliceDate AS MemoPoliceDate, " + Environment.NewLine +
                         "       CASE WHEN EnquiryDepartment = 'Others' THEN L.EngDescription + ' - ' + OtherEnquiryDepartment ELSE L.EngDescription END AS EnquiryDepartment, " + Environment.NewLine +
                         "       OrgInvolved, EnquiryContent, EnclosureNum, Remark " + Environment.NewLine +
                         "FROM ComplaintOtherDepartmentEnquiry Code " + Environment.NewLine +
                         "LEFT JOIN Lookup L ON L.Type = 'Department' and L.Code = '" + LookupType.Department.ToEnumValue() + "' and L.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE Code.ComplaintMasterId = :ComplaintMasterId and Code.IsDeleted = 0";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetInt32("ComplaintMasterId", ComplaintMasterId);

            IList<R17FromOthersDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R17FromOthersDto)))
               .List<R17FromOthersDto>();
            return list;
        }

        #endregion R17

        #region R18

        public IList<R18Dto> GetR18Report()
        {
            string sql = "SELECT EngOrgName, ChiOrgName, OrgRef, ComplaintsReceivedB4, ComplaintsReceivedSince, NoOfWarningLetterB4, PspRef, PermitIssueDate, PermitNum, EventPeriodFrom, EventPeriodTo, NoOfEvents, " + Environment.NewLine +
                         "       ComplaintsReceivedAF, NoOfWarningLetterAF, PspRef2nd, PermitIssueDate2nd, PermitNum2nd, EventPeriodFrom2nd, EventPeriodTo2nd, NoOfEvents2nd, " + Environment.NewLine +
                         "       RejectionLetterDate, RepresentationReceiveDate, Remark " + Environment.NewLine +
                         "from PspReportR18View " + Environment.NewLine +
                         "order by EngOrgName, PermitIssueDate";

            var query = this.Session.CreateSQLQuery(sql);
            IList<R18Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R18Dto)))
               .List<R18Dto>();
            return list;
        }

        #endregion R18

        #region R19

        public IList<R19Dto> GetR19Report()
        {
            string sql = "SELECT ComplaintDate, OrgRef, EngOrgNameSorting, ChiOrgName, FundRaisingDate, ConvictedPersonName, ConvictedPersonPosition, OffenceDetail, SentenceDetail, PoliceRefNum, CourtRefNum, CourtHearingDate, CaseReferralRemark, InformedByPolice "
                       + "FROM PspReportR19View "
                       + "WHERE InvestigationResult = '1' "
                       + "ORDER BY ComplaintMasterId";

            var query = this.Session.CreateSQLQuery(sql);

            IList<R19Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R19Dto)))
               .List<R19Dto>();
            return list;
        }

        #endregion R19

        #region R21

        public IList<R21SummaryDto> GetR21SummaryReport(int? year)
        {
            string sql = "SELECT PspYear, PspReceived, StatCount1, StatCount2, StatCount3, StatCount4, StatCount5 " + Environment.NewLine +
                         "FROM PspReportR21_SummaryView ";

            string condition = year.HasValue ? "WHERE PspYear between DATEPART(YEAR, DATEADD(YEAR, -{0}, GETDATE())) AND DATEPART(YEAR, GETDATE()) ".FormatWith(year.Value) : "";
            string orderBy = " ORDER BY PspYear";

            sql = sql + condition + orderBy;

            var query = this.Session.CreateSQLQuery(sql);
            IList<R21SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R21SummaryDto)))
               .List<R21SummaryDto>();
            return list;
        }

        public IList<R21ARDto> GetR21ARReport(int? year)
        {
            string sql = "SELECT PspYear, OrgRef, EngOrgName, SubventedIndicator, ChiOrgName, PspRef, PermitNum, GrossProceed, Expenditure, NetProceed, ExpenditurePercent, DocRemark " + Environment.NewLine +
                         "FROM PspReportR21_ARView ";

            string condition = year.HasValue ? "WHERE PspYear between DATEPART(YEAR, DATEADD(YEAR, -{0}, GETDATE())) AND DATEPART(YEAR, GETDATE()) ".FormatWith(year.Value) : "";
            string orderBy = " ORDER BY PspYear, PermitNum";

            sql = sql + condition + orderBy;

            var query = this.Session.CreateSQLQuery(sql);
            IList<R21ARDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R21ARDto)))
               .List<R21ARDto>();
            return list;
        }

        #endregion R21

        #region R22

        public IList<R22Dto> GetR22Report(DateTime? fromDate, DateTime? toDate)
        {
            string sql = "select EngOrgName, ChiOrgName, PspRef, PermitIssueDate, ApprovalType, EventPeriodFrom, EventPeriodTo, FlagDayCount, TotalEventCount " + Environment.NewLine +
                         "from PspReportR22View ";

            string condition = "WHERE 1=1 ";
            string orderBy = "order by EventPeriodFrom, EngOrgName, EventPeriodTo";

            if (fromDate.HasValue)
                condition += " and EventPeriodFrom >= convert(datetime,'" + fromDate.Value.ToString("dd-MM-yyyy") + "',105) ";
            if (toDate.HasValue)
                condition += " and EventPeriodTo <= convert(datetime,'" + toDate.Value.ToString("dd-MM-yyyy") + "',105) ";
            sql = sql + condition + orderBy;

            var query = this.Session.CreateSQLQuery(sql);
            IList<R22Dto> resultList = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R22Dto)))
               .List<R22Dto>();
            return resultList;
        }

        #endregion R22

        #region R23

        public IList<R23Dto> GetR23Report(int? from, int? to)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();

            string sql = "SELECT EngOrgName, ChiOrgName, OrgRef, PspYear, R1, R2, R3, R4 , AfrDate, PermitNums, OrgAnnualIncome, LastVersionDate , BelowInd FROM PspReportR23_AllView ";

            string orderBy = "ORDER BY EngOrgName ASC ";

            if (from.HasValue || to.HasValue)
            {
                string condition = "WHERE 1 = 1 ";

                if (from.HasValue)
                {
                    condition += " AND convert(int,PspYear) >=  :fromYear ";
                    paramMap.Add("fromYear", from.Value);
                }

                if (to.HasValue)
                {
                    condition += " AND convert(int,PspYear) <=  :toYear ";
                    paramMap.Add("toYear", to.Value);
                }

                sql += condition;
            }

            sql += orderBy;

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R23Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R23Dto)))
               .List<R23Dto>();

            return list;
        }

        #endregion R23

        #region R24

        public IList<R24Dto> GetR24Report(string from, int twr)
        {
            string condition = string.Empty;

            if (from.IsNotNullOrEmpty())
                condition = "and YEAR(FlagDay) = :from ";

            string sql = ("SELECT FlagDay, Region, EngOrgNameSorting, ChiOrgName, NetProceed, ApplyPledgingMechanismIndicator, Benchmark " + Environment.NewLine +
                          "FROM PspReportR24View " + Environment.NewLine +
                          "WHERE TWR = :twr {0} " + Environment.NewLine +
                          "ORDER BY FlagDay, DisplayOrder").FormatWith(condition);

            var query = this.Session.CreateSQLQuery(sql);

            query.SetInt32("twr", twr);
            if (from.IsNotNullOrEmpty())
                query.SetString("from", from);

            IList<R24Dto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R24Dto)))
               .List<R24Dto>();
            return list;
        }

        #endregion R24

        #region R25

        public IList<R25AllDto> GetR25AllReport(DateTime? sendDate, DateTime? toDate)
        {
            Dictionary<string, object> paramMap = new Dictionary<string, object>();

            //string sql = "SELECT EngOrgName, ChiOrgName, SendWithPsp, Subvented, FullyAdopt, PartiallyAdopt, WillNotAdopt, Result, " + Environment.NewLine +
            //             "       A1, A2, A3, A4, A5, A6, A7, A8, B1, B2, B3, B4, B5, B6, C1, C2, C3, C4, C5, C6, C7, Others, " + Environment.NewLine +
            //             "       PromulgationReason, ReturnMail, Remarks, R1.SendDate, ReplySlipReceiveDate, RemarkB, ReplySlipDate, PartNum, EnclosureNum " + Environment.NewLine +
            //             "FROM PspReportR25_AllView R1 " + Environment.NewLine +
            //             "WHERE 1 = 1 " + Environment.NewLine;

            string sql = "SELECT om.EngOrgNameSorting As EngOrgName, om.ChiOrgName, " + Environment.NewLine +
                         "       CASE WHEN (om.SubventedIndicator = 1) THEN 'Y' ELSE '' END as Subvented, " + Environment.NewLine +
                         "       CASE WHEN (op.OrgReply = '1') THEN 'Y' ELSE '' END as FullyAdopt, " + Environment.NewLine +
                         "       CASE WHEN (op.OrgReply = '2') THEN 'Y' ELSE '' END as PartiallyAdopt, " + Environment.NewLine +
                         "       CASE WHEN (op.OrgReply = '3') THEN 'Y' ELSE '' END as WillNotAdopt, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A1') as A1, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A2') as A2, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A3') as A3, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A4') as A4, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A5') as A5, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A6') as A6, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A7') as A7, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'A8') as A8, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B1') as B1, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B2') as B2, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B3') as B3, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B4') as B4, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B5') as B5, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'B6') as B6, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C1') as C1, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C2') as C2, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C3') as C3, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C4') as C4, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C5') as C5, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C6') as C6, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'C7') as C7, " + Environment.NewLine +
                         "       dbo.ufnGetProvisionIndicator(op.OrgRefGuidePromulgationId, 'Others') as Others, " + Environment.NewLine +
                         "       op.PromulgationReason, op.Remarks, op.SendDate, op.ReplySlipReceiveDate, op.ReplySlipDate,  " + Environment.NewLine +
                         "       op.PartNum, op.EnclosureNum " + Environment.NewLine +
                         "FROM ( " + Environment.NewLine +
                         "	SELECT DISTINCT OrgId " + Environment.NewLine +
                         "	FROM OrgRefGuidePromulgation " + Environment.NewLine +
                         "  WHERE OrgReply != '5' " + Environment.NewLine +
                         ") org " + Environment.NewLine +
                         "CROSS APPLY ( " + Environment.NewLine +
                         "	SELECT TOP 1 * " + Environment.NewLine +
                         "	FROM OrgRefGuidePromulgation ofg " + Environment.NewLine +
                         "	WHERE ofg.OrgId = org.OrgId AND ofg.OrgReply != '5' {0} " + Environment.NewLine +
                         "	ORDER BY CASE WHEN ISNULL(ReplySlipReceiveDate, 0) = 0 THEN 0 ELSE 1 END desc, ReplySlipReceiveDate desc, SendDate desc " + Environment.NewLine +
                         ") op " + Environment.NewLine +
                         "INNER JOIN OrgMaster om ON org.OrgId = om.OrgId " + Environment.NewLine +
                         "WHERE om.IsDeleted = 0 " + Environment.NewLine +
                         "ORDER BY EngOrgName";

            string where = string.Empty;

            if (sendDate.HasValue)
            {
                where += "AND SendDate >= :sentDate " + Environment.NewLine;
                paramMap.Add("sentDate", sendDate.Value);
            }

            if (toDate.HasValue)
            {
                where += "AND SendDate <= :toDate " + Environment.NewLine;
                paramMap.Add("toDate", toDate.Value);
            }

            sql = sql.FormatWith(where);

            var query = this.Session.CreateSQLQuery(sql);
            foreach (KeyValuePair<string, object> kvp in paramMap)
                query.SetParameter(kvp.Key, kvp.Value);

            IList<R25AllDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R25AllDto)))
               .List<R25AllDto>();
            return list;
        }

        public IList<R25SummaryDto> GetR25SummaryReport()
        {
            string sql = "SELECT Sort, Cnt, FullyAdopt, PartiallyAdopt, WillNotAdopt, FullyAdoptNotInMailing, PartiallyAdoptNotInMailing, WillNotAdoptNotInMailing" + Environment.NewLine +
                         "FROM PspReportR25_SummaryView" + Environment.NewLine +
                         "ORDER BY Sort";

            var query = this.Session.CreateSQLQuery(sql);

            IList<R25SummaryDto> list = query
               .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R25SummaryDto)))
               .List<R25SummaryDto>();
            return list;
        }

        public IList<R25SubNGOs> GetR25SubNGOs()
        {
            //TODO: Remove function as it is no longer use due to report layout change
            string sql = "SELECT [EngOrgName],[ChiOrgName],[FullyAdopt],[PartiallyAdopt],[WillNotAdopt],[ReplySlipReceiveDate] FROM [dbo].[PspReportR25_SubNGOsView] ORDER BY [EngOrgName] ASC";

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<R25SubNGOs>();
            for (var i = 0; i < list.Count; i++)
            {
                var dto = new R25SubNGOs();
                object[] obj = list[i];
                dto.EngOrgName = Convert.ToString(obj[0]);
                dto.ChiOrgName = Convert.ToString(obj[1]);
                if (obj[2] != null && Convert.ToString(obj[2]) != "")
                    dto.FullyAdopt = Convert.ToString(obj[2]);
                if (obj[3] != null && Convert.ToString(obj[3]) != "")
                    dto.PartiallyAdopt = Convert.ToString(obj[3]);
                if (obj[4] != null && Convert.ToString(obj[4]) != "")
                    dto.WillNotAdopt = Convert.ToString(obj[4]);
                if (obj[5] != null)
                    dto.ReplySlipReceiveDate = Convert.ToDateTime(obj[5]);
                resultList.Add(dto);
            }
            return resultList;
        }

        public IList<R25NonSubNGOs> GetR25NonSubNGOs()
        {
            //TODO: Remove function as it is no longer use due to report layout change
            string sql = "SELECT [EngOrgName],[ChiOrgName] ,[FullyAdopt],[PartiallyAdopt],[WillNotAdopt],[ReplySlipReceiveDate] FROM [dbo].[PspReportR25_NonSubNGOsView] ORDER BY [EngOrgName] ASC";

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<R25NonSubNGOs>();
            for (var i = 0; i < list.Count; i++)
            {
                var dto = new R25NonSubNGOs();
                object[] obj = list[i];
                dto.EngOrgName = Convert.ToString(obj[0]);
                dto.ChiOrgName = Convert.ToString(obj[1]);
                if (obj[2] != null && Convert.ToString(obj[2]) != "")
                    dto.FullyAdopt = Convert.ToString(obj[2]);
                if (obj[3] != null && Convert.ToString(obj[3]) != "")
                    dto.PartiallyAdopt = Convert.ToString(obj[3]);
                if (obj[4] != null && Convert.ToString(obj[4]) != "")
                    dto.WillNotAdopt = Convert.ToString(obj[4]);
                if (obj[5] != null)
                    dto.ReplySlipReceiveDate = Convert.ToDateTime(obj[5]);
                resultList.Add(dto);
            }
            return resultList;
        }

        #endregion R25

        #region R26

        public IList<R26Dto> GetR26Report(DateTime? fromDate, DateTime? toDate)
        {
            string condition = "";

            if (fromDate.HasValue)
                condition += " and R26.EffectiveDate >= convert(datetime,'" + fromDate.Value.ToString("dd-MM-yyyy") + "',105) ";
            if (toDate.HasValue)
                condition += " and R26.EffectiveDate <= convert(datetime,'" + toDate.Value.ToString("dd-MM-yyyy") + "',105) ";

            string sql = string.Format("SELECT L_LAT.Code as AdviceCode, L_VT.Code As VenueCode, " + Environment.NewLine +
                                       "       R26.LegalAdviceCodeType, R26.LegalAdviceCodeSubType, R26.LegalAdviceCodeCode, R26.LegalAdviceDescription, " + Environment.NewLine +
                                       "       R26.EnclosureNum, R26.PartNum, R26.EffectiveDate, R26.PspRequire, R26.Remarks " + Environment.NewLine +
                                       "FROM Lookup L_LAT " + Environment.NewLine +
                                       "LEFT JOIN Lookup L_VT ON L_VT.Type = '" + LookupType.VenueType.ToEnumValue() + "' and L_LAT.Code = '01' and L_VT.IsDeleted = 0 " + Environment.NewLine +
                                       "LEFT JOIN PspReportR26View R26 ON R26.LegalAdviceType = L_LAT.Code " + Environment.NewLine +
                                       "AND (R26.VenueType = L_VT.Code OR L_LAT.Code  <> '01') {0} " + Environment.NewLine +
                                       "WHERE L_LAT.Type = '" + LookupType.LegalAdviceType.ToEnumValue() + "' and L_LAT.IsDeleted = 0 " + Environment.NewLine +
                                       "ORDER BY L_LAT.DisplayOrder, L_VT.DisplayOrder, R26.LegalAdviceCodeType, R26.LegalAdviceCodeSubType, R26.LegalAdviceCodeCode ", condition);

            var query = this.Session.CreateSQLQuery(sql);
            IList<R26Dto> list = query
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R26Dto)))
                .List<R26Dto>();

            return list;
        }

        #endregion R26

        public IList<object[]> GetObjListFromSql(string strSql, ref List<string> colNames)
        {
            var query = this.Session.CreateSQLQuery(strSql);

            QueryTransformer qt = new QueryTransformer();
            IList<object[]> resultList = query.SetResultTransformer(qt).List<object[]>();
            colNames = qt.columnsNames;

            return resultList;
        }
    }
}
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IComplaintMasterRepository : IRepository<ComplaintMaster, int>
    {
        List<ComplaintBringUpDto> GetComplaintBringUpSummary();

        List<CompEnqDto> GetLastFiveYrsCompEnqSummary();

        string GenerateComplaintRef(int year);

        IList<ComplaintMaster> GetRecordsByParam(string param);

        IPagedList<ComplaintMaster> GetPage(GridSettings grid, string fundRaisingLocation);

        IPagedList<ComplaintMasterSearchDto> GetPageByComplaintMasterSearchDto(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OthersFollowUpIndicator);

        ComplaintMaster GetByComplaintRef(string complaintRef);
    }

    public class ComplaintMasterRepository : BaseRepository<ComplaintMaster, int>, IComplaintMasterRepository
    {
        public ComplaintMasterRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<ComplaintMaster> GetPage(GridSettings grid, string fundRaisingLocation)
        {
            var query = this.Table;
            //if (!String.IsNullOrEmpty(fundRaisingLocation))
            //{
            //    query = query.Where(x => x.ComplaintTelRecord.Any(y => SqlMethods.Like(y.FundRaisingLocation, "%" + fundRaisingLocation + "%") && y.IsDeleted == false));
            //}
            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public List<ComplaintBringUpDto> GetComplaintBringUpSummary()
        {
            //fix permit concern when the problem is understood
            string sql = " select B.OrgRef, B.OtherEngOrgName , B.OtherChiOrgName,  " +
                         " 	A.ComplaintRef, A.ComplaintDate, L.EngDescription,  " +
                         " 	[dbo].[GetComplaintBringPermitNum](PspApprovalHistoryId,FdEventId) as PermitConcern, A.ActionFileEnclosureNum       " +
                         " from      " +
                         " 	ComplaintMaster A     " +
                         " Left join      " +
                         " 	OrgMaster B     " +
                         " 		on A.OrgId = B.OrgId    " +
                         " inner join    " +
                         " 	[Lookup] L   " +
                         " 		on A.ComplaintSource = L.code and L.Type = '" + LookupType.ComplaintSource.ToEnumValue() + "' " +
                         " where A.ReplyDueDate >= getdate() and ReplyDueDate < dateadd(DAY,10,getdate()) ";

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<ComplaintBringUpDto>();

            for (var i = 0; i < list.Count; i++)
            {
                var complaintBringUpDto = new ComplaintBringUpDto();
                object[] obj = list[i];
                var FunctionId = obj[0];
                complaintBringUpDto.OrgRef = Convert.ToString(obj[0]);
                complaintBringUpDto.EngChiOrgName = Convert.ToString(obj[1]) + "\n" + Convert.ToString(obj[2]);
                complaintBringUpDto.ComplaintRef = Convert.ToString(obj[3]);
                complaintBringUpDto.ComplaintDate = Convert.ToDateTime(obj[4]);
                complaintBringUpDto.ComplaintSource = Convert.ToString(obj[5]);
                complaintBringUpDto.PermitConcern = Convert.ToString(obj[6]);
                complaintBringUpDto.ActionFileEnclosureNum = Convert.ToString(obj[7]);

                resultList.Add(complaintBringUpDto);
            }

            return resultList;
        }

        public List<CompEnqDto> GetLastFiveYrsCompEnqSummary()
        {
            //string testSql = " declare @pComplaint varchar(2);   " +
            //                 " declare @pEnquiry varchar(2);   " +
            //                 " declare @sql varchar(MAX);   " +
            //                 "                                  " +
            //                 " select @pComplaint = code from [lookup] where EngDescription = 'Complaint' and type = 'ComplaintRecordType';   " +
            //                 " select @pEnquiry = code from [lookup] where EngDescription = 'Enquiry' and type = 'ComplaintRecordType';   " +
            //                 "                                  " +
            //                 " set @sql = 'select  year(ComplaintDate) as [Year],    " +
            //                 " sum (case when s.ComplaintRecordType = '+@pComplaint+' then 1 else 0 end) as ComplaintRecieved,   " +
            //                 " 0 as WarningIssuedLetter, " +
            //                 " sum (case when s.ComplaintRecordType = '+@pEnquiry+' then 1 else 0 end) as EnquiryRecieved,   " +
            //                 " sum (case when s.ComplaintRecordType = ''NA'' then 1 else 0 end) as NARecieved,   " +
            //                 " sum (case when s.ComplaintRecordType is null then 1 else 0 end) as NullRecieved   " +
            //                 "                                  " +
            //                 " from ComplaintMaster s   " +
            //                 " Where year(ComplaintDate) <= Year(GETDATE()) and year(ComplaintDate) >= Year(GETDATE()) - 5 " +
            //                 " group by year(ComplaintDate)   " +
            //                 " order by year(ComplaintDate) desc'   " +
            //                 "                                  " +
            //                 " exec(@sql) ";

            string testSql = "Select Year(ComplaintDate) as Year, CASE WHEN l.Code IN ('04', 'Others') THEN 'Others' ELSE l.Code END AS Code, Count(ComplaintRecordType) as Result " + Environment.NewLine +
                             "From [Lookup] l " + Environment.NewLine +
                             "Inner join ComplaintMaster cm on l.Code = cm.ComplaintRecordType and Year(cm.ComplaintDate) between Year(GETDATE()) - 5 and Year(GETDATE()) and cm.IsDeleted = 0 " + Environment.NewLine +
                             "Where l.[type] = 'complaintRecordType' and l.IsDeleted = 0 -- and Code in ('01', '02') " + Environment.NewLine +
                             "Group By Year(ComplaintDate) , CASE WHEN l.Code IN ('04', 'Others') THEN 'Others' ELSE l.Code END " + Environment.NewLine +
                             "UNION " + Environment.NewLine +
                             "Select Year(cm.ComplaintDate) as Year, 'reminding' as Code, Count(fa.FollowUpLetterType) as Result " + Environment.NewLine +
                             "From ComplaintMaster cm " + Environment.NewLine +
                             "Left Join ComplaintFollowUpAction fa on fa.FollowUpLetterType = '02' and cm.ComplaintMasterId = fa.ComplaintMasterId and fa.IsDeleted = 0 " + Environment.NewLine +
                             "Where Year(cm.ComplaintDate) between Year(GETDATE()) - 5 and Year(GETDATE()) and cm.IsDeleted = 0 " + Environment.NewLine +
                             "Group By Year(cm.ComplaintDate) " + Environment.NewLine +
                             "UNION " + Environment.NewLine +
                             "Select Year(cm.ComplaintDate) as Year, 'alert' as Code, Count(fa.FollowUpLetterType) as Result " + Environment.NewLine +
                             "from ComplaintMaster cm " + Environment.NewLine +
                             "Left Join ComplaintFollowUpAction fa on fa.FollowUpLetterType = '03' and cm.ComplaintMasterId = fa.ComplaintMasterId " + Environment.NewLine +
                             "Where Year(cm.ComplaintDate) between Year(GETDATE()) - 5 and Year(GETDATE()) and cm.IsDeleted = 0 " + Environment.NewLine +
                             "Group By Year(cm.ComplaintDate) " + Environment.NewLine +
                             "UNION " + Environment.NewLine +
                             "Select Year(cm.ComplaintDate) as Year, 'warning' as Code, Count(fa.FollowUpLetterType) as Result " + Environment.NewLine +
                             "from ComplaintMaster cm " + Environment.NewLine +
                             "Left Join ComplaintFollowUpAction fa on fa.FollowUpLetterType = '04' and cm.ComplaintMasterId = fa.ComplaintMasterId " + Environment.NewLine +
                             "Where Year(cm.ComplaintDate) between Year(GETDATE()) - 5 and Year(GETDATE()) and cm.IsDeleted = 0 " + Environment.NewLine +
                             "Group By Year(cm.ComplaintDate) " + Environment.NewLine +
                             "UNION " + Environment.NewLine +
                             "Select Year(ComplaintDate) as Year, 'progress' as Code, Count(ComplaintMasterId) as Result " + Environment.NewLine +
                             "From ComplaintMaster " + Environment.NewLine +
                             "Where Year(ComplaintDate) between Year(GETDATE()) - 5 and Year(GETDATE()) and ProcessStatus = '1' and IsDeleted = 0 " + Environment.NewLine +
                             "Group By Year(ComplaintDate) " + Environment.NewLine +
                             "Order By 1 desc, 2";

            IList<object[]> list = this.Session.CreateSQLQuery(testSql).List<object[]>();
            var resultList = new List<CompEnqDto>();

            for (var i = 0; i < list.Count; i++)
            {
                var CompEnqDto = new CompEnqDto();
                object[] obj = list[i];
                CompEnqDto.Year = Convert.ToString(obj[0]);
                CompEnqDto.Code = Convert.ToString(obj[1]);
                CompEnqDto.Count = Convert.ToInt32(obj[2]);

                resultList.Add(CompEnqDto);
            }

            var returnlist = resultList.GroupBy(rl => rl.Year)
                                  .Select(
                                        g => new CompEnqDto
                                        {
                                            Year = g.Key,

                                            CountList = g.Select(x => new { x.Code, x.Count }).AsEnumerable().ToDictionary(ri => ri.Code, rs => rs.Count),
                                        }

                                  ).Take(5).ToList();

            return returnlist;
        }

        public string GenerateComplaintRef(int year)
        {
            string prefix = "(" + year + ")C";
            string suffix;
            var list = this.Table.Where(x => SqlMethods.Like(x.ComplaintRef, "%" + prefix + "%")).OrderBy("ComplaintMasterId", "desc").ToList();
            if (list != null && list.Count() > 0)
            {
                var complaint = list.First();
                string complaintRef = complaint.ComplaintRef;
                var tempSuffix = complaintRef.Substring(complaintRef.Length - 3, 3);
                suffix = (Convert.ToInt32(tempSuffix) + 1).ToString().PadLeft(3, '0');
            }
            else
            {
                suffix = "001";
            }

            return prefix + suffix;
        }

        public IList<ComplaintMaster> GetRecordsByParam(string param)
        {
            var hql = "from ComplaintMaster c ";
            IQuery hqlQuery = null;
            string[] values;
            IList<string> paramList = null;
            if (param != null && param.Length > 0)
            {
                var where = "where c.ComplaintMasterId IN (:param)";
                hql = hql + where;
                values = param.Split(',');
                paramList = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    var value = values[i];
                    if (value != null && value.Length > 0)
                    {
                        paramList.Add(values[i]);
                    }
                }
            }
            hql = hql + "order by c.ComplaintMasterId asc";
            hqlQuery = this.Session.CreateQuery(hql);
            hqlQuery.SetParameterList("param", paramList);

            var list = hqlQuery.List<ComplaintMaster>();
            return list;
        }

        public IPagedList<ComplaintMasterSearchDto> GetPageByComplaintMasterSearchDto(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OtherFollowUpIndicator)
        {
            IQueryable<ComplaintMasterSearchDto> query = null;

            if (IsFollowUp)
            {
                query = (from c in this.Table.Where(x => x.ComplaintFollowUpAction.Any(y => y.FollowUpIndicator == FollowUpIndicator)
                             && x.ComplaintFollowUpAction.Any(y => y.ReportPoliceIndicator == ReportPoliceIndicator && y.IsDeleted == false)
                             && x.ComplaintFollowUpAction.Any(y => y.OtherFollowUpIndicator == OtherFollowUpIndicator && y.IsDeleted == false))
                         let TelRecordNum = c.ComplaintTelRecord.Count()
                         let FollowUpActionRecordNum = c.ComplaintFollowUpAction.Count()
                         let PoliceCaseNum = c.ComplaintPoliceCase.Count()
                         let OtherDepartmentEnquiryNum = c.ComplaintOtherDepartmentEnquiry.Count()
                         select new ComplaintMasterSearchDto
                         {
                             ComplaintMasterId = c.ComplaintMasterId,
                             OrgRef = c.OrgMaster != null ? c.OrgMaster.OrgRef : "",
                             //                             EngChiOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + c.OrgMaster.ChiOrgName : "",
                             EngChiOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + c.OrgMaster.ChiOrgName : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                             EngOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgName : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                             ChiOrgName = c.OrgMaster != null ? c.OrgMaster.ChiOrgName : "",
                             SubventedIndicator = c.OrgMaster != null ? c.OrgMaster.SubventedIndicator : false,
                             DisableIndicator = c.OrgMaster != null ? c.OrgMaster.DisableIndicator : false,
                             RegistrationType1 = c.OrgMaster != null ? c.OrgMaster.RegistrationType1 : "",
                             RegistrationType2 = c.OrgMaster != null ? c.OrgMaster.RegistrationType2 : "",
                             RegistrationOtherName1 = c.OrgMaster != null ? c.OrgMaster.RegistrationOtherName1 : "",
                             RegistrationOtherName2 = c.OrgMaster != null ? c.OrgMaster.RegistrationOtherName2 : "",
                             ComplaintRecordType = c.ComplaintRecordType,
                             ComplaintRef = c.ComplaintRef,
                             ComplaintSource = c.ComplaintSource,
                             ActivityConcern = c.ActivityConcern,
                             OtherActivityConcern = c.OtherActivityConcern,
                             ComplaintDate = c.ComplaintDate,
                             FirstComplaintDate = c.FirstComplaintDate,
                             NonComplianceNature = c.NonComplianceNature,
                             FundRaisingLocation = c.FundRaisingLocation,
                             ProcessStatus = c.ProcessStatus,
                             TelRecordNum = TelRecordNum,
                             FollowUpActionRecordNum = FollowUpActionRecordNum,
                             PoliceCaseNum = PoliceCaseNum,
                             OtherDepartmentEnquiryNum = OtherDepartmentEnquiryNum,
                             ComplaintResult = c.ComplaintResult,
                             WithholdingRemark = c.WithholdingRemark,
                         }).AsQueryable();
            }
            else
            {
                query = (from c in this.Table
                         let TelRecordNum = c.ComplaintTelRecord.Count()
                         let FollowUpActionRecordNum = c.ComplaintFollowUpAction.Count()
                         let PoliceCaseNum = c.ComplaintPoliceCase.Count()
                         let OtherDepartmentEnquiryNum = c.ComplaintOtherDepartmentEnquiry.Count()
                         select new ComplaintMasterSearchDto
                         {
                             ComplaintMasterId = c.ComplaintMasterId,
                             OrgRef = c.OrgMaster != null ? c.OrgMaster.OrgRef : "",
                             //EngChiOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + c.OrgMaster.ChiOrgName : "",
                             EngChiOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + c.OrgMaster.ChiOrgName : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                             EngOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgName : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                             ChiOrgName = c.OrgMaster != null ? c.OrgMaster.ChiOrgName : "",
                             SubventedIndicator = c.OrgMaster != null ? c.OrgMaster.SubventedIndicator : false,
                             DisableIndicator = c.OrgMaster != null ? c.OrgMaster.DisableIndicator : false,
                             RegistrationType1 = c.OrgMaster != null ? c.OrgMaster.RegistrationType1 : "",
                             RegistrationType2 = c.OrgMaster != null ? c.OrgMaster.RegistrationType2 : "",
                             RegistrationOtherName1 = c.OrgMaster != null ? c.OrgMaster.RegistrationOtherName1 : "",
                             RegistrationOtherName2 = c.OrgMaster != null ? c.OrgMaster.RegistrationOtherName2 : "",
                             ComplaintRecordType = c.ComplaintRecordType,
                             ComplaintRef = c.ComplaintRef,
                             ComplaintSource = c.ComplaintSource,
                             ActivityConcern = c.ActivityConcern,
                             OtherActivityConcern = c.OtherActivityConcern,
                             ComplaintDate = c.ComplaintDate,
                             FirstComplaintDate = c.FirstComplaintDate,
                             NonComplianceNature = c.NonComplianceNature,
                             FundRaisingLocation = c.FundRaisingLocation,
                             ProcessStatus = c.ProcessStatus,
                             TelRecordNum = TelRecordNum,
                             FollowUpActionRecordNum = FollowUpActionRecordNum,
                             PoliceCaseNum = PoliceCaseNum,
                             OtherDepartmentEnquiryNum = OtherDepartmentEnquiryNum,
                             ComplaintResult = c.ComplaintResult,
                             WithholdingRemark = c.WithholdingRemark,
                         }).AsQueryable();
            }

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintMasterSearchDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintMasterSearchDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public ComplaintMaster GetByComplaintRef(string complaintRef)
        {
            if (this.Table.Where(x => x.ComplaintRef == complaintRef).Count() == 1)
            {
                return this.Table.Where(x => x.ComplaintRef == complaintRef).First();
            }
            else
            { return null; }
        }
    }
}
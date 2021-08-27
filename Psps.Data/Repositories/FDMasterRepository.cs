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
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.FdStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IFDMasterRepository : IRepository<FdMaster, int>
    {
        FdMaster GetFdMasterById(int FdMasterId);

        List<FdStatusDto> GetFdStatusSummary(int currentYear);

        IPagedList<FlagDaySearchDto> GetPageByFlagDaySearchDto(GridSettings grid);

        IPagedList<FdMaster> GetPage(GridSettings grid, string permitNum);

        string GetMaxSeq(string fdYear);

        IList<FlagDaySearchDto> GetFdList(GridSettings grid);

        List<FdEventApproveSummaryDto> GetApproveEveSummaryPage();

        //IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear);

        bool ifOrgMasterHasFdevent(string orgRef, string flagYear);

        string genPermitNo(string fdYear, string TWR);

        FdMaster GetFdMasterByFdYearAndOrg(string FdYear, OrgMaster OrgMaster);
    }

    public class FDMasterRepository : BaseRepository<FdMaster, int>, IFDMasterRepository
    {
        private IFdApprovalHistoryRepository _fdApprovalHistoryRepository;

        private ILookupRepository _lookupRepository;

        public FDMasterRepository(ISession session, IFdApprovalHistoryRepository fdApprovalHistoryRepository, ILookupRepository lookupRepository)
            : base(session)
        {
            _fdApprovalHistoryRepository = fdApprovalHistoryRepository;
            _lookupRepository = lookupRepository;
        }

        public FdMaster GetFdMasterById(int FdMasterId)
        {
            return this.Table.Where(x => x.FdMasterId == FdMasterId).FirstOrDefault();
        }

        public IPagedList<FdMaster> GetPage(GridSettings grid, string permitNum)
        {
            var query = this.Table;

            //filtring
            if (!String.IsNullOrEmpty(permitNum))
            {
                query = query.Where(x => x.FdEvent.Any(y => SqlMethods.Like(y.PermitNum, "%" + permitNum + "%") && y.IsDeleted == false));
            }

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public override IPagedList<FdMaster> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public string genPermitNo(string fdYear, string TWR)
        {
            if (string.IsNullOrEmpty(fdYear) || string.IsNullOrEmpty(TWR))
            {
                return "";
            }

            var query = from u in this.Table.Where(x => x.FdYear == fdYear)
                        from z in u.FdEvent.Where(x => x.TWR == TWR && x.PermitNum != null && x.PermitNum != "")
                        select new
                        {
                            seqNo = z.PermitNum.Substring(1, 3)
                        };

            if (query.ToList().Count > 0)
            {
                var maxSeq = query.Max(u => u.seqNo);

                int intMaxSeq = Convert.ToInt32(maxSeq) + 1;

                string seqLeadZero = intMaxSeq.ToString();

                while (seqLeadZero.Length < 3)
                {
                    seqLeadZero = "0" + seqLeadZero;
                }

                seqLeadZero = TWR == "1" ? "T" + seqLeadZero : "R" + seqLeadZero;
                return seqLeadZero;
            }
            else
            {
                var seqLeadZero = TWR == "1" ? "T001" : "R001";
                return seqLeadZero;
            }
        }

        public IPagedList<FlagDaySearchDto> GetPageByFlagDaySearchDto(GridSettings grid)
        {
            var query = from u in this.Table
                        //from ah in u.FdApprovalHistory.DefaultIfEmpty()
                        from eve in u.FdEvent.DefaultIfEmpty()
                        select new FlagDaySearchDto
                        {
                            OrgRef = u.OrgMaster.OrgRef,
                            OrgName = u.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
                            EngOrgNameSorting = u.OrgMaster.EngOrgNameSorting,
                            EngOrgName = u.OrgMaster.EngOrgName,
                            ChiOrgName = u.OrgMaster.ChiOrgName,
                            SubventedIndicator = u.OrgMaster.SubventedIndicator,
                            FdRef = u.FdRef,
                            PermitNum = eve.PermitNum,
                            WithPermit = (eve.PermitNum != null && eve.PermitNum.Length != 0) ? 1 : 0,
                            FlagDay = eve.FlagDay,
                            ApplyForTWR = u.ApplyForTwr,
                            TWR = eve.TWR,
                            NewApplicantIndicator = u.NewApplicantIndicator,
                            ApplicationResult = u.ApplicationResult,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.OrgMaster.DisableIndicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            TwrDistrict = eve.TwrDistrict,
                            EngMailingAddress1 = u.OrgMaster.EngMailingAddress1,
                            EngMailingAddress2 = u.OrgMaster.EngMailingAddress2,
                            EngMailingAddress3 = u.OrgMaster.EngMailingAddress3,
                            EngMailingAddress4 = u.OrgMaster.EngMailingAddress4,
                            EngMailingAddress5 = u.OrgMaster.EngMailingAddress5,
                            ChiMailingAddress1 = u.OrgMaster.ChiMailingAddress1,
                            ChiMailingAddress2 = u.OrgMaster.ChiMailingAddress2,
                            ChiMailingAddress3 = u.OrgMaster.ChiMailingAddress3,
                            ChiMailingAddress4 = u.OrgMaster.ChiMailingAddress4,
                            ChiMailingAddress5 = u.OrgMaster.ChiMailingAddress5,
                            ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                            EmailAddress = u.OrgMaster.EmailAddress,
                            ContactPersonSalute = u.ContactPersonSalute,
                            ContactPersonFirstName = u.ContactPersonFirstName,
                            ContactPersonLastName = u.ContactPersonLastName,
                            FdYear = u.FdYear,
                            FdMasterId = u.FdMasterId,
                            UsedLanguage = u.UsedLanguage,
                            FdGroup = u.FdGroup,
                            FdLotResult = u.FdLotResult,
                            ApplyPledgingMechanismIndicator = u.ApplyPledgingMechanismIndicator,
                            PermitRevokeIndicator = eve.PermitRevokeIndicator,
                            ApplicationResultInLastYear = u.FdMasterComputeView.ApplicationResultInLastYear,
                            LotGroupInLastYear = u.FdMasterComputeView.LotGroupInLastYear,
                            RefLotGroup = u.FdMasterComputeView.RefLotGroup
                        };

            //var query = (from u in this.Table
            //             //from ah in u.FdApprovalHistory.DefaultIfEmpty()
            //             from eve in u.FdEvent.DefaultIfEmpty()
            //             select new
            //             {
            //                 OrgRef = u.OrgMaster.OrgRef,
            //                 OrgName = u.OrgMaster.EngOrgName + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
            //                 EngOrgName = u.OrgMaster.EngOrgName,
            //                 ChiOrgName = u.OrgMaster.ChiOrgName,
            //                 SubventedIndicator = u.OrgMaster.SubventedIndicator,
            //                 FdRef = u.FdRef,
            //                 PermitNum = eve.PermitNum,
            //                 FlagDay = eve.FlagDay,
            //                 TWR = eve.TWR,
            //                 NewApplicantIndicator = u.NewApplicantIndicator,
            //                 ApplicationResult = u.ApplicationResult,
            //                 ApplicationReceiveDate = u.ApplicationReceiveDate,
            //                 DisableIndicator = u.OrgMaster.DisableIndicator,
            //                 ContactPersonName = u.ContactPersonName,
            //                 ContactPersonChiName = u.ContactPersonChiName,
            //                 TwrDistrict = eve.TwrDistrict,
            //                 FdMasterId = u.FdMasterId
            //             }).AsQueryable();

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FlagDaySearchDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FlagDaySearchDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public List<FdStatusDto> GetFdStatusSummary(int currentYear)
        {
            string currYear = currentYear.ToString().Substring(2, 2);
            string nextYear = (currentYear + 1).ToString().Substring(2, 2);
            string concatCurNext = currYear + "/" + nextYear;

            string sql = string.Format(
                //" select 'FD' as [Type] ,A.FdYear as [Year], isnull(B.DataInputStage,0) as [DataInputStage],  " +
                //" A.ReadyForApprove, A.Approved, A.MultiBatchApp   " +
                //" from   " +
                //   " ( " +
                //" select m.FdYear, " +
                //" sum( case when m.FdYear is not null and h.fdyear is null then 1 else 0 end)  ReadyForApprove,  	 " +
                //" sum( case when m.FdYear is not null and h.fdyear is not null then 1 else 0 end)  Approved,  	 " +
                //" '-' MultiBatchApp   " +
                //" from FdEvent e " +
                //" inner join  " +
                //" FdMaster m " +
                //" on e.FdMasterId = m.FdMasterId and m.IsDeleted = 0 " +
                //" left join " +
                //" FdApprovalHistory h " +
                //" on m.FdYear = h.FdYear " +
                //" group by m.FdYear  " +
                //" ) " +
                //" as A " +
                //" left join   " +
                //" (  	select FdYear, count(FdmasterId) as DataInputStage   	from FdMaster " +
                //" where FdmasterId not in (select FdmasterId from FdEvent)   	group by FdYear  ) as B  " +
                //" on A.FdYear = B.FdYear     " +
                //" union     " +
                                     " select 'PSP' as [Type] ,A.PspYear as [Year], isnull(B.DataInputStage,0) as [DataInputStage], A.ReadyForApprove, A.Approved, A.MultiBatchApp  " +
                                     " from       " +
                                     " (  	select PspYear,   	 " +
                                     " sum( case when a.ApprovalStatus='RA' then 1 else 0 end)  ReadyForApprove,  	 " +
                                     " sum( case when a.ApprovalStatus='AP' then 1 else 0 end)  Approved,  	 " +
                                     " sum( case when a.TwoBatchApproachIndicator=1 then 1 else 0 end) MultiBatchApp    	 " +
                                     " from PspMaster m  	 " +
                                     " Left join   	 " +
                                     " PspApprovalHistory a  	 " +
                                     " on m.PspmasterId = a.PspmasterId and m.IsDeleted = 0 " +
                                     " group by PspYear  ) as A   " +
                                     " left join   " +
                                     " (  	 " +
                                     " select PspYear, count(PspmasterId) as DataInputStage   	 " +
                                     " from PspMaster   	 " +
                                     " where PspmasterId not in (select PspmasterId from PspApprovalHistory)   	group by PspYear  ) as B   " +
                                     " on A.PspYear = B.PspYear  ");
            //" where A.PspYear = '{0}' ", concatCurNext);

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<FdStatusDto>();

            for (var i = 0; i < list.Count; i++)
            {
                var fdStatusDto = new FdStatusDto();
                object[] obj = list[i];
                var FunctionId = obj[0];
                fdStatusDto.Type = Convert.ToString(obj[0]);
                fdStatusDto.Year = Convert.ToString(obj[1]);
                fdStatusDto.DataInputStage = Convert.ToInt32(obj[2]);
                fdStatusDto.ReadyForApproval = Convert.ToInt32(obj[3]);
                fdStatusDto.Approved = Convert.ToInt32(obj[4]);
                fdStatusDto.MultiBatchApp = Convert.ToString(obj[5]);

                resultList.Add(fdStatusDto);
            }

            return resultList;
        }

        public bool ifOrgMasterHasFdevent(string orgRef, string flagYear)
        {
            string sql = string.Format(
                                     " select B.OrgRef, count(*)   " +
                                    " from fdmaster A   " +
                                    " inner join orgmaster B   " +
                                    " on A.OrgId = B.OrgId and B.OrgRef ='{0}' and A.fdyear ='{1}'" +
                                    " group by B.OrgRef  ", orgRef, flagYear);

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();

            var count = list.Count();

            if (count > 0)
                return false; // false means validation fail.
            else
                return true;
        }

        public string GetMaxSeq(string year)
        {
            var currYr = DateTime.Now.Year.ToString();
            var yearOfFlagDay = "20" + year.Substring(0, 2);
            var query = from u in this.Table
                         .Where(x => x.FdRef.IndexOf(yearOfFlagDay) > 0)
                        select new
                        {
                            midFdRef = u.FdRef.Substring(0, 3)
                        };

            string midFdRef = query.Max(x => x.midFdRef);
            if (String.IsNullOrEmpty(midFdRef))
            {
                return "001" + "(" + yearOfFlagDay + "/" + (Convert.ToInt32(yearOfFlagDay) + 1).ToString().Substring(2, 2) + ")";
            }
            else
            {
                var maxSeq = Convert.ToInt32(midFdRef) + 1;
                string fdRefLeadZero = maxSeq.ToString();
                while (fdRefLeadZero.Length < 3)
                {
                    fdRefLeadZero = "0" + fdRefLeadZero;
                }

                return fdRefLeadZero + "(" + yearOfFlagDay + "/" + (Convert.ToInt32(yearOfFlagDay) + 1).ToString().Substring(2, 2) + ")";
            }
        }

        public IList<FlagDaySearchDto> GetFdList(GridSettings grid)
        {
            var query = from u in this.Table
                        from eve in u.FdEvent.DefaultIfEmpty()
                        select new FlagDaySearchDto
                        {
                            OrgRef = u.OrgMaster.OrgRef,
                            OrgName = u.OrgMaster.EngOrgName + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
                            EngOrgName = u.OrgMaster.EngOrgName,
                            ChiOrgName = u.OrgMaster.ChiOrgName,
                            SubventedIndicator = u.OrgMaster.SubventedIndicator,
                            FdRef = u.FdRef,
                            PermitNum = eve.PermitNum,
                            FlagDay = eve.FlagDay,
                            TWR = eve.TWR,
                            NewApplicantIndicator = u.NewApplicantIndicator,
                            ApplicationResult = u.ApplicationResult,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.OrgMaster.DisableIndicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            TwrDistrict = eve.TwrDistrict,
                            FdMasterId = u.FdMasterId,
                            FdYear = u.FdYear,
                            PermitRevokeIndicator = eve.PermitRevokeIndicator
                        };

            //var innerQuery = from u in query select u.FdMasterId;

            //var query2 = from u in this.Table
            //             where innerQuery.Contains(u.FdMasterId)
            //             select u;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FlagDaySearchDto>(grid.SortColumn, grid.SortOrder);

            IList<FlagDaySearchDto> data = query.ToList();

            //var page = new PagedList<FdMaster>(query, grid.PageIndex, grid.PageSize);

            return data;
        }

        public List<FdEventApproveSummaryDto> GetApproveEveSummaryPage()
        {
            string sql = "SELECT m.FdYear, " + Environment.NewLine +
                         "       count(*) as appRece, sum(case when eve.FdEventId is not null then 1 else 0 end) as appAppro, " + Environment.NewLine +
                         "       sum(case when m.WithholdingListIndicator = 1 then 1 else 0 end) as appWithdraw, " + Environment.NewLine +
                         "       ah.ApproverPostId, ah.ApproverUserId, ah.ApprovalDate, ah.ApprovalRemark, case when isnull(ah.FdYear, '') = '' then 0 else 1 end Approved " + Environment.NewLine +
                         "FROM fdmaster m " + Environment.NewLine +
                         "LEFT JOIN fdevent eve on m.FdMasterId = eve.FdMasterId AND eve.IsDeleted = 0 " + Environment.NewLine +
                         "LEFT JOIN FdApprovalHistory ah on m.FdYear = ah.FdYear AND ah.IsDeleted = 0  " + Environment.NewLine +
                         "WHERE m.IsDeleted = 0 " + Environment.NewLine +
                         "GROUP BY m.FdYear, ah.FdYear, ah.ApproverPostId, ah.ApproverUserId, ah.ApprovalDate, ah.ApprovalRemark";

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<FdEventApproveSummaryDto>();

            DateTime? nullDate = (DateTime?)null;

            for (var i = 0; i < list.Count; i++)
            {
                var fdEventApproveSummaryDto = new FdEventApproveSummaryDto();
                object[] obj = list[i];

                fdEventApproveSummaryDto.YearOfFlagDay = Convert.ToString(obj[0]);
                fdEventApproveSummaryDto.ApplicationReceivedNum = Convert.ToInt32(obj[1]);
                fdEventApproveSummaryDto.ApplicationApprovedNum = Convert.ToInt32(obj[2]);
                fdEventApproveSummaryDto.ApplicationWithdrawNum = Convert.ToInt32(obj[3]);
                fdEventApproveSummaryDto.PostOfApprover = Convert.ToString(obj[4]);
                fdEventApproveSummaryDto.ApproverId = Convert.ToString(obj[5]);
                fdEventApproveSummaryDto.ApprovalDate = obj[6] == null ? nullDate : Convert.ToDateTime(obj[6]);
                fdEventApproveSummaryDto.SummaryRemarks = Convert.ToString(obj[7]);
                fdEventApproveSummaryDto.Approved = Convert.ToBoolean(obj[8]);

                resultList.Add(fdEventApproveSummaryDto);
            }

            return resultList;
        }

        public FdMaster GetFdMasterByFdYearAndOrg(string FdYear, OrgMaster OrgMaster)
        {
            return this.Table.Where(x => x.FdYear == FdYear && x.OrgMaster == OrgMaster).FirstOrDefault();
        }

        //public IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear)
        //{
        //    var query = (from u in this.Table.Where(x => x.FdYear == fdYear)
        //                 from eve in u.FdEvent.DefaultIfEmpty()
        //                 select new FdApplicationListDto
        //                {
        //                    FdRef = u.FdRef,
        //                    OrgName = u.OrgMaster.EngOrgName,
        //                    ApplicationResult = u.ApplicationResult,
        //                    FlagDay = eve.FlagDay,
        //                    TWR = eve.TWR,
        //                    PermitNo = eve.PermitNum,
        //                    ApproveRemarks = eve.Remarks,
        //                    TwrDistrict = eve.TwrDistrict
        //                }).AsQueryable();

        //    //filtring
        //    if (grid.IsSearch)
        //        query = query.Where(grid.Where);

        //    //sorting
        //    if (!string.IsNullOrEmpty(grid.SortColumn))
        //        query = query.OrderBy<FdApplicationListDto>(grid.SortColumn, grid.SortOrder);

        //    var page = new PagedList<FdApplicationListDto>(query, grid.PageIndex, grid.PageSize);

        //    return page;
        //}
    }
}
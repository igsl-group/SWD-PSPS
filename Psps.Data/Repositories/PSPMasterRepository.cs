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
    public interface IPSPMasterRepository : IRepository<PspMaster, int>
    {
        PspMaster GetPspMasterById(int PspMasterId);

        List<PspMasterDto> GetPspBringUpSummary();

        IPagedList<PspMaster> GetPage(GridSettings grid, string permitNum);

        IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid);

        IPagedList<PspApproveEventDto> GetRecommendPspEvents(GridSettings grid);

        string GetMaxSeq(string yearOfPsp);
    }

    public class PSPMasterRepository : BaseRepository<PspMaster, int>, IPSPMasterRepository
    {
        protected IPspCountEventsRepository _pspCountEventsRepository;
        protected IPspApprovalHistoryRepository _pspApprovalHistoryRepository;

        public PSPMasterRepository(ISession session, IPspCountEventsRepository pspCountEventsRepository, IPspApprovalHistoryRepository pspApprovalHistoryRepository)
            : base(session)
        {
            _pspCountEventsRepository = pspCountEventsRepository;
            _pspApprovalHistoryRepository = pspApprovalHistoryRepository;
        }

        public PspMaster GetPspMasterById(int PspMasterId)
        {
            return this.Table.Where(x => x.PspMasterId == PspMasterId).FirstOrDefault();
        }

        public string GetMaxSeq(string yearOfPsp)
        {
            var currYr = DateTime.Now.Year.ToString();
            var query = from u in this.Table
                         .Where(x => x.PspRef.IndexOf(yearOfPsp) > 0)
                        select new
                        {
                            pspRef = Convert.ToInt32(u.PspRef.Substring(0, 3))
                        };

            var pspRef = int.MinValue;

            if (query.ToList().Count != 0)
            {
                pspRef = query.Max(x => x.pspRef);
            }
            else
                pspRef = int.MinValue;
            //var maxSeq = 0;

            if (pspRef == int.MinValue)
            {
                return "001(" + yearOfPsp + ")";
            }
            else
            {
                //Match match = Regex.Match(pspRef, @"(?<=\().+?(?=\))");
                //maxSeq = Convert.ToInt32(match.Groups[0].Value) + 1;
                string pspRefLeadZero = Convert.ToString(pspRef + 1);
                while (pspRefLeadZero.Length < 3)
                {
                    pspRefLeadZero = "0" + pspRefLeadZero;
                }
                return pspRefLeadZero + "(" + yearOfPsp + ")";
            }
        }

        public override IPagedList<PspMaster> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<PspMaster> GetPage(GridSettings grid, string permitNum)
        {
            var query = this.Table;

            //filtring
            if (!String.IsNullOrEmpty(permitNum))
            {
                query = query.Where(x => x.PspApprovalHistory.Any(y => SqlMethods.Like(y.PermitNum, "%" + permitNum + "%")));
            }

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public List<PspMasterDto> GetPspBringUpSummary()
        {
            //" GetDate() as PspEventDate" should be fix when the event date field of Pspevent is established
            //" 0 as AmenPspNo" should be fix when relevant field is established
            //" 0 as PrevPspNo" should be fix when relevant field is established
            string sql = " select A.Pspref , B.EngOrgName, A.ApplicationReceiveDate, EventStartDate as PspEventDate, A.ProcessingOfficerPost, a.SpecialRemark     " +
                         " from      " +
                         "     pspmaster A     " +
                         "         inner join      " +
                         "     OrgMaster B     " +
                         "         on A.OrgId = B.OrgId     " +
                         "         inner join     " +
                         "     (Select  PspEventId, PspMasterId,min(EventStartDate) as EventStartDate from PspEvent  group by PspMasterId, PspEventId ) as C     " +

                         "         on A.PspMasterId  = C.PspMasterId    " +
                          " AND C.EventStartDate >= GETDATE() AND C.EventStartDate <= DATEADD(DAY,10,GETDATE() ) " +
                         "   	inner join   " +
                         " PspApprovalHistory D   " +
                         "   	on A.PspMasterId = D.PspMasterId and D.ApprovalStatus = 'RA' ";

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<PspMasterDto>();

            for (var i = 0; i < list.Count; i++)
            {
                var pspMasterDto = new PspMasterDto();
                object[] obj = list[i];
                var FunctionId = obj[0];
                pspMasterDto.PspRef = Convert.ToString(obj[0]);
                pspMasterDto.EngOrgName = Convert.ToString(obj[1]);
                pspMasterDto.ApplicationReceiveDate = Convert.ToDateTime(obj[2]);
                pspMasterDto.PspEventDate = Convert.ToDateTime(obj[3]);
                pspMasterDto.ProcessingOfficerPost = Convert.ToString(obj[4]);
                pspMasterDto.SpecialRemark = Convert.ToString(obj[5]);

                resultList.Add(pspMasterDto);
            }

            return resultList;
        }

        public IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid)
        {
            var query = from u in this.Table
                        from pah in u.PspApprovalHistory.DefaultIfEmpty()
                        join eve in _pspCountEventsRepository.Table
                        on u.PspMasterId equals eve.PspMasterId
                        select new PspSearchDto
                        {
                            PspMasterId = u.PspMasterId,
                            OrgRef = u.OrgMaster.OrgRef,
                            OrgName = u.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + u.OrgMaster.ChiOrgName,                            
                            EngOrgName = u.OrgMaster.EngOrgName,
                            ChiOrgName = u.OrgMaster.ChiOrgName,
                            SubventedIndicator = u.OrgMaster.SubventedIndicator,
                            //AddressProofIndicator = u.OrgMaster.AddressProofIndicator,
                            PspRef = u.PspRef,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.OrgMaster.DisableIndicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            Section88Indicator = u.OrgMaster.Section88Indicator,
                            RegType1 = u.OrgMaster.RegistrationType1,
                            RegOtherName1 = u.OrgMaster.RegistrationOtherName1,
                            RegType2 = u.OrgMaster.RegistrationType2,
                            RegOtherName2 = u.OrgMaster.RegistrationOtherName2,
                            PermitNum = pah.PermitNum,
                            //AnnualReportIndicator = u.OrgMaster.AnnualReport1Indicator == false && u.OrgMaster.AnnualReport2Indicator == false && u.OrgMaster.AnnualReport3Indicator == false ? false : true,
                            //AfsIndicator = u.OrgMaster.Afs1Indicator == false && u.OrgMaster.Afs2Indicator == false && u.OrgMaster.Afs3Indicator == false ? false : true,
                            ApprovalStatus = pah.ApprovalStatus,
                            PspApprovalHistoryId = pah.PspApprovalHistoryId,
                            EventStartDate = eve.EventStartDate,
                            EventEndDate = eve.EventEndDate,
                            TotEvent = eve.TotEvents,
                            IsSsaf = u.IsSsaf
                        };

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspSearchDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspSearchDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<PspApproveEventDto> GetRecommendPspEvents(GridSettings grid)
        {
            var query = (from u in this.Table
                         from y in u.PspApprovalHistory.DefaultIfEmpty()
                         join z in _pspCountEventsRepository.Table
                         on u.PspMasterId equals z.PspMasterId
                         select new PspApproveEventDto
                         {
                             PspMasterId = u.PspMasterId,
                             EngOrgName = u.OrgMaster.EngOrgName,
                             PspRef = u.PspRef,
                             PspPermitNo = y.PermitNum,
                             EventStartDate = z.EventStartDate,
                             EventEndDate = z.EventEndDate,
                             EventStatus = y.ApprovalType,
                             TotalEventsToBeApprove = z.TotEvents.HasValue ? (int)z.TotEvents : 0
                         }).AsQueryable();

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspApproveEventDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspApproveEventDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        private string ConvertToDesc(string code)
        {
            if (code == "AP")
            {
                return "Approved";
            }
            else if (code == "RA")
            {
                return "Ready for Approve";
            }
            else if (code == "RJ")
            {
                return "Rejected";
            }
            else if (code == "CA")
            {
                return "Cancelled";
            }
            else
            {
                return "Ready for Cancel";
            }
        }
    }
}
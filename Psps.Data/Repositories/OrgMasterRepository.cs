using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IOrgMasterRepository : IRepository<OrgMaster, int>
    {
        string CreateOrgRef();

        IPagedList<OrgFlagDaySearchDto> getPageByOrgFlagDaySearchDto(GridSettings grid);

        OrgMaster GetOrgByRef(string orgRef);

        IPagedList<OrgMasterSearchView> getPageByOrgMasterSearchView(GridSettings grid);

        IPagedList<ComplaintPspMasterDto> getPageComplaintByPspMasterId(GridSettings grid, int pspMasterId);

        IPagedList<ComplaintPspMasterDto> getPageEnquiryByPspMasterId(GridSettings grid, int pspMasterId);

        IPagedList<ComplaintFdMasterDto> getPageComplaintByFdMasterId(GridSettings grid, int fdMasterId);

        IPagedList<ComplaintFdMasterDto> getPageEnquiryByFdMasterId(GridSettings grid, int fdMasterId);

        int getEnqRecCntByPspMasterId(int pspMasterId);

        int getCompRecCntByPspMasterId(int pspMasterId);

        int getEnqRecCntByFdMasterId(int fdMasterId);

        int getCompRecCntByFdMasterId(int fdMasterId);
    }

    public class OrgMasterRepository : BaseRepository<OrgMaster, int>, IOrgMasterRepository
    {
        private readonly IPspApprovalHistoryRepository _pspApprovalHistoryRepository;
        private readonly IFdEventRepository _fdEventRepository;
        private readonly IOrgEditComplaintViewRepository _orgEditComplaintViewRepository;
        private readonly IOrgEditEnquiryViewRepository _orgEditEnquiryViewRepository;
        private readonly IComplaintMasterRepository _complaintMasterRepository;

        public OrgMasterRepository(ISession session, IOrgEditComplaintViewRepository orgEditComplaintViewRepository, IOrgEditEnquiryViewRepository orgEditEnquiryViewRepository,
            IPspApprovalHistoryRepository pspApprovalHistoryRepository, IFdEventRepository fdEventRepository, IComplaintMasterRepository complaintMasterRepository)
            : base(session)
        {
            this._pspApprovalHistoryRepository = pspApprovalHistoryRepository;
            this._orgEditComplaintViewRepository = orgEditComplaintViewRepository;
            this._orgEditEnquiryViewRepository = orgEditEnquiryViewRepository;
            this._fdEventRepository = fdEventRepository;
            this._complaintMasterRepository = complaintMasterRepository;
        }

        public virtual string CreateOrgRef()
        {
            string OrgRefPrefix = "ORG";

            string RetStr = "0001";
            var hql = "select OrgRef from OrgMaster s ";

            hql = hql + "order by s.OrgId desc";
            IList<string> list = this.Session.CreateSQLQuery(hql).List<string>();
            int RetInt = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string listvalue = list[i].ToString();
                if (listvalue.Length > 4)
                {
                    listvalue = listvalue.Substring(listvalue.Length - 4, 4);
                    if (ConvertInt(listvalue) > RetInt)
                        RetInt = ConvertInt(listvalue);
                }
            }

            if (RetInt > 0)
            {
                int num = RetInt + 1;
                RetStr = num.ToString().PadLeft(4, '0');
            }
            RetStr = OrgRefPrefix + RetStr;
            return RetStr;
        }

        public int ConvertInt(string num)
        {
            int mValue = 0;
            try
            {
                mValue = Convert.ToInt32(num);
            }
            catch
            {
                mValue = 0;
            }
            return mValue;
        }

        public IPagedList<OrgFlagDaySearchDto> getPageByOrgFlagDaySearchDto(GridSettings grid)
        {
            var query = (from org in this.Table
                         from fd in org.FdMaster.DefaultIfEmpty()
                         //from fah in fd.FdApprovalHistory.DefaultIfEmpty()
                         from eve in fd.FdEvent.DefaultIfEmpty()
                         select new OrgFlagDaySearchDto
                         {
                             OrgRef = org.OrgRef,
                             OrgName = org.EngOrgName + System.Environment.NewLine + org.ChiOrgName,
                             OrgEngName = org.EngOrgName,
                             OrgChiName = org.ChiOrgName,
                             SubventedIndicator = org.SubventedIndicator,
                             FdRef = fd.FdRef,
                             PermitNum = eve.PermitNum,
                             FlagDay = eve.FlagDay,
                             TWR = eve.TWR,
                             NewApplicantIndicator = fd.NewApplicantIndicator,
                             ApplicationResult = fd.ApplicationResult
                         }).AsQueryable();

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<OrgFlagDaySearchDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<OrgFlagDaySearchDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public OrgMaster GetOrgByRef(string orgRef)
        {
            if (this.Table.Where(x => x.OrgRef == orgRef).Count() == 1)
            {
                return this.Table.Where(x => x.OrgRef == orgRef).First();
            }
            else
            { return null; }
        }

        public IPagedList<OrgMasterSearchView> getPageByOrgMasterSearchView(GridSettings grid)
        {
            var query = (from o in this.Table
                         let pspCount = o.PspMasters.Count
                         let fdCount = o.FdMaster.Count
                         select new OrgMasterSearchView
                         {
                             OrgId = o.OrgId,
                             OrgRef = o.OrgRef,
                             EngOrgName = o.EngOrgName,
                             ChiOrgName = o.ChiOrgName,
                             TelNum = o.TelNum,
                             EngRegisteredAddress1 = o.EngRegisteredAddress1,
                             EngRegisteredAddress2 = o.EngRegisteredAddress2,
                             EngRegisteredAddress3 = o.EngRegisteredAddress3,
                             EngRegisteredAddress4 = o.EngRegisteredAddress4,
                             EngRegisteredAddress5 = o.EngRegisteredAddress5,
                             ChiRegisteredAddress1 = o.ChiRegisteredAddress1,
                             ChiRegisteredAddress2 = o.ChiRegisteredAddress2,
                             ChiRegisteredAddress3 = o.ChiRegisteredAddress3,
                             ChiRegisteredAddress4 = o.ChiRegisteredAddress4,
                             ChiRegisteredAddress5 = o.ChiRegisteredAddress5,
                             DisableIndicator = o.DisableIndicator,
                             SubventedIndicator = o.SubventedIndicator,
                             ApplicantFirstName = o.ApplicantFirstName,
                             ApplicantLastName = o.ApplicantLastName,
                             ApplicantChiFirstName = o.ApplicantChiFirstName,
                             ApplicantChiLastName = o.ApplicantChiLastName,
                             OrgObjective = o.OrgObjective,
                             RegistrationType1 = o.RegistrationType1,
                             RegistrationOtherName1 = o.RegistrationOtherName1,
                             RegistrationType2 = o.RegistrationType2,
                             RegistrationOtherName2 = o.RegistrationOtherName2,
                             Section88Indicator = o.Section88Indicator,
                             AddressProofIndicator = o.AddressProofIndicator,
                             AppliedPSPBefore = pspCount > 0,
                             AppliedFDBefore = fdCount > 0,
                             ReceivedComplaintEnquiryBefore = o.ComplaintMaster.Count > 0
                         }).AsQueryable();
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<OrgMasterSearchView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<OrgMasterSearchView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<ComplaintPspMasterDto> getPageComplaintByPspMasterId(GridSettings grid, int pspMasterId)
        {
            var query =
            (from u in _orgEditComplaintViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join aph in _pspApprovalHistoryRepository.Table on comp.PspApprovalHistory.PspApprovalHistoryId equals aph.PspApprovalHistoryId
             where (aph.PspMaster.PspMasterId == pspMasterId && comp.ComplaintRecordType == "02")
             select new ComplaintPspMasterDto
             {
                 OrgId = u.OrgId,
                 ComplaintMasterId = u.ComplaintMasterId,
                 ComplaintRef = u.ComplaintRef,
                 ComplaintSource = u.ComplaintSource,
                 ActivityConcern = u.ActivityConcern,
                 ComplaintDate = u.ComplaintDate,
                 PermitNum = u.PermitNum,
                 ComplaintRemarks = u.ComplaintRemarks,
                 FollowUpLetterType = u.FollowUpLetterType,
                 FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                 LetterIssuedNum = u.LetterIssuedNum,
                 PspMasterId = aph.PspMaster.PspMasterId
             }).AsQueryable();

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintPspMasterDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintPspMasterDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<ComplaintPspMasterDto> getPageEnquiryByPspMasterId(GridSettings grid, int pspMasterId)
        {
            var query =
            (from u in _orgEditComplaintViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join aph in _pspApprovalHistoryRepository.Table on comp.PspApprovalHistory.PspApprovalHistoryId equals aph.PspApprovalHistoryId
             where (aph.PspMaster.PspMasterId == pspMasterId && comp.ComplaintRecordType == "01")
             select new ComplaintPspMasterDto
             {
                 OrgId = u.OrgId,
                 ComplaintMasterId = u.ComplaintMasterId,
                 ComplaintRef = u.ComplaintRef,
                 ComplaintSource = u.ComplaintSource,
                 ActivityConcern = u.ActivityConcern,
                 ComplaintDate = u.ComplaintDate,
                 PermitNum = u.PermitNum,
                 ComplaintRemarks = u.ComplaintRemarks,
                 FollowUpLetterType = u.FollowUpLetterType,
                 FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                 LetterIssuedNum = u.LetterIssuedNum,
                 PspMasterId = aph.PspMaster.PspMasterId
             }).AsQueryable();

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintPspMasterDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintPspMasterDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<ComplaintFdMasterDto> getPageComplaintByFdMasterId(GridSettings grid, int fdMasterId)
        {
            var query =
            (from u in _orgEditComplaintViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join fdEve in _fdEventRepository.Table on comp.FdEvent.FdEventId equals fdEve.FdEventId
             where (fdEve.FdMaster.FdMasterId == fdMasterId && comp.ComplaintRecordType == "02")
             select new ComplaintFdMasterDto
             {
                 OrgId = u.OrgId,
                 ComplaintMasterId = u.ComplaintMasterId,
                 ComplaintRef = u.ComplaintRef,
                 ComplaintSource = u.ComplaintSource,
                 ActivityConcern = u.ActivityConcern,
                 ComplaintDate = u.ComplaintDate,
                 PermitNum = u.PermitNum,
                 ComplaintRemarks = u.ComplaintRemarks,
                 FollowUpLetterType = u.FollowUpLetterType,
                 FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                 LetterIssuedNum = u.LetterIssuedNum,
                 FdMasterId = fdEve.FdMaster.FdMasterId
             }).AsQueryable();

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintFdMasterDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintFdMasterDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<ComplaintFdMasterDto> getPageEnquiryByFdMasterId(GridSettings grid, int fdMasterId)
        {
            var query =
            (from u in _orgEditComplaintViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join fdEve in _fdEventRepository.Table on comp.FdEvent.FdEventId equals fdEve.FdEventId
             where (fdEve.FdMaster.FdMasterId == fdMasterId && comp.ComplaintRecordType == "01")
             select new ComplaintFdMasterDto
             {
                 OrgId = u.OrgId,
                 ComplaintMasterId = u.ComplaintMasterId,
                 ComplaintRef = u.ComplaintRef,
                 ComplaintSource = u.ComplaintSource,
                 ActivityConcern = u.ActivityConcern,
                 ComplaintDate = u.ComplaintDate,
                 PermitNum = u.PermitNum,
                 ComplaintRemarks = u.ComplaintRemarks,
                 FollowUpLetterType = u.FollowUpLetterType,
                 FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                 LetterIssuedNum = u.LetterIssuedNum,
                 FdMasterId = fdEve.FdMaster.FdMasterId
             }).AsQueryable();

            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintFdMasterDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintFdMasterDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public int getEnqRecCntByPspMasterId(int pspMasterId)
        {
            var recCnt =
           (
            from u in _orgEditComplaintViewRepository.Table
            join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
            join aph in _pspApprovalHistoryRepository.Table on comp.PspApprovalHistory.PspApprovalHistoryId equals aph.PspApprovalHistoryId
            where (aph.PspMaster.PspMasterId == pspMasterId && comp.ComplaintRecordType == "01")
            select u
           ).ToList().Count();

            return recCnt;
        }

        public int getCompRecCntByPspMasterId(int pspMasterId)
        {
            var recCnt =
           (
            from u in _orgEditComplaintViewRepository.Table
            join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
            join aph in _pspApprovalHistoryRepository.Table on comp.PspApprovalHistory.PspApprovalHistoryId equals aph.PspApprovalHistoryId
            where (aph.PspMaster.PspMasterId == pspMasterId && comp.ComplaintRecordType == "02")
            select u
           ).ToList().Count();

            return recCnt;
        }

        public int getEnqRecCntByFdMasterId(int fdMasterId)
        {
            var recCnt =
            (from u in _orgEditEnquiryViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join fdEve in _fdEventRepository.Table on comp.FdEvent.FdEventId equals fdEve.FdEventId
             where (fdEve.FdMaster.FdMasterId == fdMasterId && comp.ComplaintRecordType == "01")
             select u
            ).ToList().Count();

            return recCnt;
        }

        public int getCompRecCntByFdMasterId(int fdMasterId)
        {
            var recCnt =
            (from u in _orgEditComplaintViewRepository.Table
             join comp in _complaintMasterRepository.Table on u.ComplaintMasterId equals comp.ComplaintMasterId
             join fdEve in _fdEventRepository.Table on comp.FdEvent.FdEventId equals fdEve.FdEventId
             where (fdEve.FdMaster.FdMasterId == fdMasterId && comp.ComplaintRecordType == "02")
             select u).ToList().Count();

            return recCnt;
        }
    }
}
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
    public interface IPspSearchViewRepository : IRepository<PspSearchView, int>
    {
        IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid);

        IList<PspSearchDto> GetPspList(GridSettings grid);
    }

    public class PspSearchViewRepository : BaseRepository<PspSearchView, int>, IPspSearchViewRepository
    {
        public PspSearchViewRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid)
        {
            var query = from u in this.Table
                        select new PspSearchDto
                        {
                            PspYear = u.SortPspRef.Substring(0, 4),
                            PspMasterId = u.PspMasterId,
                            OrgRef = u.OrgMaster.OrgRef,
                            OrgName = u.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
                            EngOrgNameSorting = u.OrgMaster.EngOrgNameSorting,
                            EngOrgName = u.OrgMaster.EngOrgName,
                            ChiOrgName = u.OrgMaster.ChiOrgName,
                            OrgValidTo_Month = u.OrgMaster.OrgValidTo_Month,
                            OrgValidTo_Year = u.OrgMaster.OrgValidTo_Year,
                            IVP = u.OrgMaster.IVP,
                            SubventedIndicator = u.OrgMaster.SubventedIndicator,
                            PspRef = u.PspRef,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.OrgMaster.DisableIndicator,
                            Section88Indicator = u.Section88Indicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            RegType1 = u.OrgMaster.RegistrationType1,
                            RegOtherName1 = u.OrgMaster.RegistrationOtherName1,
                            RegType2 = u.OrgMaster.RegistrationType2,
                            RegOtherName2 = u.OrgMaster.RegistrationOtherName2,
                            PermitNum = u.PermitNum,
                            ApprovalStatus = u.ApplicationResult,
                            RejectReason = u.RejectReason,
                            RejectRemark = u.RejectRemark,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            TotalLocation = u.TotalLocation,
                            TotEvent = u.TotEvent,
                            EventApprovedNum = u.EventApprovedNum,
                            EventHeldNum = u.EventHeldNum,
                            EventCancelledNum = u.EventCancelledNum,
                            EventHeldPercent = u.EventHeldPercent,
                            OverdueIndicator = u.OverdueIndicator,
                            LateIndicator = u.LateIndicator,
                            ApplicationDisposalDate = u.ApplicationDisposalDate,
                            ApplicationCompletionDate = u.ApplicationCompletionDate,
                            QualityOpinionDetail = u.QualityOpinionDetail,
                            ProcessingOfficerPost = u.ProcessingOfficerPost,
                            ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                            ContactPerson = u.ContactPerson,
                            EngRegisteredAddress1 = u.OrgMaster.EngRegisteredAddress1,
                            EngRegisteredAddress2 = u.OrgMaster.EngRegisteredAddress2,
                            ChiRegisteredAddress1 = u.OrgMaster.ChiRegisteredAddress1,
                            ChiRegisteredAddress2 = u.OrgMaster.ChiRegisteredAddress2,
                            OrgEmailAddress = u.OrgMaster.EmailAddress,
                            SortPspRef = u.SortPspRef,
                            PreviousPspMasterId = u.PreviousPspMasterId,
                            PreviousPspRef = u.PreviousPspRef,
                            NewApplicantIndicator = u.NewApplicantIndicator,
                            DisasterId = u.DisasterMasterId,
                            ReSubmit = u.ReSubmit,
                            ReEvents = u.ReEvents,
                            ApplicationDate = u.ApplicationDate,
                            BeneficiaryOrg = u.BeneficiaryOrg,
                            SpecialRemark = u.SpecialRemark,
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

        public IList<PspSearchDto> GetPspList(GridSettings grid)
        {
            var query = from u in this.Table
                        select new PspSearchDto
                        {
                            PspMasterId = u.PspMasterId,
                            OrgRef = u.OrgMaster.OrgRef,
                            OrgName = u.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
                            EngOrgName = u.OrgMaster.EngOrgName,
                            ChiOrgName = u.OrgMaster.ChiOrgName,
                            OrgValidTo_Month = u.OrgMaster.OrgValidTo_Month,
                            OrgValidTo_Year = u.OrgMaster.OrgValidTo_Year,
                            IVP = u.OrgMaster.IVP,
                            SubventedIndicator = u.OrgMaster.SubventedIndicator,
                            PspRef = u.PspRef,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.OrgMaster.DisableIndicator,
                            Section88Indicator = u.Section88Indicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            RegType1 = u.OrgMaster.RegistrationType1,
                            RegOtherName1 = u.OrgMaster.RegistrationOtherName1,
                            RegType2 = u.OrgMaster.RegistrationType2,
                            RegOtherName2 = u.OrgMaster.RegistrationOtherName2,
                            PermitNum = u.PermitNum,
                            ApprovalStatus = u.ApplicationResult,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            TotalLocation = u.TotalLocation,
                            TotEvent = u.TotEvent,
                            EventApprovedNum = u.EventApprovedNum,
                            EventHeldNum = u.EventHeldNum,
                            EventCancelledNum = u.EventCancelledNum,
                            EventHeldPercent = u.EventHeldPercent,
                            OverdueIndicator = u.OverdueIndicator,
                            LateIndicator = u.LateIndicator,
                            ApplicationDisposalDate = u.ApplicationDisposalDate,
                            QualityOpinionDetail = u.QualityOpinionDetail,
                            ProcessingOfficerPost = u.ProcessingOfficerPost,
                            ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                            ContactPerson = u.ContactPerson,
                            EngRegisteredAddress1 = u.OrgMaster.EngRegisteredAddress1,
                            EngRegisteredAddress2 = u.OrgMaster.EngRegisteredAddress2,
                            ChiRegisteredAddress1 = u.OrgMaster.ChiRegisteredAddress1,
                            ChiRegisteredAddress2 = u.OrgMaster.ChiRegisteredAddress2,
                            OrgEmailAddress = u.OrgMaster.EmailAddress,
                            SortPspRef = u.SortPspRef
                        };

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspSearchDto>(grid.SortColumn, grid.SortOrder);

            IList<PspSearchDto> data = query.ToList();

            return data;
        }
    }
}
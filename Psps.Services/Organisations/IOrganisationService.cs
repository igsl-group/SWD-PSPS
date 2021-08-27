using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Organisation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    /// <summary>
    /// System message service interface
    /// </summary>
    public partial interface IOrganisationService
    {
        /// <summary>
        /// Get system message by Code
        /// </summary>
        /// <param name="systemMessageCode">System message Code</param>
        /// <returns>System message entity</returns>
        OrgMaster GetOrgById(int Id);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<OrgMaster> GetPage(GridSettings grid);

        /// <summary>
        /// Update a OrgMaster
        /// </summary>
        /// <param name="model">OrgMaster</param>
        void UpdateOrgMaster(OrgMaster model);

        /// <summary>
        /// Updates the OrgMaster
        /// </summary>
        /// <param name="oldOrgMaster">The old OrgMaster</param>
        /// <param name="newOrgMaster">The new OrgMaster</param>
        void UpdateOrgMaster(OrgMaster oldOrgMaster, OrgMaster newOrgMaster);

        /// <summary>
        /// Create a OrgMaster
        /// </summary>
        /// <param name="model">OrgMaster</param>
        void CreateOrgMaster(OrgMaster model);

        /// <summary>
        /// Create OrgRef
        /// </summary>
        /// <returns>string</returns>
        string CreateOrgRef();

        /// <summary>
        /// get page by OrgFlagDaySearchDto
        /// </summary>
        /// <returns>page</returns>
        IPagedList<OrgFlagDaySearchDto> getPageByOrgFlagDaySearchDto(GridSettings grid);

        /// <summary>
        /// get Org
        /// </summary>
        /// <returns>OrgMaster</returns>
        OrgMaster GetOrgByRef(string orgRef);

        /// <summary>
        /// get page by OrgEditPspView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<OrgEditPspView> getPageByOrgEditPspView(GridSettings grid);

        /// <summary>
        /// Get OrgEditPspView Amount By OrgId
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetOrgEditPspViewAmountByOrgId(string OrgId, bool IsSSAF = false);

        /// <summary>
        /// get page by OrgEditEnquiryView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<OrgEditEnquiryView> getPageByOrgEditEnquiryView(GridSettings grid);

        /// <summary>
        /// get page by OrgEditComplaintView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<OrgEditComplaintView> getPageByOrgEditComplaintView(GridSettings grid);

        /// <summary>
        /// Get OrgEditComplaintView Amount By OrgId
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetOrgEditComplaintViewAmountByOrgId(string OrgId);

        /// <summary>
        /// Get OrgEditComplaintView Amount By OrgId
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetOrgEditEnquiryViewAmountByOrgId(string OrgId);

        /// <summary>
        /// Get ReferenceGuideSearchView Amount by OrgId
        /// </summary>
        /// <param name="OrgId">Organisation Master Id</param>
        /// <returns>amount</returns>
        int GetOrgEditReferenceGuideViewAmountByOrgId(string OrgId);

        /// <summary>
        /// get page by OrgMasterSearchView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<OrgMasterSearchView> GetPageByOrgMasterSearchView(GridSettings grid, String withHoldInd, String receivedComplaintBefore, String receivedEnquiryBefore,
                                                                     String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate,
                                                                     String appliedFdBefore, IList<string> appliedFDBeforeFdYears,
                                                                     String appliedSSAFBefore, DateTime? fromSSAFAppRecDate, DateTime? toSSAFAppRecDate,
                                                                     String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate,
                                                                     String fdIssuedBefore, IList<string> fdIssuedBeforeFdYears,
                                                                     String ssafIssuedBfore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="orgMaster">OrgMaster</param>
        /// <returns>int</returns>
        void Delete(OrgMaster orgMaster);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        IPagedList<ComplaintPspMasterDto> getPageComplaintByPspMasterId(GridSettings grid, int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        IPagedList<ComplaintPspMasterDto> getPageEnquiryByPspMasterId(GridSettings grid, int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        IPagedList<ComplaintFdMasterDto> getPageComplaintByFdMasterId(GridSettings grid, int fdMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        IPagedList<ComplaintFdMasterDto> getPageEnquiryByFdMasterId(GridSettings grid, int fdMasterId);

        int GetLeastOrgMaster();

        /// <summary>
        /// get page by ReferenceGuideSearchView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid);

        /// <summary>
        /// get page by ReferenceGuideSearchView
        /// </summary>
        /// <returns>page</returns>
        IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid, 
            String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate, 
            List<string> appliedFDBeforeFdYears, IList<string> fdIssuedBeforeFdYears, string appliedFDBeforeId, string fdIssuedBeforeId,
            String appliedSSAFBefore, DateTime? fromSSAFApplicationDate, DateTime? toSSAFApplicationDate,
            String ssafIssuedBefore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate,
            String referenceGuideActivityConcern);

        bool IsExistedOrgName(string engOrgName, string chiOrgName);

        string ImportRefGuideXlsFile(Stream ms);
    }
}
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial interface IFlagDayService
    { /// <summary>
        /// Get system message by Id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        FdMaster GetFDById(int Id);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<FdMaster> GetPage(GridSettings grid);

        /// <summary>
        /// List FdMaster
        /// </summary>
        /// <param name="grid,permitNum">jqGrid parameters,string</param>
        /// <returns>Messages</returns>
        IPagedList<FdMaster> GetPage(GridSettings grid, string permitNum);

        /// <summary>
        /// Update a FdMaster
        /// </summary>
        /// <param name="model">FdMaster</param>
        void UpdateFdMaster(FdMaster model);

        /// <summary>
        /// Updates the FdMaster
        /// </summary>
        /// <param name="oldFDMaster">The old FdMaster</param>
        /// <param name="newFDMaster">The new FdMaster</param>
        void UpdateFdMaster(FdMaster oldFDMaster, FdMaster newFDMaster);

        /// <summary>
        /// Create a FdMaster
        /// </summary>
        /// <param name="model">FdMaster</param>
        void CreateFdMaster(FdMaster model);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        IPagedList<FlagDaySearchDto> GetPageByFlagDaySearchDto(GridSettings grid);

        /// <summary>
        /// Get FdTabGridView Amount By OrgId
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetFdTabGridViewAmountByOrgId(string OrgId);

        IPagedList<OrgFdTabGridView> GetPageByOrgFdTabGridView(GridSettings grid);

        /// <summary>
        /// get max sequence number from fdref. numbers within the bracket
        /// </summary>
        /// <param name="fdYear"></param>
        /// <returns></returns>
        string GetMaxSeq(string fdYear);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        MemoryStream GetPageToXls(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        MemoryStream InserFlagDayByImportXls(Stream xlsxStream, out int warning);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        List<FdEventApproveSummaryDto> GetApproveEveSummaryPage();

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdYear"></param>
        /// <returns></returns>
        IList<FdApplicationListDto> GetFdApplicationList(string fdYear);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fdYear"></param>
        /// <returns></returns>
        IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        Hashtable CalFdEditRecCnt(int fdMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdYear"></param>
        /// <param name="district"></param>
        /// <returns></returns>
        string genPermitNo(string fdYear, string TWR);

        string switchToPreviousFdYear(string fdYear);

        FdMaster GetFdMasterByFdYearAndOrg(string FdYear, OrgMaster OrgMaster);

        String GetFdBenchmarkStatusByFdYearAndOrg(string FdYear, OrgMaster OrgMaster,
                                                  IDictionary<string, string> BenchmarkTWFD,
                                                  IDictionary<string, string> BenchmarkRFD);

        /// <summary>
        /// List FdAcSummaryView
        /// </summary>
        /// <param name="grid,permitNum">jqGrid parameters,string</param>
        /// <returns>Messages</returns>
        IPagedList<FdAcSummaryView> GetPageByFdAcSummaryView(GridSettings grid, string permitNum, string flagDay);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgRef"></param>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        bool NewApplicantExists(string orgRef, int fdMasterId);
    }
}
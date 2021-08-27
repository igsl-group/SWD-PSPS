using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial interface IPspService
    { /// <summary>
        /// Get system message by Id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        PspMaster GetPSPById(int Id);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<PspMaster> GetPage(GridSettings grid);

        /// <summary>
        /// List PspMaster
        /// </summary>
        /// <param name="grid,permitNum">jqGrid parameters ,string</param>
        /// <returns>Messages</returns>
        IPagedList<PspMaster> GetPage(GridSettings grid, string permitNum);

        /// <summary>
        /// List Psp search
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid);

        /// <summary>
        /// Update a PspMaster
        /// </summary>
        /// <param name="model">PspMaster</param>
        void UpdatePspMaster(PspMaster model);

        /// <summary>
        /// Updates the PspMaster
        /// </summary>
        /// <param name="oldPspMaster">The old PspMaster</param>
        /// <param name="newPspMaster">The new PspMaster</param>
        void UpdatePspMaster(PspMaster oldPspMaster, PspMaster newPspMaster);

        /// <summary>
        /// Create a PspMaster
        /// </summary>
        /// <param name="model">PspMaster</param>
        void CreatePspMaster(PspMaster model);

        /// <summary>
        /// return max seq of psp file ref
        /// </summary>
        /// <param name="pspYear"></param>
        /// <returns></returns>
        string GetMaxSeq(string yearOfPsp);

        /// <summary>
        /// return recomend event page list
        /// </summary>
        /// <param name="pspYear"></param>
        /// <returns></returns>
        IPagedList<PspApproveEventDto> GetRecommendPspEvents(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        Hashtable CalPspEditRecCnt(int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetAllPspYearForDropdown();

        /// <summary>
        /// Gets all Lookups for dropdown
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetAllEventYearForDropdown();

        /// <summary>
        /// List PspAcSummaryView
        /// </summary>
        /// <param name="grid,permitNum">jqGrid parameters ,string</param>
        /// <returns>Messages</returns>
        IPagedList<PspAcSummaryView> GetPspAcSummaryPage(GridSettings grid, string permitNum);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        bool getByPassVal(int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Grid"></param>
        /// <returns></returns>
        MemoryStream GetPageToXls(GridSettings Grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="xlsxStream"></param>
        /// <returns></returns>
        MemoryStream InserPspByImportXls(Stream xlsxStream);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        PspMaster GetChildPspmaster(int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgRef"></param>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        bool NewApplicantExists(string orgRef, int pspMasterId);

        /// <summary>
        /// CR-005 Obtain the next PermitNum from PspMaster
        /// </summary>
        /// <param name="pspYear"></param>
        /// <returns></returns>
        string GetNextPermitNum(string pspYear);

        /// <summary>
        /// CR-005 Obtain the amended PermitNum from PspMaster
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        string GetNextPermitNum(int pspMasterId);
    }
}
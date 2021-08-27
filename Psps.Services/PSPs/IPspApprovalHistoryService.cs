using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial interface IPspApprovalHistoryService
    {
        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<PspApprovalHistory> GetPage(GridSettings grid);

        /// <summary>
        /// Create a PspApprovalHistory
        /// </summary>
        /// <param name="model">PspApprovalHistory</param>
        void CreatePspApprovalHistory(PspApprovalHistory model);

        /// <summary>
        /// Update a PspApprovalHistory
        /// </summary>
        /// <param name="model">PspApprovalHistory</param>
        void UpdatePspApprovalHistory(PspApprovalHistory model);

        /// <summary>
        /// delete PspApprovalHistory
        /// </summary>
        /// <param name="model"></param>
        void Delete(PspApprovalHistory model);

        /// <summary>
        /// Create a PspApprovalHistory
        /// </summary>
        /// <param name="model">PspApprovalHistory</param>
        PspApprovalHistory getPspApprovalHistoryById(int id);

        /// <summary>
        /// get approvalhistory by permit no
        /// </summary>
        /// <param name="permitNo"></param>
        /// <returns></returns>
        PspApprovalHistory GetPspApprovalHistByPermitNo(string permitNo);

        /// <summary>
        /// get max seq at the end
        /// </summary>
        /// <param name="pspYear"></param>
        /// <param name="ApprType"></param>
        /// <returns></returns>
        string GetNewCaseMaxSeq(string pspYear);

        /// <summary>
        ///
        /// </summary>
        /// <param name="prevPspMasterId"></param>
        /// <param name="pspYear"></param>
        /// <returns></returns>
        string GetOldCaseMaxSeq(int prevPspMasterId, string pspYear);

        /// <summary>
        /// get max seq at the end
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        int ifContainTwoBatchRec(int pspMasterId);

        /// <summary>
        /// get max seq at the end
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        bool HasRecomAprovRecs(int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        bool HasCancelledRecs(int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IPagedList<PspApprovalHistorySummaryDto> GetPspApprovHistSummary(GridSettings grid, int pspMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PspMaster getPspMasterByApprHistId(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        bool ifPspMasterIsApproved(int? pspMasterId);
    }
}
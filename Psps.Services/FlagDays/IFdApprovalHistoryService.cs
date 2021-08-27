using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psps.Models.Dto.FdStatus;

namespace Psps.Services.FlagDays
{
    public partial interface IFdApprovalHistoryService
    {
        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<FdApprovalHistory> GetPage(GridSettings grid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        IPagedList<FdStatusSummary> GetFdStatus(GridSettings grid);

        /// <summary>
        /// Create a FdApprovalHistory
        /// </summary>
        /// <param name="model">FdApprovalHistory</param>
        void CreateFdApprovalHistory(FdApprovalHistory model);

        /// <summary>
        /// Get FdApprovalHistory by Id
        /// </summary>
        /// <param name="fdYear">int</param>
        /// <returns>FdApprovalHistory</returns>
        FdApprovalHistory GetFdApprovalHistoryById(string fdYear);

    }
}

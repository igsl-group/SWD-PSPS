using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.FdStatus;
using Psps.Models.Dto.PspMaster;
using Psps.Models.Dto.SuggestionMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Home
{
    public partial interface IHomeService
    { /// <summary>
        /// Get system message by Id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        IPagedList<FdStatusDto> GetFdSatusSummary(GridSettings grid, int year);

        List<SuggestDto> GetLastFiveYrsSuggestionSummary();

        List<CompEnqDto> GetLastFiveYrsCompEnqSummary();

        IPagedList<PspMasterDto> GetPspBringUpSummary(GridSettings grid);

        IPagedList<ComplaintBringUpDto> GetComplaintBringUpSummary(GridSettings grid);

        IPagedList<PspApplicationStatusView> GetPspApplicationStatus(GridSettings grid);

        IPagedList<SsafApplicationStatusView> GetSsafApplicationStatus(GridSettings grid);

        IPagedList<PspBringUpSummaryView> GetPspBringUpSummaryView(GridSettings grid);

        IPagedList<ComplaintBringUpSummaryView> GetComplaintBringUpSummaryView(GridSettings grid);

    }
}
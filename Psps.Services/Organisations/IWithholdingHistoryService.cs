using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Organisation;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial interface IWithholdingHistoryService
    {
        IPagedList<WithholdingHistory> GetPage(GridSettings grid);

        /// <summary>
        /// Get WithholdingHistory Amount By OrgId
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetWithholdingHistoryAmountByOrgId(string OrgId);

        WithHoldingDate GetWithholdingDateByOrgId(int OrgId);

        //string GetMaxWithholdingBeginDateByOrgId(int OrgId);

        //string GetMaxWithholdingEndDateByOrgId(int OrgId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        bool GetWithHoldBySysDt(int OrgId);
    }
}
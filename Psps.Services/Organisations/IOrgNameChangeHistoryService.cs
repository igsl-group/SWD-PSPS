using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial interface IOrgNameChangeHistoryService
    {
        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<OrgNameChangeHistory> GetPage(GridSettings grid);

        /// <summary>
        /// Create a OrgNameChangeHistory
        /// </summary>
        /// <param name="model">OrgNameChangeHistory</param>
        void CreateOrgNameChangeHistory(OrgNameChangeHistory model);

        /// <summary>
        /// Get OrgNameChangeHistory Amount By OrgId 
        /// </summary>
        /// <param name="OrgId">string</param>
        /// <returns>int</returns>
        int GetOrgNameChangeHistoryAmountByOrgId(string OrgId);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="Id">int</param>
        /// <returns>System message entity</returns>
        OrgNameChangeHistory GetById(int Id);

        /// <summary>
        /// Update a OrgNameChangeHistory
        /// </summary>
        /// <param name="orgNameChangeHistory">OrgNameChangeHistory</param>
        void Update(OrgNameChangeHistory orgNameChangeHistory);

        /// <summary>
        /// Delete a OrgNameChangeHistory
        /// </summary>
        /// <param name="OrgNameChangeId">int</param>
        void Delete(int OrgNameChangeId);
    }
}

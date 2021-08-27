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
    public partial interface IOrgRefGuidePromulgationService
    {
        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<OrgRefGuidePromulgation> GetPage(GridSettings grid);

        /// <summary>
        /// Create a OrgRefGuidePromulgation
        /// </summary>
        /// <param name="model">OrgRefGuidePromulgation</param>
        void CreateOrgRefGuidePromulgation(OrgRefGuidePromulgation model);

        /// <summary>
        /// update a OrgRefGuidePromulgation
        /// </summary>
        /// <param name="model">OrgRefGuidePromulgation</param>
        void UpdateOrgRefGuidePromulgation(OrgRefGuidePromulgation model);

        /// <summary>
        /// Gets a OrgRefGuidePromulgation by Id
        /// </summary>
        /// <param name="orgId">Attachment Id</param>
        /// <returns>OrgRefGuidePromulgation</returns>
        OrgRefGuidePromulgation GetOrgRefGuidePromulgationByOrgRefGuidePromulgationId(int orgRefGuidePromulgationId);
    }
}
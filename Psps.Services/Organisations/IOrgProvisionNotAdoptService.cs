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
    public partial interface IOrgProvisionNotAdoptService
    {

        /// <summary>
        /// Delete OrgProvisionNotAdopt by OrgRefGuidePromulgationId
        /// </summary>
        /// <param>void</param>
        void DeleteByGuidePromulgationId(int OrgRefGuidePromulgationId);

        /// <summary>
        /// Create a OrgProvisionNotAdopt
        /// </summary>
        /// <param name="orgProvisionNotAdopt">OrgProvisionNotAdopt</param>
        void CreateOrgProvisionNotAdopt(OrgProvisionNotAdopt orgProvisionNotAdopt);

        /// <summary>
        /// Get List By OrgRefGuidePromulgationId
        /// </summary>
        /// <param>OrgRefGuidePromulgationId</param>
        /// <returns>IList<OrgProvisionNotAdopt></returns>
        IList<OrgProvisionNotAdopt> GetAllByOrgRefGuidePromulgationId(int OrgRefGuidePromulgationId);

    }
}

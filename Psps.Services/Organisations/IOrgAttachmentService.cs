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

    public partial interface IOrgAttachmentService
    {

        /// <summary>
        /// List OrgAttachment
        /// </summary>
        /// <param name="grid">GridSettings</param>
        /// <returns>IPagedList</returns>
        IPagedList<OrgAttachment> GetPage(GridSettings grid);


        /// <summary>
        /// Get OrgAttachment Amount By code 
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetOrgAttachmentAmountByCode(string code);

        /// <summary>
        /// Create a OrgAttachment
        /// </summary>
        /// <param name="model">model</param>
        void CreateOrgAttachment(OrgAttachment model);

        /// <summary>
        /// Gets a OrgAttachment
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>OrgAttachment</returns>
        OrgAttachment GetOrgAttachmentByName(string name);

        /// <summary>
        /// Gets a OrgAttachment by Id
        /// </summary>
        /// <param name="attachmentId">Attachment Id</param>
        /// <returns>OrgAttachment</returns>
        OrgAttachment GetOrgAttachmentById(int attachmentId);

        /// <summary>
        /// Update a OrgAttachment
        /// </summary>
        /// <param name="OrgAttachment">OrgAttachment</param>
        void UpdateOrgAttachment(OrgAttachment model);

        /// <summary>
        /// Delete a OrgAttachment
        /// </summary>
        /// <param name="OrgAttachment">OrgAttachment</param>
        void DeleteOrgAttachment(OrgAttachment model);

    }
}

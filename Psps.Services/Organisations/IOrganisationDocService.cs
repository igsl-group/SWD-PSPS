using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;

using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial interface IOrganisationDocService
    {
        /// <summary>
        /// List OrgDocs
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<OrgDoc> GetPage(GridSettings grid);

        /// <summary>
        /// Get OrgDoc Amount By code
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetOrgDocAmount();

        /// <summary>
        /// Create a OrgDoc
        /// </summary>
        /// <param name="model">OrgDoc</param>
        void CreateOrgDoc(OrgDoc model);

        /// <summary>
        /// Update a OrgDoc
        /// </summary>
        /// <param name="model">OrgDoc</param>
        void UpdateOrgDoc(OrgDoc model);

        /// <summary>
        /// Get a OrgDoc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrgDoc GetOrgDocById(int id);

        /// <summary>
        /// Get a OrgDoc by View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrgDocView GetOrgDocViewById(int id);

        /// <summary>
        /// Get All OrgDoc Records
        /// </summary>
        /// <param name="param">OrganisationRef param eg : 300,301</param>
        IList<OrgDoc> GetRecordsByParam(string param);

        /// <summary>
        /// Check Active Template OrgDoc as deleted
        /// </summary>
        /// <param name="letterId">letterId</param>
        bool CheckActiveTemplate(int letterId);

        /// <summary>
        /// Check Least Template OrgDoc as deleted
        /// </summary>
        /// <param name="name">name</param>
        bool CheckLeastTemplate(string docNum);

        void DeleteOrgDoc(OrgDoc suggestionDoc);

        OrgDoc GetOrgDocByVersionNum(string versionNum);

        IPagedList<OrgDoc> GetPage(GridSettings grid, string docNum);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueOrgDocNum(int suggestionDocId, string docNum);

        /// <summary>
        /// Determine the uniqueness of Version within the same set of Letters
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <param name="version">Version</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueOrgDocVersion(int suggestionDocId, string docNum, string version);

        /// <summary>
        /// List OrgDocSummaryView
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>OrgDocSummaryView</returns>
        IPagedList<OrgDocSummaryView> GetOrgDocSummaryViewPage(GridSettings grid);
    }
}
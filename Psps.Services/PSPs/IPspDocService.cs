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

namespace Psps.Services.PSPs
{
    public partial interface IPspDocService
    {
        /// <summary>
        /// List PspDocs
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<PspDoc> GetPage(GridSettings grid);

        /// <summary>
        /// Get PspDoc Amount By code
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetPspDocAmount();

        /// <summary>
        /// Create a PspDoc
        /// </summary>
        /// <param name="model">PspDoc</param>
        void CreatePspDoc(PspDoc model);

        /// <summary>
        /// Update a PspDoc
        /// </summary>
        /// <param name="model">PspDoc</param>
        void UpdatePspDoc(PspDoc model);

        /// <summary>
        /// Get a PspDoc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PspDoc GetPspDocById(int id);

        /// <summary>
        /// Get a PspDoc from View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PspDocView GetPspDocViewById(int id);

        /// <summary>
        /// Get All PspDoc Records
        /// </summary>
        /// <param name="param">OrganisationRef param eg : 300,301</param>
        IList<PspDoc> GetRecordsByParam(string param);

        /// <summary>
        /// Check Active Template PspDoc as deleted
        /// </summary>
        /// <param name="letterId">letterId</param>
        bool CheckActiveTemplate(int letterId);

        /// <summary>
        /// Check Least Template PspDoc as deleted
        /// </summary>
        /// <param name="docNum">docNum</param>
        bool CheckLeastTemplate(string docNum);

        void DeletePspDoc(PspDoc PspDoc);

        PspDoc GetPspDocByVersionNum(string versionNum);

        IPagedList<PspDoc> GetPage(GridSettings grid, string docNum);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniquePspDocNum(int pspDocId, string docNum);

        /// <summary>
        /// Determine the uniqueness of Version within the same set of Letters
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <param name="version">Version</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniquePspDocVersion(int pspDocId, string docNum, string version);

        IPagedList<PspDocSummaryView> GetPspDocSummaryViewPage(GridSettings grid);
    }
}
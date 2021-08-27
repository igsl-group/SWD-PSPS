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

namespace Psps.Services.FlagDays
{
    public partial interface IFlagDayDocService
    {
        /// <summary>
        /// List FdDocs
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<FdDoc> GetPage(GridSettings grid);

        /// <summary>
        /// Get FdDoc Amount By code
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetFdDocAmount();

        /// <summary>
        /// Create a FdDoc
        /// </summary>
        /// <param name="model">FdDoc</param>
        void CreateFdDoc(FdDoc model);

        /// <summary>
        /// Update a FdDoc
        /// </summary>
        /// <param name="model">FdDoc</param>
        void UpdateFdDoc(FdDoc model);

        IDictionary<string, string> getAllFdDocTemplateForDropdown();

        /// <summary>
        /// Get a FdDoc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FdDoc GetFdDocById(int id);

        /// <summary>
        /// Get a FdDoc from View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FdDocView GetFdDocViewById(int id);

        /// <summary>
        /// Get All FdDoc Records
        /// </summary>
        /// <param name="param">FlagDayRef param eg : 300,301</param>
        IList<FdDoc> GetRecordsByParam(string param);

        /// <summary>
        /// Check Active Template FdDoc as deleted
        /// </summary>
        /// <param name="letterId">letterId</param>
        bool CheckActiveTemplate(int letterId);

        /// <summary>
        /// Check Least Template FdDoc as deleted
        /// </summary>
        /// <param name="docNum">docNum</param>
        bool CheckLeastTemplate(string docNum);

        void DeleteFdDoc(FdDoc flagDayDoc);

        IPagedList<FdDoc> GetPage(GridSettings grid, string docNum);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueFdDocNum(int flagDayDocId, string docNum);

        /// <summary>
        /// Determine the uniqueness of Version within the same set of Letters
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <param name="version">Version</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueFdDocVersion(int flagDayDocId, string docNum, string version);

        IPagedList<FdDocSummaryView> GetFdDocSummaryViewPage(GridSettings grid);
    }
}
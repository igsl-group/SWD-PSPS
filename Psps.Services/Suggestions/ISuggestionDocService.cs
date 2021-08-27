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

namespace Psps.Services.Suggestions
{
    public partial interface ISuggestionDocService
    {
        /// <summary>
        /// List SuggestionDocs
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<SuggestionDoc> GetPage(GridSettings grid);

        /// <summary>
        /// Get SuggestionDoc Amount By code
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetSuggestionDocAmount();

        /// <summary>
        /// Create a SuggestionDoc
        /// </summary>
        /// <param name="model">SuggestionDoc</param>
        void CreateSuggestionDoc(SuggestionDoc model);

        /// <summary>
        /// Update a SuggestionDoc
        /// </summary>
        /// <param name="model">SuggestionDoc</param>
        void UpdateSuggestionDoc(SuggestionDoc model);

        /// <summary>
        /// Get a SuggestionDoc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SuggestionDoc GetSuggestionDocById(int id);

        /// <summary>
        /// Get a SuggestionDoc by View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SuggestionDocView GetSuggestionDocViewById(int id);

        /// <summary>
        /// Get All SuggestionDoc Records
        /// </summary>
        /// <param name="param">SuggestionRef param eg : 300,301</param>
        IList<SuggestionDoc> GetRecordsByParam(string param);

        /// <summary>
        /// Check Active Template SuggestionDoc as deleted
        /// </summary>
        /// <param name="letterId">letterId</param>
        bool CheckActiveTemplate(int letterId);

        /// <summary>
        /// Check Least Template SuggestionDoc as deleted
        /// </summary>
        /// <param name="name">name</param>
        bool CheckLeastTemplate(string name);

        void DeleteSuggestionDoc(SuggestionDoc suggestionDoc);

        SuggestionDoc GetSuggestionDocByVersionNum(string versionNum);

        IPagedList<SuggestionDoc> GetPage(GridSettings grid, string docNum);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueSuggestionDocNum(int suggestionDocId, string docNum);

        /// <summary>
        /// Determine the uniqueness of Version within the same set of Letters
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <param name="version">Version</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueSuggestionDocVersion(int suggestionDocId, string docNum, string version);

        IPagedList<SuggestionDocSummaryView> GetSuggestionDocSummaryViewPage(GridSettings grid);
    }
}
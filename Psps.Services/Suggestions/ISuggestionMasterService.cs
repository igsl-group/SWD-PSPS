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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Suggestions
{
    public partial interface ISuggestionMasterService
    {
        /// <summary>
        /// List SuggestionMasters
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<SuggestionMaster> GetPage(GridSettings grid);

        /// <summary>
        /// Create a SuggestionMaster
        /// </summary>
        /// <param name="model">SuggestionMaster</param>
        void CreateSuggestionMaster(SuggestionMaster model);

        /// <summary>
        /// Update a SuggestionMaster
        /// </summary>
        /// <param name="model">SuggestionMaster</param>
        void UpdateSuggestionMaster(SuggestionMaster model);

        /// <summary>
        /// Updates the SuggestionMaster
        /// </summary>
        /// <param name="oldSuggestionMaster">The old SuggestionMaster</param>
        /// <param name="newSuggestionMaster">The new SuggestionMaster</param>
        void UpdateSuggestionMaster(SuggestionMaster oldSuggestionMaster, SuggestionMaster newSuggestionMaster);

        /// <summary>
        /// Get a SuggestionMaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SuggestionMaster GetSuggestionMasterById(int id);

        /// <summary>
        /// Get All SuggestionMaster Records
        /// </summary>
        /// <param name="param">SuggestionRef param eg : 300,301</param>
        IList<SuggestionMaster> GetRecordsByParam(string param);

        /// <summary>
        /// Create SuggestionRefNum
        /// </summary>
        /// <returns>string</returns>
        string CreateSuggestionRefNum();

        /// <summary>
        /// Generate R20 report
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="generatedBy"></param>
        /// <returns></returns>
        MemoryStream GenerateR20PDF(String templatePath, DateTime? fromDate, DateTime? toDate);
    }
}
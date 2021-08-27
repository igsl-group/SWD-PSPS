using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Suggestions
{
    public partial interface ISuggestionAttachmentService
    {

        /// <summary>
        /// List SuggestionAttachment
        /// </summary>
        /// <param name="grid">GridSettings</param>
        /// <returns>IPagedList</returns>
        IPagedList<SuggestionAttachment> GetPage(GridSettings grid);


        /// <summary>
        /// Get SuggestionAttachment Amount By code 
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetSuggestionAttachmentAmountByCode(string code);

        /// <summary>
        /// Create a SuggestionAttachment
        /// </summary>
        /// <param name="model">model</param>
        void CreateSuggestionAttachment(SuggestionAttachment model);

        /// <summary>
        /// Gets a SuggestionAttachment
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>SuggestionAttachment</returns>
        SuggestionAttachment GetSuggestionAttachmentByName(string name);

        /// <summary>
        /// Gets a SuggestionAttachment by Id
        /// </summary>
        /// <param name="attachmentId">Attachment Id</param>
        /// <returns>SuggestionAttachment</returns>
        SuggestionAttachment GetSuggestionAttachmentById(int attachmentId);

        /// <summary>
        /// Update a SuggestionAttachment
        /// </summary>
        /// <param name="SuggestionAttachment">SuggestionAttachment</param>
        void UpdateSuggestionAttachment(SuggestionAttachment model);

        /// <summary>
        /// Delete a SuggestionAttachment
        /// </summary>
        /// <param name="SuggestionAttachment">SuggestionAttachment</param>
        void DeleteSuggestionAttachment(SuggestionAttachment model);

    }
}

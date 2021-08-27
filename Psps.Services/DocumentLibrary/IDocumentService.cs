using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.DocumentLibrary
{
    /// <summary>
    /// Document service interface
    /// </summary>
    public partial interface IDocumentService
    {
        /// <summary>
        /// Gets a Document
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <returns>A Document</returns>
        Document GetDocumentById(int documentId);

        /// <summary>
        /// Create a Document
        /// </summary>
        /// <param name="document">Document</param>
        void CreateDocument(Document document);

        void UpdateDocument(Document document);

        /// <summary>
        /// Marks Document as deleted
        /// </summary>
        /// <param name="document">Document</param>
        void DeleteDocument(Document document);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="documentLibraryId">Document Library Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the same Folder</returns>
        bool IsUniqueDocumentName(int documentLibraryId, string name);

        bool IsUniqueDocumentName(int documentLibraryId, int documentId, string name);

        /// <summary>
        /// List Document
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <param name="documentLibraryId">Document Library Id</param>
        /// <returns>Documents</returns>
        IPagedList<Document> GetPage(GridSettings grid, int documentLibraryId);
    }
}
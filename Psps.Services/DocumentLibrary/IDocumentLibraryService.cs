using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.DocumentLibrary
{
    /// <summary>
    /// Document library service interface
    /// </summary>
    public partial interface IDocumentLibraryService
    {
        /// <summary>
        /// Get all document library
        /// </summary>
        /// <returns>List of Document Library</returns>
        IEnumerable<Psps.Models.Domain.DocumentLibrary> GetAllDocumentLibrary();

        /// <summary>
        /// Create a document library
        /// </summary>
        /// <param name="name">Document library name</param>
        /// <param name="parentDocumentLibraryId">(Optional) Parent document library id</param>
        Psps.Models.Domain.DocumentLibrary CreateDocumentLibrary(string name, int? parentDocumentLibraryId);

        /// <summary>
        /// Marks document library as deleted
        /// </summary>
        /// <param name="documentLibraryId">Document library id</param>
        void DeleteDocumentLibrary(int documentLibraryId);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="documentLibraryId">(Optional) Document library id</param>
        /// <param name="name">Name</param>
        /// <returns>Unique within the same document library</returns>
        bool IsUniqueDocumentLibraryName(int? documentLibraryId, string name);

        bool IsContainDocumentOrSubDocumentLibrary(int documentLibraryId);

        Psps.Models.Domain.DocumentLibrary GetDocumentLibrary(int documentLibraryId);

        void UpdateDocumentLibrary(Psps.Models.Domain.DocumentLibrary documentLibrary);
    }
}
using Psps.Core.Models;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class DocumentLibrary : BaseAuditEntity<int>
    {
        public DocumentLibrary()
        {
            Documents = new List<Document>();
            DocumentLibraries = new List<DocumentLibrary>();
        }

        public virtual int DocumentLibraryId { get; set; }

        public virtual DocumentLibrary Parent { get; set; }

        public virtual string Name { get; set; }

        public virtual string Path { get; set; }

        public virtual IList<Document> Documents { get; set; }

        public virtual IList<DocumentLibrary> DocumentLibraries { get; set; }

        public override int Id
        {
            get
            {
                return DocumentLibraryId;
            }
            set
            {
                DocumentLibraryId = value;
            }
        }
    }
}
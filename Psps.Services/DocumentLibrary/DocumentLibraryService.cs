using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.DocumentLibrary
{
    public partial class DocumentLibraryService : IDocumentLibraryService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string DOC_LIB_ALL_KEY = "Psps.doclib.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string DOC_LIB_PATTERN_KEY = "Psps.doclib.";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IDocumentLibraryRepository _documentLibraryRepository;

        #endregion Fields

        #region Ctor

        public DocumentLibraryService(ICacheManager cacheManager, IEventPublisher eventPublisher, IDocumentLibraryRepository documentLibraryRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._documentLibraryRepository = documentLibraryRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IEnumerable<Psps.Models.Domain.DocumentLibrary> GetAllDocumentLibrary()
        {
            return _documentLibraryRepository.GetAll("Parent");
        }

        public virtual Psps.Models.Domain.DocumentLibrary CreateDocumentLibrary(string name, int? parentDocumentLibraryId)
        {
            Ensure.Argument.NotNullOrEmpty(name, "name");

            Psps.Models.Domain.DocumentLibrary parent = null;
            if (parentDocumentLibraryId.HasValue)
                parent = _documentLibraryRepository.GetById(parentDocumentLibraryId.Value);

            var path = parent != null ? Path.Combine(parent.Path, name) : name;
            path = CommonHelper.CleanPath(path);

            var documentLibrary = new Psps.Models.Domain.DocumentLibrary
            {
                Name = name,
                Parent = parent,
                Path = path
            };

            _documentLibraryRepository.Add(documentLibrary);
            _eventPublisher.EntityInserted<Psps.Models.Domain.DocumentLibrary>(documentLibrary);

            return documentLibrary;
        }

        public virtual void DeleteDocumentLibrary(int documentLibraryId)
        {
            var documentLibrary = _documentLibraryRepository.GetById(documentLibraryId);
            documentLibrary.IsDeleted = true;

            _documentLibraryRepository.Update(documentLibrary);
        }

        public virtual bool IsUniqueDocumentLibraryName(int? documentLibraryId, string name)
        {
            Ensure.Argument.NotNull(name, "name");

            if (documentLibraryId.HasValue)
                return _documentLibraryRepository.Table.Count(l => l.Name == name && l.Parent.DocumentLibraryId == documentLibraryId.Value) == 0;
            else
                return _documentLibraryRepository.Table.Count(l => l.Name == name && l.Parent == null) == 0;
        }

        public bool IsContainDocumentOrSubDocumentLibrary(int documentLibraryId)
        {
            var documentLibrary = _documentLibraryRepository.GetById(documentLibraryId);

            if (documentLibrary != null)
            {
                if (documentLibrary.DocumentLibraries.Count > 0)
                    return true;

                if (documentLibrary.Documents.Count > 0)
                    return true;
            }

            return false;
        }

        public Psps.Models.Domain.DocumentLibrary GetDocumentLibrary(int documentLibraryId)
        {
            return _documentLibraryRepository.GetById(documentLibraryId);
        }

        public void UpdateDocumentLibrary(Psps.Models.Domain.DocumentLibrary documentLibrary) 
        {
            _documentLibraryRepository.Update(documentLibrary);
            _eventPublisher.EntityUpdated<Psps.Models.Domain.DocumentLibrary>(documentLibrary);

        }
        #endregion Methods
    }
}
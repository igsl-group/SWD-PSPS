using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.DocumentLibrary
{
    public partial class DocumentService : IDocumentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IDocumentRepository _documentRepository;

        #endregion Fields

        #region Ctor

        public DocumentService(ICacheManager cacheManager, IEventPublisher eventPublisher, IDocumentRepository documentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._documentRepository = documentRepository;
        }

        #endregion Ctor

        #region Methods

        public Document GetDocumentById(int documentId)
        {
            if (documentId == 0)
                return null;

            return _documentRepository.GetById(documentId);
        }

        public void CreateDocument(Document document)
        {
            Ensure.Argument.NotNull(document, "document");

            _documentRepository.Add(document);

            _eventPublisher.EntityInserted<Document>(document);
        }

        public void UpdateDocument(Document document)
        {
            Ensure.Argument.NotNull(document, "document");

            _documentRepository.Update(document);

            //event notification
            _eventPublisher.EntityUpdated<Document>(document);
        }

        public virtual void DeleteDocument(Document document)
        {
            Ensure.Argument.NotNull(document, "document");

            document.IsDeleted = true;

            UpdateDocument(document);
        }

        public bool IsUniqueDocumentName(int documentLibraryId, string name)
        {
            Ensure.Argument.NotNull(name, "name");

            return _documentRepository.Table.Count(l => l.DocumentLibrary.DocumentLibraryId == documentLibraryId && l.Name == name) == 0;
        }

        public bool IsUniqueDocumentName(int documentLibraryId, int documentId, string name)
        {
            Ensure.Argument.NotNull(name, "name");

            return _documentRepository.Table.Count(l => l.DocumentLibrary.DocumentLibraryId == documentLibraryId && l.DocumentId != documentId && l.Name == name) == 0;
        }

        public Core.Models.IPagedList<Document> GetPage(Core.JqGrid.Models.GridSettings grid, int documentLibraryId)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "DocumentLibrary.DocumentLibraryId",
                data = documentLibraryId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            return _documentRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
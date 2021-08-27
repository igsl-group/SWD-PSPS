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
    public partial class SuggestionAttachmentService : ISuggestionAttachmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISuggestionAttachmentRepository _suggestionAttachmentRepository;

        #endregion Fields

        #region Ctor

        public SuggestionAttachmentService(ICacheManager cacheManager, IEventPublisher eventPublisher, ISuggestionAttachmentRepository suggestionAttachmentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._suggestionAttachmentRepository = suggestionAttachmentRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<SuggestionAttachment> GetPage(GridSettings grid)
        {
            return _suggestionAttachmentRepository.GetPage(grid);
        }

        public virtual int GetSuggestionAttachmentAmountByCode(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code, "code");
            return _suggestionAttachmentRepository.Table.Count(a => a.SuggestionMaster.SuggestionMasterId == Convert.ToInt32(code));
        }

        public virtual void CreateSuggestionAttachment(SuggestionAttachment model)
        {
            Ensure.Argument.NotNull(model);
            _suggestionAttachmentRepository.Add(model);
            _eventPublisher.EntityInserted<SuggestionAttachment>(model);
        }

        public virtual SuggestionAttachment GetSuggestionAttachmentByName(string name)
        {
            Ensure.Argument.NotNullOrEmpty(name, "name");
            var list = _suggestionAttachmentRepository.Table.Where(a => a.FileName == name).ToList();
            SuggestionAttachment suggestionAttachment = null;
            if (list != null && list.Count() > 0)
            {
                suggestionAttachment = list.First();
            }
            return suggestionAttachment;
        }

        public virtual SuggestionAttachment GetSuggestionAttachmentById(int attachmentId)
        {
            return _suggestionAttachmentRepository.GetById(attachmentId);
        }

        public virtual void UpdateSuggestionAttachment(SuggestionAttachment model)
        {
            Ensure.Argument.NotNull(model);
            _suggestionAttachmentRepository.Update(model);
            _eventPublisher.EntityUpdated<SuggestionAttachment>(model);
        }

        public virtual void DeleteSuggestionAttachment(SuggestionAttachment model)
        {
            model.IsDeleted = true;
            _suggestionAttachmentRepository.Update(model);
            _eventPublisher.EntityUpdated<SuggestionAttachment>(model);
        }

        #endregion Methods
    }
}

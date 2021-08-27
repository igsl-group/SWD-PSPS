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
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class SuggestionDocService : ISuggestionDocService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISuggestionDocRepository _suggestionDocRepository;
        private readonly ISuggestionDocViewRepository _suggestionDocViewRepository;
        private readonly ISuggestionDocSummaryViewRepository _suggestionDocSummaryViewRepository;

        #endregion Fields

        #region Ctor

        public SuggestionDocService(ICacheManager cacheManager, IEventPublisher eventPublisher, ISuggestionDocRepository suggestionDocRepository, ISuggestionDocViewRepository suggestionDocViewRepository, ISuggestionDocSummaryViewRepository suggestionDocSummaryViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._suggestionDocRepository = suggestionDocRepository;
            this._suggestionDocViewRepository = suggestionDocViewRepository;
            this._suggestionDocSummaryViewRepository = suggestionDocSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<SuggestionDoc> GetPage(GridSettings grid)
        {
            return _suggestionDocRepository.GetPage(grid);
        }

        public virtual int GetSuggestionDocAmount()
        {
            return _suggestionDocRepository.Table.Count(s => s.DocStatus == true);
        }

        public void CreateSuggestionDoc(SuggestionDoc model)
        {
            if (model.DocStatus)
            {
                _suggestionDocRepository.ChangeOtherVersionStatus(model);
            }
            _suggestionDocRepository.Add(model);
            _eventPublisher.EntityInserted<SuggestionDoc>(model);
        }

        public void UpdateSuggestionDoc(SuggestionDoc model)
        {
            if (model.DocStatus)
            {
                _suggestionDocRepository.ChangeOtherVersionStatus(model);
            }
            _suggestionDocRepository.Update(model);
            _eventPublisher.EntityUpdated<SuggestionDoc>(model);
        }

        public SuggestionDoc GetSuggestionDocById(int id)
        {
            return _suggestionDocRepository.GetById(id);
        }

        public SuggestionDocView GetSuggestionDocViewById(int id)
        {
            return _suggestionDocViewRepository.GetById(id);
        }

        public virtual IList<SuggestionDoc> GetRecordsByParam(string param)
        {
            return _suggestionDocRepository.GetRecordsByParam(param);
        }

        public SuggestionDoc GetSuggestionDocByVersionNum(string versionNum)
        {
            Ensure.Argument.NotNull(versionNum, "versionNum");

            return _suggestionDocRepository.Get(s => s.VersionNum == versionNum);
        }

        public Core.Models.IPagedList<SuggestionDoc> GetPage(Core.JqGrid.Models.GridSettings grid, string docNum)
        {
            return _suggestionDocRepository.GetPage(grid, docNum);
        }

        public bool CheckActiveTemplate(int id)
        {
            Ensure.Argument.NotNull(id, "suggestionDocId");
            bool isTrue = true;
            if (_suggestionDocRepository.Table.Count(s => s.SuggestionDocId == id && s.DocStatus == true) < 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public bool CheckLeastTemplate(string name)
        {
            bool isTrue = true;
            if (_suggestionDocRepository.Table.Count(s => s.DocName == name) > 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public virtual void DeleteSuggestionDoc(SuggestionDoc suggestionDoc)
        {
            Ensure.Argument.NotNull(suggestionDoc, "letter");

            suggestionDoc.IsDeleted = true;
            UpdateSuggestionDoc(suggestionDoc);
        }

        public bool IsUniqueSuggestionDocNum(int suggestionDocId, string docNum)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _suggestionDocRepository.Table.Count(l => l.DocNum == docNum && l.SuggestionDocId != suggestionDocId) == 0;
        }

        public bool IsUniqueSuggestionDocVersion(int suggestionDocId, string docNum, string version)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _suggestionDocRepository.Table.Count(l => l.DocNum == docNum && l.VersionNum == version && l.SuggestionDocId != suggestionDocId) == 0;
        }

        public IPagedList<SuggestionDocSummaryView> GetSuggestionDocSummaryViewPage(GridSettings grid)
        {
            return _suggestionDocSummaryViewRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
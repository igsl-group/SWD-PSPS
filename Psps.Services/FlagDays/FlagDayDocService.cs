using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
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

namespace Psps.Services.FlagDays
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class FlagDayDocService : IFlagDayDocService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IFdDocRepository _fdDocRepository;
        private readonly IFdDocViewRepository _fdDocViewRepository;
        private readonly IFdDocSummaryViewRepository _fdDocSummaryViewRepository;

        #endregion Fields

        #region Ctor

        public FlagDayDocService(ICacheManager cacheManager, IEventPublisher eventPublisher, IFdDocViewRepository flagDayDocViewRepository, IFdDocRepository flagDayDocRepository, IFdDocSummaryViewRepository fdDocSummaryViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._fdDocRepository = flagDayDocRepository;
            this._fdDocViewRepository = flagDayDocViewRepository;
            this._fdDocSummaryViewRepository = fdDocSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<FdDoc> GetPage(GridSettings grid)
        {
            return _fdDocRepository.GetPage(grid);
        }

        public virtual int GetFdDocAmount()
        {
            return _fdDocRepository.Table.Count();
        }

        public void CreateFdDoc(FdDoc model)
        {
            if (model.DocStatus)
            {
                _fdDocRepository.ChangeOtherVersionStatus(model);
            }
            _fdDocRepository.Add(model);
            _eventPublisher.EntityInserted<FdDoc>(model);
        }

        public void UpdateFdDoc(FdDoc model)
        {
            if (model.DocStatus)
            {
                _fdDocRepository.ChangeOtherVersionStatus(model);
            }
            _fdDocRepository.Update(model);
            _eventPublisher.EntityUpdated<FdDoc>(model);
        }

        public IDictionary<string, string> getAllFdDocTemplateForDropdown()
        {
            string key = Constant.FLAGDAYTEMPLATE_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._fdDocSummaryViewRepository.Table
                     .OrderBy(p => p.DocNum)
                     .Where(p => p.Enabled == true)
                     .Select(p => new { Key = p.FdDocId.ToString(), Value = p.DocNum + " - " + p.DocName })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public FdDoc GetFdDocById(int id)
        {
            return _fdDocRepository.GetById(id);
        }

        public FdDocView GetFdDocViewById(int id)
        {
            return _fdDocViewRepository.GetById(id);
        }

        public virtual IList<FdDoc> GetRecordsByParam(string param)
        {
            return _fdDocRepository.GetRecordsByParam(param);
        }

        public FdDoc GetFdDocByVersionNum(string versionNum)
        {
            Ensure.Argument.NotNull(versionNum, "versionNum");

            return _fdDocRepository.Get(s => s.VersionNum == versionNum);
        }

        public Core.Models.IPagedList<FdDoc> GetPage(Core.JqGrid.Models.GridSettings grid, string docNum)
        {
            return _fdDocRepository.GetPage(grid, docNum);
        }

        public bool CheckActiveTemplate(int id)
        {
            Ensure.Argument.NotNull(id, "flagDayDocId");
            bool isTrue = true;
            if (_fdDocRepository.Table.Count(s => s.FdDocId == id && s.DocStatus == true) < 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public bool CheckLeastTemplate(string docNum)
        {
            bool isTrue = true;
            if (_fdDocRepository.Table.Count(s => s.DocNum == docNum && s.IsDeleted == false) > 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public virtual void DeleteFdDoc(FdDoc flagDayDoc)
        {
            Ensure.Argument.NotNull(flagDayDoc, "letter");

            flagDayDoc.IsDeleted = true;
            UpdateFdDoc(flagDayDoc);
        }

        public bool IsUniqueFdDocNum(int flagDayDocId, string docNum)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _fdDocRepository.Table.Count(l => l.DocNum == docNum && l.FdDocId != flagDayDocId) == 0;
        }

        public bool IsUniqueFdDocVersion(int flagDayDocId, string docNum, string version)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _fdDocRepository.Table.Count(l => l.DocNum == docNum && l.VersionNum == version && l.FdDocId != flagDayDocId) == 0;
        }

        public IPagedList<FdDocSummaryView> GetFdDocSummaryViewPage(GridSettings grid)
        {
            return _fdDocSummaryViewRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
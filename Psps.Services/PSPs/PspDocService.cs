using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.PSPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class PspDocService : IPspDocService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPspDocRepository _pspDocRepository;
        private readonly IPspDocViewRepository _pspDocViewRepository;
        private readonly IPspDocSummaryViewRepository _pspDocSummaryViewRepository;

        #endregion Fields

        #region Ctor

        public PspDocService(ICacheManager cacheManager, IEventPublisher eventPublisher, IPspDocRepository pspDocRepository, IPspDocViewRepository pspDocViewRepository, IPspDocSummaryViewRepository pspDocSummaryViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._pspDocRepository = pspDocRepository;
            this._pspDocViewRepository = pspDocViewRepository;
            this._pspDocSummaryViewRepository = pspDocSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<PspDoc> GetPage(GridSettings grid)
        {
            return _pspDocRepository.GetPage(grid);
        }

        public virtual int GetPspDocAmount()
        {
            return _pspDocRepository.Table.Count();
        }

        public void CreatePspDoc(PspDoc model)
        {
            if (model.DocStatus)
            {
                _pspDocRepository.ChangeOtherVersionStatus(model);
            }
            _pspDocRepository.Add(model);
            _eventPublisher.EntityInserted<PspDoc>(model);
        }

        public void UpdatePspDoc(PspDoc model)
        {
            if (model.DocStatus)
            {
                _pspDocRepository.ChangeOtherVersionStatus(model);
            }
            _pspDocRepository.Update(model);
            _eventPublisher.EntityUpdated<PspDoc>(model);
        }

        public PspDoc GetPspDocById(int id)
        {
            return _pspDocRepository.GetById(id);
        }

        public PspDocView GetPspDocViewById(int id)
        {
            return _pspDocViewRepository.GetById(id);
        }

        public virtual IList<PspDoc> GetRecordsByParam(string param)
        {
            return _pspDocRepository.GetRecordsByParam(param);
        }

        public PspDoc GetPspDocByVersionNum(string versionNum)
        {
            Ensure.Argument.NotNull(versionNum, "versionNum");

            return _pspDocRepository.Get(s => s.VersionNum == versionNum);
        }

        public Core.Models.IPagedList<PspDoc> GetPage(Core.JqGrid.Models.GridSettings grid, string docNum)
        {
            return _pspDocRepository.GetPage(grid, docNum);
        }

        public bool CheckActiveTemplate(int id)
        {
            Ensure.Argument.NotNull(id, "pspDocId");
            bool isTrue = true;
            if (_pspDocRepository.Table.Count(s => s.PspDocId == id && s.DocStatus == true) < 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public bool CheckLeastTemplate(string docNum)
        {
            bool isTrue = true;
            if (_pspDocRepository.Table.Count(s => s.DocNum == docNum && s.IsDeleted==false) > 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public virtual void DeletePspDoc(PspDoc pspDoc)
        {
            Ensure.Argument.NotNull(pspDoc, "letter");

            pspDoc.IsDeleted = true;
            UpdatePspDoc(pspDoc);
        }

        public bool IsUniquePspDocNum(int pspDocId, string docNum)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _pspDocRepository.Table.Count(l => l.DocNum == docNum && l.PspDocId != pspDocId) == 0;
        }

        public bool IsUniquePspDocVersion(int pspDocId, string docNum, string version)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _pspDocRepository.Table.Count(l => l.DocNum == docNum && l.VersionNum == version && l.PspDocId != pspDocId) == 0;
        }

        public IPagedList<PspDocSummaryView> GetPspDocSummaryViewPage(GridSettings grid)
        {
            return _pspDocSummaryViewRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
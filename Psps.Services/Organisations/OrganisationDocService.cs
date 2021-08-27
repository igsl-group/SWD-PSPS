using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class OrganisationDocService : IOrganisationDocService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOrgDocRepository _orgDocRepository;
        private readonly IOrgDocViewRepository _orgDocViewRepository;
        private readonly IOrgDocSummaryViewRepository _orgDocSummaryViewRepository;

        #endregion Fields

        #region Ctor

        public OrganisationDocService(ICacheManager cacheManager, IEventPublisher eventPublisher, IOrgDocRepository orgDocRepository, IOrgDocViewRepository orgDocViewRepository,
            IOrgDocSummaryViewRepository orgDocSummaryViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._orgDocRepository = orgDocRepository;
            this._orgDocViewRepository = orgDocViewRepository;
            this._orgDocSummaryViewRepository = orgDocSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<OrgDoc> GetPage(GridSettings grid)
        {
            return _orgDocRepository.GetPage(grid);
        }

        public virtual int GetOrgDocAmount()
        {
            return _orgDocSummaryViewRepository.Table.Count(x => x.Enabled == true);
        }

        public void CreateOrgDoc(OrgDoc model)
        {
            if (model.DocStatus)
            {
                _orgDocRepository.ChangeOtherVersionStatus(model);
            }
            _orgDocRepository.Add(model);
            _eventPublisher.EntityInserted<OrgDoc>(model);
        }

        public void UpdateOrgDoc(OrgDoc model)
        {
            if (model.DocStatus)
            {
                _orgDocRepository.ChangeOtherVersionStatus(model);
            }
            _orgDocRepository.Update(model);
            _eventPublisher.EntityUpdated<OrgDoc>(model);
        }

        public OrgDoc GetOrgDocById(int id)
        {
            return _orgDocRepository.GetById(id);
        }

        public OrgDocView GetOrgDocViewById(int id)
        {
            return _orgDocViewRepository.GetById(id);
        }

        public virtual IList<OrgDoc> GetRecordsByParam(string param)
        {
            return _orgDocRepository.GetRecordsByParam(param);
        }

        public OrgDoc GetOrgDocByVersionNum(string versionNum)
        {
            Ensure.Argument.NotNull(versionNum, "versionNum");

            return _orgDocRepository.Get(s => s.VersionNum == versionNum);
        }

        public Core.Models.IPagedList<OrgDoc> GetPage(Core.JqGrid.Models.GridSettings grid, string docNum)
        {
            return _orgDocRepository.GetPage(grid, docNum);
        }

        public bool CheckActiveTemplate(int id)
        {
            Ensure.Argument.NotNull(id, "suggestionDocId");
            bool isTrue = true;
            if (_orgDocRepository.Table.Count(s => s.OrgDocId == id && s.DocStatus == true) < 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public bool CheckLeastTemplate(string docNum)
        {
            bool isTrue = true;
            if (_orgDocRepository.Table.Count(s => s.DocNum == docNum && s.IsDeleted == false) > 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public virtual void DeleteOrgDoc(OrgDoc suggestionDoc)
        {
            Ensure.Argument.NotNull(suggestionDoc, "letter");

            suggestionDoc.IsDeleted = true;
            UpdateOrgDoc(suggestionDoc);
        }

        public bool IsUniqueOrgDocNum(int suggestionDocId, string docNum)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _orgDocRepository.Table.Count(l => l.DocNum == docNum && l.OrgDocId != suggestionDocId) == 0;
        }

        public bool IsUniqueOrgDocVersion(int suggestionDocId, string docNum, string version)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _orgDocRepository.Table.Count(l => l.DocNum == docNum && l.VersionNum == version && l.OrgDocId != suggestionDocId) == 0;
        }

        public IPagedList<OrgDocSummaryView> GetOrgDocSummaryViewPage(GridSettings grid)
        {
            return _orgDocSummaryViewRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
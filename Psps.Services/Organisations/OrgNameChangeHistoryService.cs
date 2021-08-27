using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial class OrgNameChangeHistoryService : IOrgNameChangeHistoryService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IOrgNameChangeHistoryRepository _orgNameChangeHistoryRepository;

        #endregion Fields

        #region Ctor

        public OrgNameChangeHistoryService(IEventPublisher eventPublisher, IOrgNameChangeHistoryRepository orgNameChangeHistoryRepository)
        {
            this._eventPublisher = eventPublisher;
            this._orgNameChangeHistoryRepository = orgNameChangeHistoryRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<OrgNameChangeHistory> GetPage(GridSettings grid)
        {
            return _orgNameChangeHistoryRepository.GetPage(grid);
        }

        public void CreateOrgNameChangeHistory(OrgNameChangeHistory model)
        {
            _orgNameChangeHistoryRepository.Add(model);
            _eventPublisher.EntityInserted<OrgNameChangeHistory>(model);
        }

        public int GetOrgNameChangeHistoryAmountByOrgId(string OrgId)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            return _orgNameChangeHistoryRepository.Table.Count(a => a.OrgMaster.OrgId == Convert.ToInt32(OrgId));
        }

        public OrgNameChangeHistory GetById(int Id) 
        {
            return _orgNameChangeHistoryRepository.GetById(Id);
        }

        public void Update(OrgNameChangeHistory orgNameChangeHistory)
        {
            _orgNameChangeHistoryRepository.Update(orgNameChangeHistory);
            _eventPublisher.EntityUpdated<OrgNameChangeHistory>(orgNameChangeHistory);
        }

        public void Delete(int OrgNameChangeId)
        {
            var orgNameChangeHistory = _orgNameChangeHistoryRepository.GetById(OrgNameChangeId);
            orgNameChangeHistory.IsDeleted = true;
            this.Update(orgNameChangeHistory);
        }
        
        #endregion Methods
    }
}
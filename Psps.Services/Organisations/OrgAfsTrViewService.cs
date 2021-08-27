using Psps.Core.Caching;
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

namespace Psps.Services.Organisations
{
    public partial class OrgAfsTrViewService : IOrgAfsTrViewService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOrgAfsTrViewRepository _orgAfsTrViewRepository;

        #endregion Fields

        #region Ctor

        public OrgAfsTrViewService(ICacheManager cacheManager, IEventPublisher eventPublisher, IOrgAfsTrViewRepository orgAfsTrViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._orgAfsTrViewRepository = orgAfsTrViewRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<OrgAfsTrView> GetPage(GridSettings grid)
        {
            return _orgAfsTrViewRepository.GetPage(grid);
        }

        public int GetTrViewAmt(string orgId)
        {
            //return _orgAfsTrViewRepository.Table.Where(x => x.OrgId == Convert.ToInt32(orgId) && x.TrackRecordDetails != null && x.TrackRecordDetails != "" && x.TrackRecordStartDate != null && x.TrackRecordEndDate != null).ToList().Count();
            return _orgAfsTrViewRepository.Table.Where(x => x.OrgId == Convert.ToInt32(orgId) && ((x.TrackRecordDetails != null && x.TrackRecordDetails != "") || x.TrackRecordStartDate != null || x.TrackRecordEndDate != null)).ToList().Count();
        }

        public int GetAfsViewAmt(string orgId)
        {
            //return _orgAfsTrViewRepository.Table.Where(x => x.OrgId == Convert.ToInt32(orgId) && x.AfsRecordDetails != null && x.AfsRecordDetails != "" && x.AfsRecordStartDate != null && x.AfsRecordEndDate != null).ToList().Count();
            return _orgAfsTrViewRepository.Table.Where(x => x.OrgId == Convert.ToInt32(orgId) && ((x.AfsRecordDetails != null && x.AfsRecordDetails != "") || x.AfsRecordStartDate != null || x.AfsRecordEndDate != null)).ToList().Count();
        }

        #endregion Methods
    }
}
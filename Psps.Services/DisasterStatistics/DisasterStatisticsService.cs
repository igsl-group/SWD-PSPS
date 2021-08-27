using AutoMapper;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;

using Psps.Services.Events;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.Disaster
{
    public partial class DisasterStatisticsService : IDisasterStatisticsService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IDisasterMasterRepository _disasterMasterRepository;

        private readonly IDisasterStatisticsRepository _disasterStatisticsRepository;

        #endregion Fields

        #region Ctor

        public DisasterStatisticsService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IDisasterMasterRepository disasterMasterRepository,
            IDisasterStatisticsRepository disasterStatisticsRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._disasterMasterRepository = disasterMasterRepository;
            this._disasterStatisticsRepository = disasterStatisticsRepository;
        }

        #endregion Ctor

        public IPagedList<DisasterStatistics> GetPage(GridSettings grid)
        {
            return _disasterStatisticsRepository.GetPage(grid);
        }

        public DisasterStatistics GetDisasterStatisticsById(int disasterStatisticsId)
        {
            return _disasterStatisticsRepository.Get(u => u.DisasterStatisticsId == disasterStatisticsId);
        }

        public void CreateDisasterStatistics(DisasterStatistics disasterStatistics)
        {
            Ensure.Argument.NotNull(disasterStatistics, "disasterStatistics");

            //var disasterMaster = Mapper.Map<DisasterInfoDto, DisasterMaster>(disasterInfoDto);
            Ensure.NotNull(disasterStatistics, "No disaster statistics found with the specified id");

            _disasterStatisticsRepository.Add(disasterStatistics);
            _eventPublisher.EntityInserted<DisasterStatistics>(disasterStatistics);
        }

        public void UpdateDisasterStatistics(DisasterStatistics disasterStatistics)
        {
            Ensure.Argument.NotNull(disasterStatistics, "disasterStatistics");

            _disasterStatisticsRepository.Update(disasterStatistics);
            _eventPublisher.EntityUpdated<DisasterStatistics>(disasterStatistics);
        }

        public void deleteDisasterStatisticsByMasterId(int disasterMasterId)
        {
            Ensure.Argument.NotNull(disasterMasterId, "disasterMasterId");

            var disStatRecs = _disasterStatisticsRepository.GetDisasterStatisticsByMasterId(disasterMasterId);

            foreach (DisasterStatistics ds in disStatRecs)
            {
                _disasterStatisticsRepository.Delete(ds);
                _eventPublisher.EntityDeleted<DisasterStatistics>(ds);
            }
        }

        public IDictionary<int, string> getAllDisasterStatisticsForDropdown()
        {
            string key = Constant.DISASTERSTATISTICS_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._disasterStatisticsRepository.Table
                     .OrderBy(p => p.DisasterStatisticsId)
                     .Where(p => p.IsDeleted == false)
                     .Select(p => new { Key = p.DisasterStatisticsId, Value = p.DisasterMaster.DisasterName })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public bool IsUniqueDisasterMasterId(int DisasterStatisticsId)
        {
            Ensure.Argument.NotNull(DisasterStatisticsId);

            return _disasterStatisticsRepository.Table.Count(p => p.DisasterStatisticsId == DisasterStatisticsId) == 0;
        }

        public bool GetDisasterStatisticsByPostIdRecordDate(int disasterMasterId, string postId, System.DateTime recordDate)
        {
            return !_disasterStatisticsRepository.Table.Any(u => u.DisasterMaster.DisasterMasterId == disasterMasterId && u.RecordPostId == postId && u.RecordDate.Date == recordDate.Date);
        }
    }
}
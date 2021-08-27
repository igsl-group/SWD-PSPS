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
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.Disaster
{
    public partial class DisasterMasterService : IDisasterMasterService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IDisasterMasterRepository _disasterMasterRepository;

        #endregion Fields

        #region Ctor

        public DisasterMasterService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IDisasterMasterRepository DisasterMasterRepository,
            IDisasterStatisticsRepository DisasterStatisticsRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._disasterMasterRepository = DisasterMasterRepository;
        }

        #endregion Ctor

        public IPagedList<DisasterMaster> GetPage(GridSettings grid)
        {
            return _disasterMasterRepository.GetPage(grid);
        }

        public IPagedList<DisasterInfoDto> GetPageWithDto(GridSettings grid)
        {
            return _disasterMasterRepository.GetPageWithDto(grid);
        }

        public DisasterMaster GetDisasterMasterById(int disasterMasterId)
        {
            return _disasterMasterRepository.Get(u => u.DisasterMasterId == disasterMasterId);
        }

        public void CreateDisasterMaster(DisasterMaster disasterMaster)
        {
            Ensure.Argument.NotNull(disasterMaster, "disasterMaster");

            //var disasterMaster = Mapper.Map<DisasterInfoDto, DisasterMaster>(disasterInfoDto);
            Ensure.NotNull(disasterMaster, "No disaster master found with the specified id");

            _disasterMasterRepository.Add(disasterMaster);
            _eventPublisher.EntityInserted<DisasterMaster>(disasterMaster);
        }

        public void delete(DisasterMaster disasterMaster)
        {
            Ensure.Argument.NotNull(disasterMaster, "disasterMaster");

            //var disasterMaster = Mapper.Map<DisasterInfoDto, DisasterMaster>(disasterInfoDto);
            Ensure.NotNull(disasterMaster, "No disaster master found with the specified id");

            _disasterMasterRepository.Delete(disasterMaster);
            _eventPublisher.EntityDeleted<DisasterMaster>(disasterMaster);
        }

        public void UpdateDisasterMaster(DisasterMaster disasterMaster)
        {
            Ensure.Argument.NotNull(disasterMaster, "disasterMaster");

            _disasterMasterRepository.Update(disasterMaster);
            _eventPublisher.EntityUpdated<DisasterMaster>(disasterMaster);
        }

        public IDictionary<int, string> GetAllDisasterMasterForDropdown()
        {
            string key = Constant.DISASTERMASTER_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._disasterMasterRepository.Table
                     .OrderByDescending(p => p.BeginDate)
                     .Where(p => p.IsDeleted == false)
                     .Select(p => new { Key = p.DisasterMasterId, Value = p.DisasterName })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<int, string> GetAllDisasterMasterBySysDate()
        {
            string key = Constant.DISASTERMASTER_FOR_DROWDROP_GTSYSDATE_KEY;

            return _cacheManager.Get(key, () =>
            {
                return this._disasterMasterRepository.Table
                     .Where(p => p.IsDeleted == false && (p.EndDate >= DateTime.Today || p.EndDate == null))
                     .OrderByDescending(p => p.BeginDate)
                     .Select(p => new { Key = p.DisasterMasterId, Value = p.DisasterName })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        //public IDictionary<int, string> GetAllDisasterForReportDropdown()
        //{
        //    string key = Constant.DISASTERMASTER_FOR_DROWDROP_NAME;
        //    return _cacheManager.Get(key, () =>
        //    {
        //        return this._disasterMasterRepository.Table
        //             .OrderBy(p => p.DisasterMasterId)
        //             .Where(p => p.IsDeleted == false)
        //             .Select(p => new { Key = p.DisasterMasterId, Value = p.DisasterName })
        //             .ToDictionary(k => k.Key, v => v.Value);
        //    });
        //}

        public bool IsUniqueDisasterMasterId(int disasterMasterId)
        {
            Ensure.Argument.NotNull(disasterMasterId);

            return _disasterMasterRepository.Table.Count(p => p.DisasterMasterId == disasterMasterId) == 0;
        }

        public bool IsUniqueDisasterName(string disasterName)
        {
            Ensure.Argument.NotNull(disasterName);
            return _disasterMasterRepository.Table.Count(p => p.DisasterName == disasterName && p.IsDeleted == false) == 0;
        }

        public bool IsUniqueDisasterName(int disasterMasterId, string disasterName)
        {
            Ensure.Argument.NotNull(disasterMasterId);
            Ensure.Argument.NotNull(disasterName);
            return _disasterMasterRepository.Table.Count(p => p.DisasterMasterId != disasterMasterId && p.DisasterName == disasterName && p.IsDeleted == false) == 0;
        }
    }
}
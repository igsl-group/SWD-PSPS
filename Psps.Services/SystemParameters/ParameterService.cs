using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.SystemParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.SystemParameters
{
    public partial class ParameterService : IParameterService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly ISystemParameterRepository _systemParameterRepository;

        #endregion Fields

        #region Ctor

        public ParameterService(ICacheManager cacheManager, IEventPublisher eventPublisher, ISystemParameterRepository systemParameterRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._systemParameterRepository = systemParameterRepository;
        }

        #endregion Ctor

        #region Methods

        public SystemParameter GetParameterById(int systemParameterId)
        {
            if (systemParameterId == 0)
                return null;

            return _systemParameterRepository.GetById(systemParameterId);
        }

        public SystemParameter GetParameterByCode(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code, "code");

            return _systemParameterRepository.Get(u => u.Code == code);
        }

        public void UpdateParameter(SystemParameter systemParameter)
        {
            Ensure.Argument.NotNull(systemParameter, "systemParameter");

            _systemParameterRepository.Update(systemParameter);

            //cache
            _cacheManager.RemoveByPattern(Constant.SYSTEMPARAMETER_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated<SystemParameter>(systemParameter);
        }

        public Core.Models.IPagedList<SystemParameter> GetPage(Core.JqGrid.Models.GridSettings grid)
        {
            return _systemParameterRepository.GetPage(grid);
        }

        #endregion Methods
    }
}
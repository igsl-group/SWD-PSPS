using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Events;
using Psps.Models.Domain;
using Psps.Services.Events;

namespace Psps.Web.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        //lookups
        IConsumer<EntityInserted<Lookup>>,
        IConsumer<EntityUpdated<Lookup>>,
        IConsumer<EntityDeleted<Lookup>>,
        //disaster master
        IConsumer<EntityInserted<DisasterMaster>>,
        IConsumer<EntityUpdated<DisasterMaster>>,
        IConsumer<EntityDeleted<DisasterMaster>>,
        //PspEvent
        IConsumer<EntityInserted<PspEvent>>,
        IConsumer<EntityUpdated<PspEvent>>,
        IConsumer<EntityDeleted<PspEvent>>
    {
        private readonly ICacheManager _cacheManager;

        public ModelCacheEventConsumer(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #region Lookup

        /// <summary>
        /// Key for lookup type
        /// </summary>
        /// <remarks>{0} : lookup type</remarks>

        public void HandleEvent(EntityInserted<Lookup> eventMessage)
        {
            _cacheManager.RemoveByPattern(Constant.LOOKUP_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<Lookup> eventMessage)
        {
            _cacheManager.RemoveByPattern(Constant.LOOKUP_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<Lookup> eventMessage)
        {
            _cacheManager.RemoveByPattern(Constant.LOOKUP_PATTERN_KEY);
        }

        #endregion Lookup

        #region DisasterMaster

        public void HandleEvent(EntityInserted<DisasterMaster> eventMessage)
        {
            _cacheManager.RemoveByPattern(Constant.LOOKUP_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<DisasterMaster> eventMessage)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(EntityDeleted<DisasterMaster> eventMessage)
        {
            throw new System.NotImplementedException();
        }

        #endregion DisasterMaster

        #region PspEvent

        public void HandleEvent(EntityInserted<PspEvent> eventMessage)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(EntityUpdated<PspEvent> eventMessage)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(EntityDeleted<PspEvent> eventMessage)
        {
            throw new System.NotImplementedException();
        }

        #endregion PspEvent
    }
}
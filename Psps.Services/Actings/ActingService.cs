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

namespace Psps.Services.Actings
{
    /// <summary>
    /// Default implementation of Acting service interface
    /// </summary>
    public partial class ActingService : IActingService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string LOOKUP_ALL_KEY = "Psps.Acting.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LOOKUP_PATTERN_KEY = "Psps.Acting.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string Acting_FOR_DROWDROP_KEY = "Psps.Acting.dropdown.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string Acting_FOR_DROWDROP_PATTERN_KEY = "Psps.Acting.dropdown.";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IActingRepository _ActingRepository;

        #endregion Fields

        #region Ctor

        public ActingService(ICacheManager cacheManager, IEventPublisher eventPublisher, IActingRepository ActingRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._ActingRepository = ActingRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual void DeleteActing(Acting acting)
        {
            Ensure.Argument.NotNull(acting, "Acting");

            acting.IsDeleted = true;
            UpdateActing(acting);
        }

        public virtual IPagedList<Acting> GetPage(GridSettings grid)
        {
            return _ActingRepository.GetPage(grid);
        }

        public virtual void CreateActing(Acting Acting)
        {
            Ensure.Argument.NotNull(Acting, "Acting");

            _ActingRepository.Add(Acting);

            //event notification
            _eventPublisher.EntityInserted<Acting>(Acting);
        }

        public virtual void UpdateActing(Acting Acting)
        {
            Ensure.Argument.NotNull(Acting, "Acting");

            _ActingRepository.Update(Acting);

            //event notification
            _eventPublisher.EntityUpdated<Acting>(Acting);
        }

        public virtual Acting GetActingById(int actingId)
        {
            return _ActingRepository.GetById(actingId);
        }

        /// <summary>
        /// Gets all Actings for dropdown
        /// </summary>
        /// <returns>Actings</returns>
        //public IDictionary<string, string> GetAllActingsForDropdown()
        //{
        //    string key = Acting_FOR_DROWDROP_KEY;
        //    return _cacheManager.Get(key, () =>
        //    {
        //        return this._ActingRepository.Table
        //            .OrderBy(p => p.User.UserId)
        //            .Where(p => p.User.IsActive == true)
        //            .Select(p => new { Key = p.User.UserId, Value = p.User.UserId + "-" + p.User.EngUserName })
        //            .ToDictionary(k => k.Key, v => v.Value);
        //    });
        //}

        public IList<Acting> GetActingsByUserId(string userId)
        {
            Ensure.Argument.NotNull(userId, "userId");
            return _ActingRepository.Table.Where(x => x.User.UserId == userId).ToList();
        }

        public bool ValidateIsAssigned(int actingId, string postId, DateTime fromDate, DateTime toDate)
        {
            Ensure.Argument.NotNull(actingId, "actingtId");
            Ensure.Argument.NotNullOrEmpty(postId, "postId");
            Ensure.Argument.NotNull(fromDate, "fromDate");
            Ensure.Argument.NotNull(toDate, "toDate");

            return this._ActingRepository.Table
                .Any(p => p.ActingId != actingId && p.PostToBeActed.PostId == postId && ((p.EffectiveFrom >= fromDate && p.EffectiveFrom <= toDate) || (p.EffectiveTo >= fromDate && p.EffectiveTo <= toDate)));
        }

        #endregion Methods
    }
}
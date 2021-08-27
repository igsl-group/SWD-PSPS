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

namespace Psps.Services.Ranks
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class RankService : IRankService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string LOOKUP_ALL_KEY = "Psps.rank.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LOOKUP_PATTERN_KEY = "Psps.rank.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string POST_FOR_DROWDROP_KEY = "Psps.rank.dropdown.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string POST_FOR_DROWDROP_PATTERN_KEY = "Psps.rank.dropdown.";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IRankRepository _rankRepository;

        #endregion Fields

        #region Ctor

        public RankService(ICacheManager cacheManager, IEventPublisher eventPublisher, IRankRepository rankRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._rankRepository = rankRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual void DeleteRank(Rank rank)
        {
            Ensure.Argument.NotNull(rank, "rank");

            rank.IsDeleted = true;
            UpdateRank(rank);
        }

        public virtual IPagedList<Rank> GetPage(GridSettings grid)
        {
            return _rankRepository.GetPage(grid);
        }

        public virtual void CreateRank(Rank rank)
        {
            Ensure.Argument.NotNull(rank, "rank");

            _rankRepository.Add(rank);

            //event notification
            _eventPublisher.EntityInserted<Rank>(rank);
        }

        public virtual void UpdateRank(Rank rank)
        {
            Ensure.Argument.NotNull(rank, "rank");

            _rankRepository.Update(rank);

            //event notification
            _eventPublisher.EntityUpdated<Rank>(rank);
        }

        public virtual Rank GetRankById(string rankId)
        {


            return _rankRepository.GetById(rankId);
        }


        public bool IsUniqueRankId(string rankId)
        {
            Ensure.Argument.NotNullOrEmpty(rankId);

            return _rankRepository.Table.Count(l => l.RankId == rankId) == 0;

        }

        /// <summary>
        /// Gets all ranks for dropdown
        /// </summary>
        /// <returns>Ranks</returns>
        public IDictionary<string, string> GetAllRanksForDropdown()
        {
            string key = POST_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._rankRepository.Table
                    .OrderBy(p => p.RankId)
                    .Select(p => new { Key = p.RankId, Value = p.RankId })
                    .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        #endregion Methods
    }
}
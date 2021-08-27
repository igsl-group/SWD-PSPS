using Psps.Core;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Ranks
{
    /// <summary>
    /// Rank service interface
    /// </summary>
    public partial interface IRankService
    {
        /// <summary>
        /// Marks rank as deleted
        /// </summary>
        /// <param name="rank">Rank</param>
        void DeleteRank(Rank rank);

        /// <summary>
        /// List ranks by type
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Ranks</returns>
        IPagedList<Rank> GetPage(GridSettings grid);

        /// <summary>
        /// Inserts a rank
        /// </summary>
        /// <param name="rank">Rank</param>
        void CreateRank(Rank rank);

        /// <summary>
        /// Updates a rank
        /// </summary>
        /// <param name="rank">Rank</param>
        void UpdateRank(Rank rank);

        /// <summary>
        /// Gets a rank
        /// </summary>
        /// <param name="taskId">Rank identifier</param>
        /// <returns>Rank</returns>
        Rank GetRankById(string rankId);

        IDictionary<string, string> GetAllRanksForDropdown();
        

        bool IsUniqueRankId(string rankId);
    }
}
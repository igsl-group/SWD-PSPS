using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;
using Psps.Models.Dto.Security;
using System.Collections.Generic;
using System;

namespace Psps.Services.Disaster
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IDisasterStatisticsService
    {
        #region DisasterStatistics

        /// <summary>
        /// Gets a disasters
        /// </summary>
        /// <param name="disasterMasterId">Disaster Master Id</param>
        /// <param name="postId">Post Id</param>
        /// <param name="recordDate">Record Date</param>
        /// <returns>A disaster</returns>
        bool GetDisasterStatisticsByPostIdRecordDate(int disasterMasterId, string postId, DateTime recordDate);

        /// <summary>
        /// Gets a disasters
        /// </summary>
        /// <param name="disasterId">user identifier</param>
        /// <returns>A disaster</returns>
        DisasterStatistics GetDisasterStatisticsById(int disasterStatisticsId);

        /// <summary>
        /// Create a disaster
        /// </summary>
        /// <param name="disaster">Disaster</param>
        void CreateDisasterStatistics(DisasterStatistics disasterStatistics);

        /// <summary>
        /// Updates the disaster
        /// </summary>
        /// <param name="disaster">Disaster</param>
        void UpdateDisasterStatistics(DisasterStatistics disasterStatistics);

        /// <summary>
        /// List disasters
        /// </summary>
        /// <param name="type">Lookup Type</param>
        /// <returns>Lookups</returns>
        IPagedList<DisasterStatistics> GetPage(GridSettings grid);

        /// <summary>
        /// Gets all users for dropdown
        /// </summary>
        /// <returns>Users</returns>
        IDictionary<int, string> getAllDisasterStatisticsForDropdown();

        void deleteDisasterStatisticsByMasterId(int disasterMasterId);

        #endregion DisasterStatistics
    }
}
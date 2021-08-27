using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;
using Psps.Models.Dto.Security;
using System.Collections.Generic;

namespace Psps.Services.Disaster
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IDisasterMasterService
    {
        #region Disaster

        /// <summary>
        /// Gets a disasters
        /// </summary>
        /// <param name="disasterId">user identifier</param>
        /// <returns>A disaster</returns>
        DisasterMaster GetDisasterMasterById(int disasterId);

        /// <summary>
        /// Create a disaster
        /// </summary>
        /// <param name="disaster">Disaster</param>
        void CreateDisasterMaster(DisasterMaster disasterMaster);

        /// <summary>
        /// Updates the disaster
        /// </summary>
        /// <param name="disaster">Disaster</param>
        void UpdateDisasterMaster(DisasterMaster disasterMaster);

        /// <summary>
        /// delete disaster master
        /// </summary>
        /// <param name="disasterMaster"></param>
        void delete(DisasterMaster disasterMaster);

        /// <summary>
        /// List disasters
        /// </summary>
        /// <param name="type">Lookup Type</param>
        /// <returns>Lookups</returns>
        IPagedList<DisasterMaster> GetPage(GridSettings grid);

        /// <summary>
        /// List disasters
        /// </summary>
        /// <param name="type">Lookup Type</param>
        /// <returns>Lookups</returns>
        IPagedList<DisasterInfoDto> GetPageWithDto(GridSettings grid);

        /// <summary>
        /// Gets all users for dropdown
        /// </summary>
        /// <returns>Users</returns>
        IDictionary<int, string> GetAllDisasterMasterForDropdown();

        IDictionary<int, string> GetAllDisasterMasterBySysDate();

        //IDictionary<int, string> GetAllDisasterForReportDropdown();

        bool IsUniqueDisasterMasterId(int disasterMasterId);

        bool IsUniqueDisasterName(string disasterName);

        bool IsUniqueDisasterName(int disasterMasterId, string disasterName);

        #endregion Disaster
    }
}
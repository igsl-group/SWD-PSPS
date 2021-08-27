using Psps.Core;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Lookups
{
    /// <summary>
    /// Lookup service interface
    /// </summary>
    public partial interface ILookupService
    {
        /// <summary>
        /// Marks lookup as deleted
        /// </summary>
        /// <param name="lookup">Lookup</param>
        void DeleteLookup(Lookup lookup);

        /// <summary>
        /// List lookups by type
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Lookups</returns>
        IPagedList<Lookup> GetPage(String lookupType, GridSettings grid);

        /// <summary>
        /// Inserts a lookup
        /// </summary>
        /// <param name="lookup">Lookup</param>
        void CreateLookup(Lookup lookup);

        /// <summary>
        /// Updates a lookup
        /// </summary>
        /// <param name="lookup">Lookup</param>
        void UpdateLookup(Lookup lookup);

        /// <summary>
        /// Gets a lookup
        /// </summary>
        /// <param name="taskId">Lookup identifier</param>
        /// <returns>Lookup</returns>
        Lookup GetLookupById(int lookupId);

        /// <summary>
        /// Determine the uniqueness of given type and code
        /// </summary>
        /// <param name="lookupType">Lookup type</param>
        /// <param name="lookupId">Lookup Id</param>
        /// <param name="lookupCode">Lookup code</param>
        /// <returns>Whether the given code is unique within the same type</returns>
        bool IsUniqueLookupCode(LookupType lookupType, int lookupId, string lookupCode);

        /// <summary>
        /// Get maximum of display order of same Type + 1
        /// </summary>
        /// <param name="lookupType">Lookup type</param>
        /// <returns>default display order of given type</returns>
        int GetDefaultDisplayOrder(LookupType lookupType);

        /// <summary>
        /// Determine the uniqueness of given type and display order
        /// </summary>
        /// <param name="lookupType">Lookup type</param>
        /// <param name="lookupId">Lookup Id</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Whether the given display order is unique within the same type</returns>
        bool IsUniqueDisplayOrder(LookupType lookupType, int lookupId, int displayOrder);

        /// <summary>
        /// Gets all Lookups for dropdown
        /// </summary>
        /// <returns>code , Eng Description</returns>
        IDictionary<string, string> getAllLookupForDropdownByType(LookupType type);

        /// <summary>
        /// Gets all Lookups for dropdown
        /// </summary>
        /// <returns>code ,chi Description</returns>
        IDictionary<string, string> getAllLookupChiDescForDropdownByType(LookupType type);

        /// <summary>
        /// Gets a lookup and type
        /// </summary>
        /// <param name="code,type">string</param>
        /// <returns>Lookup</returns>
        IDictionary<string, string> GetAllLookupByType(LookupType type);

        /// <summary>
        /// Get lookup list by type
        /// </summary>
        /// <param name="type">string</param>
        /// <returns>Lookups</returns>
        IList<Lookup> GetAllLookupListByType(LookupType type);

        /// <summary>
        /// Get lookup list by type
        /// </summary>
        /// <param name="type">string</param>
        /// <returns>Lookups</returns>
        IDictionary<string, string> getAllLkpInCodec(LookupType type);

        /// <summary>
        /// Get lookup list by type
        /// </summary>
        /// <param name="type">string</param>
        /// <returns>Lookups</returns>
        IDictionary<string, string> getAllLkpInChiCodec(LookupType type);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDictionary<string, string> getAllLkpInPlainDec(LookupType type);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsMaxDisplayOrder(LookupType lookupType, string code, int displayOrder);

        /// <summary>
        /// </summary>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        bool IsSolicitDate(DateTime eventDate);

        /// <summary>
        /// </summary>
        /// <param name="eveStartDt"></param>
        /// <param name="eveEndDt"></param>
        /// <returns></returns>
        bool EveRngChk(DateTime eveStartDt, DateTime eveEndDt);

        /// <summary>
        /// Get cached lookup description by type and code
        /// </summary>
        /// <param name="lookupType">lookup type</param>
        /// <param name="code">code</param>
        /// <param name="defaultValue">default value if lookup not found</param>
        /// <returns>eng description of selected lookup</returns>
        string GetDescription(LookupType lookupType, string code, string defaultValue = "");
    }
}
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial interface IFlagDayListService
    {/// <summary>
        /// Get system message by Id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        FdList GetFlagDayListById(int Id);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<FdList> GetPage(GridSettings grid);

        /// <summary>
        /// Update a FdMaster
        /// </summary>
        /// <param name="model">FdMaster</param>
        void UpdateFlagDayList(FdList model);

        IDictionary<string, string> GetAllFlagDayListTypeForDropdown();

        IDictionary<string, string> GetAllFlagDayListYearForDropdown();

        /// <summary>
        /// Create a FdMaster
        /// </summary>
        /// <param name="model">FdMaster</param>
        void CreateFlagDayList(FdList model);

        bool IsUniqueFlagDayDate(DateTime FlagDayDate, int? FlagDayListId);

        string InsertFlagDayListByImportXls(Stream xlsxStream);

        bool MatchFlagDayInFdList(DateTime flagday, string type, string FdYear);

        FdList GetFdListByDateAndType(DateTime? FlagDayDate, string FlagDayType);

        void DeleteFlagDayList(int Id);

        bool EveRngChk(DateTime eveStartDt, string startTime, DateTime eveEndDt, string endTime);

        /// <summary>
        ///
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        bool IsFdDateAndTimeConflicit(DateTime startDate, DateTime endDate, string startTime, string endTime);
    }
}
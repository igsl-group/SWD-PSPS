using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;

namespace Psps.Services.UserLog
{
    /// <summary>
    /// System message service interface
    /// </summary>
    public partial interface IUserLogService
    {
        IDictionary<String, String> GetActions();

        ActivityLog LogOrganisationInformation(OrgMaster oldOrgMaster, OrgMaster newOrgMaster);

        ActivityLog LogFlagDayInformation(FdMaster oldFdMaster, FdMaster newFdMaster);

        ActivityLog LogPSPInformation(PspMaster oldPspMaster, PspMaster newPspMaster);

        ActivityLog LogComplaintInformation(ComplaintMaster oldComplaintMaster, ComplaintMaster newComplaintMaster);

        ActivityLog LogSuggestionMasterInformation(SuggestionMaster oldSuggestionMaster, SuggestionMaster newSuggestionMaster);

        ActivityLog LogLoginInformation(int Mode);

        ActivityLog LogLoginWrongPassword(string UserId,string IPAddress);

        int GetInvalidLoginAttemps(string UserId);

        /// <summary>
        /// Get system message by id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        ActivityLog GetUserLogById(int userlogId);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<ActivityLog> GetPage(GridSettings grid);
    }
}
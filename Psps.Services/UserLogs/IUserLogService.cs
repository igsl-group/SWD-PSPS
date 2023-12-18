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

      

       

        ActivityLog LogLoginInformation(int Mode);

        ActivityLog LogLoginWrongPassword(string UserId,string IPAddress);

        ActivityLog LogAccountLockedByInvalidAttemps(string IPAddress, int Attempts, int AttemptsLimit);

        ActivityLog LogChangePasswordAttempt(string IPAddress, string Message = "", bool Changed = false);

        ActivityLog LogCRUDUser(string UserId, string IPAddress, List<string> LogCodeList);

        ActivityLog LogCRUDActing(string Mode, string IPAddress, string ActingId, string AssignTo = "", string AssignedPost = "");

        ActivityLog LogCRUDPost(string Mode, string IPAddress, string PostId, string DataStr, List<string> LogCodeList);

        ActivityLog LogCRUDRole(string Mode, string IPAddress, string RoleId, string DataStr);

        ActivityLog LogCRUDDocumentLibrary(string Mode, string DocumentLibraryId, string IPAddress);

        ActivityLog LogCRUDDocument(string Mode, string DocumentId, string IPAddress);



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

        void LogToFile(string msg);
    }
}
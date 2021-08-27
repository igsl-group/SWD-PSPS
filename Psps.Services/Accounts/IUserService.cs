using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Security;
using System.Collections.Generic;

namespace Psps.Services.Accounts
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IUserService
    {
        #region Users

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="UserId">user identifier</param>
        /// <returns>A user</returns>
        User GetUserById(string userId);

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user">User</param>
        void CreateUser(UserInfoDto userInfoDto);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        void UpdateUser(UserInfoDto userInfoDto);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        void Update(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        void ChangePassword(string userId, string password);

        /// <summary>
        /// Check if password exist in password history
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns></returns>
        bool ExistsInPasswordHistory(string userId, string password);

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="userId">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        UserLoginResults ValidateUser(string userId, string password);

        /// <summary>
        /// List users
        /// </summary>
        /// <param name="type">Lookup Type</param>
        /// <returns>Lookups</returns>
        IPagedList<User> GetPage(GridSettings grid);

        /// <summary>
        /// Gets all users for dropdown
        /// </summary>
        /// <returns>Users</returns>
        IDictionary<string, string> getAllUserForDropdown();

        #endregion Users
    }
}
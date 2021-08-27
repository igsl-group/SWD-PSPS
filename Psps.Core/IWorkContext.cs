using Psps.Core.Models;

namespace Psps.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        IUserInfo CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the current user's roles
        /// </summary>
        string[] CurrentUserRoles { get; set; }
    }
}
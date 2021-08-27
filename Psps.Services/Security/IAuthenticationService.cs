using Psps.Core.Models;
using Psps.Models.Domain;

namespace Psps.Services.Security
{
    /// <summary>
    /// Security service interface
    /// </summary>
    public partial interface IAuthenticationService
    {
        IUserInfo GetAuthenticatedUser();

        void SignIn(string userId);

        void SignOut();

        void ChangePost(string userId, string postId);
    }
}
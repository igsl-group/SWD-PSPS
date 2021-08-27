using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Psps.Services.Security
{
    /// <summary>
    /// Security service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private IUserInfo _cachedUser;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP _dataContext</param>
        /// <param name="userService">User service</param>
        /// <param name="userRepository">User repository</param>
        public FormsAuthenticationService(HttpContextBase httpContext,
            IUserService userService, IUserRepository userRepository)
        {
            this._httpContext = httpContext;
            this._userService = userService;
            this._userRepository = userRepository;
        }

        public virtual IUserInfo GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is IPspsUser))
            {
                return null;
            }

            var pspsUser = _httpContext.User.Identity as IPspsUser;
            if (pspsUser != null)
                _cachedUser = new UserInfo
                {
                    Name = pspsUser.Name,
                    UserId = pspsUser.UserId,
                    PostId = pspsUser.PostId,
                    OriginalPostIdIfActed = pspsUser.OriginalPostIdIfActed,
                    IsSysAdmin = pspsUser.IsSysAdmin
                };
            return _cachedUser;
        }

        public virtual void SignIn(string userId)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");

            var now = DateTime.Now;
            var user = this._userRepository.GetUserAndPostById(userId);

            var userContext = new UserInfo
             {
                 Name = user.EngUserName,
                 UserId = user.UserId,
                 PostId = user.Post.PostId,
                 PasswordChangedDate = user.PasswordChangedDate,
                 IsSysAdmin = user.IsSystemAdministrator
             };

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.UserId,
                issueDate: now,
                expiration: now.Add(FormsAuthentication.Timeout),
                isPersistent: false,
                userData: userContext.ToString(),
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
                cookie.Domain = FormsAuthentication.CookieDomain;

            _httpContext.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = userContext;
        }

        public virtual void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
        }

        public void ChangePost(string userId, string postId)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");
            Ensure.Argument.NotNullOrEmpty(postId, "postId");

            var now = DateTime.Now;
            var user = this._userRepository.GetUserAndPostById(userId);

            var userContext = new UserInfo
            {
                Name = user.EngUserName,
                UserId = user.UserId,
                PostId = postId,
                OriginalPostIdIfActed = postId.Equals(user.Post.PostId) ? null : user.Post.PostId,
                IsSysAdmin = user.IsSystemAdministrator
            };

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.UserId,
                issueDate: now,
                expiration: now.Add(FormsAuthentication.Timeout),
                isPersistent: false,
                userData: userContext.ToString(),
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
                cookie.Domain = FormsAuthentication.CookieDomain;

            _httpContext.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = userContext;
            EngineContext.Current.Resolve<IWorkContext>().CurrentUser = null;
        }
    }
}
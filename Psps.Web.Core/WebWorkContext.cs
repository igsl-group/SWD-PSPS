using Psps.Core;
using Psps.Core.Fakes;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using Psps.Services.Security;
using System;
using System.Web;

namespace Psps.Web.Framework
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string UserCookieName = "Psps.user";

        #endregion Const

        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private IUserInfo _cachedUser;
        private string[] _roles;
        //private IUserInfo _originalUserIfActed;

        #endregion Fields

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IUserService userService,
            IAuthenticationService authenticationService)
        {
            this._httpContext = httpContext;
            this._userService = userService;
            this._authenticationService = authenticationService;
        }

        #endregion Ctor

        #region Utilities

        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        protected virtual void SetUserCookie(string userId)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = userId;
                if (string.IsNullOrEmpty(userId))
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion Utilities

        #region Properties

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual IUserInfo CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                IUserInfo userInfo = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //check whether request is made by a background task
                    //in this case return built-in user record for background task
                    userInfo = new UserInfo()
                    {
                        Name = "BackgroundTask",
                        UserId = "BackgroundTask",
                        PostId = "BackgroundTask"
                    };
                }

                //authenticated user
                if (userInfo == null)
                {
                    userInfo = _authenticationService.GetAuthenticatedUser();
                }

                //validation
                if (userInfo != null)
                {
                    //SetUserCookie(userInfo.UserId);
                    _cachedUser = userInfo;
                }

                return _cachedUser;
            }
            set
            {
                //SetUserCookie(value.UserId);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets or sets the current user's roles
        /// </summary>
        public virtual string[] CurrentUserRoles
        {
            get
            {
                if (_roles != null)
                    return _roles;

                return _roles;
            }

            set { _roles = value; }
        }

        #endregion Properties
    }
}
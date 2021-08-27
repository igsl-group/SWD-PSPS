using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.Models;
using Psps.Models.Dto.Security;
using Psps.Services.Security;
using Psps.Web.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Psps.Web.Core.ActionFilters
{
    public enum Allow
    {
        SysAdmin,
        AccessAdmin,
        DisasterMaster,
        AdminFunction,
        Lookup,
        DataExport,
        UserLog,
        OrgMaster,
        FlagDayList,
        PspACSummary,
        FdACSummary,
        ReferenceGuide,
        OrgTemplate,
        OrgReport,
        PspMaster,
        PspApprove,
        PspTemplate,
        PspReport,
        FdMaster,
        FdApprove,
        FdTemplate,
        FdReport,
        ComplaintMaster,
        DisasterStat,
        ComplaintTemplate,
        ComplaintReport,
        SuggestionMaster,
        SuggestionTemplate,
        SuggestionReport,
        LegalAdvice,
        LegalAdviceReport,
        DocLib
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PspsAuthorizeAttribute : AuthorizeAttribute
    {
        private bool _bypassCheckingForSysAdmin = false;

        public PspsAuthorizeAttribute(params Allow[] allows)
            : this(true, allows)
        {
        }

        public PspsAuthorizeAttribute(bool bypassCheckingForSysAdmin, params Allow[] allows)
        {
            this._bypassCheckingForSysAdmin = bypassCheckingForSysAdmin;
            this.Roles = String.Join(", ", allows.Select(x => x.GetName()));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Ensure.Argument.NotNull(httpContext, "httpContext");

            //LoadCustomAuth(httpContext);

            IPspsUser user = httpContext.User.GetPspsUser();
            if (user != null)
            {
                if (!user.IsAuthenticated)
                    return false;

                if (this._bypassCheckingForSysAdmin && user.IsSysAdmin)
                    return true;
            }

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var user = httpContext.User;

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                if (user.Identity.IsAuthenticated == false)
                    filterContext.Result = new Ajax401Response(); // return 401 - unauthorised
                else
                    filterContext.Result = new Ajax403Response(); // return 4031 - forbidden
            }
            else
            {
                if (user.Identity.IsAuthenticated == false)
                    base.HandleUnauthorizedRequest(filterContext);
                //throw new HttpException((int)HttpStatusCode.Unauthorized, "Your session has expired. Please login again to continue.");
                else
                    throw new HttpException((int)HttpStatusCode.Forbidden, "You do not have permission to perform this action.");
            }
        }

        #region Load Custom Auth Cookie

        private void LoadCustomAuth(HttpContextBase httpContext)
        {
            HttpCookie authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = EngineContext.Current.Resolve<IFormsAuthentication>();
                var aclService = EngineContext.Current.Resolve<IAclService>();

                FormsAuthenticationTicket ticket = formsAuthentication.Decrypt(authCookie.Value);
                IPspsUser user = new PspsUser(ticket.Name, ticket.UserData);
                Psps.Models.Domain.User dbUser = null;
                List<string> allowedFunctions = new List<string>();

                if (aclService.ValidateUserIdAndPostId(user.UserId, user.PostId, out dbUser))
                {
                    allowedFunctions = aclService.GetAllowedFunctionsByPost(user.PostId);
                    if (dbUser.IsSystemAdministrator)
                        allowedFunctions.Add(Psps.Web.Core.ActionFilters.Allow.SysAdmin.GetName());

                    var newTicket = UpdateAuthInfo(ticket, user, dbUser);
                    httpContext.User = new GenericPrincipal(user, allowedFunctions.ToArray());
                    formsAuthentication.SetAuthCookie(httpContext, newTicket);
                }
            }
        }

        private bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }

        private DateTime CalculateTicketExpirationDate()
        {
            return DateTime.Now.Add(FormsAuthentication.Timeout);
        }

        private FormsAuthenticationTicket UpdateAuthInfo(FormsAuthenticationTicket ticket, IPspsUser user, Psps.Models.Domain.User dbUser)
        {
            user.Name = dbUser.EngUserName;
            user.IsSysAdmin = dbUser.IsSystemAdministrator;

            return new FormsAuthenticationTicket(ticket.Version,
                                              ticket.Name,
                                              ticket.IssueDate,
                                              CalculateTicketExpirationDate(),
                                              ticket.IsPersistent,
                                              user.GetUserInfo().ToString(),
                                              ticket.CookiePath);
        }

        #endregion Load Custom Auth Cookie

        private class Ajax401Response : ActionResult
        {
            // Called by the MVC framework to run the action result using the specified controller context
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 401; // The request requires user authentication
                context.HttpContext.Response.TrySkipIisCustomErrors = true;
                context.HttpContext.Response.Write("Your session has expired. Please login again to continue."); // HTTP response
                context.HttpContext.Response.End();
            }
        }

        private class Ajax403Response : ActionResult
        {
            // Called by the MVC framework to run the action result using the specified controller context
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 403; // The request requires user authentication
                context.HttpContext.Response.TrySkipIisCustomErrors = true;
                context.HttpContext.Response.Write("You do not have permission to perform this action."); // HTTP response
                context.HttpContext.Response.End();
            }
        }
    }
}
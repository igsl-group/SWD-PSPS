using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using System;
using System.Web.Mvc;

namespace Psps.Web.Core.ActionFilters
{
    public class PasswordExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
            IMessageService messageService = EngineContext.Current.Resolve<IMessageService>();

            var user = authenticationService.GetAuthenticatedUser();

            if (user != null && (user.PasswordChangedDate == null || user.PasswordChangedDate.Value.AddDays(90) < DateTime.Now))
            {
                authenticationService.SignOut();
                filterContext.Controller.TempData["UserId"] = user.UserId;
                filterContext.Result = new RedirectToRouteResult("PasswordExpired", null);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
using System.Web.Mvc;

namespace Psps.Web.Core.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Home");
        }

        public static string Logout(this UrlHelper urlHelper, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.RouteUrl("Logout", new { ReturnUrl = returnUrl });
            return urlHelper.RouteUrl("Logout");
        }

        public static string Login(this UrlHelper urlHelper, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.RouteUrl("Login", new { ReturnUrl = returnUrl });
            return urlHelper.RouteUrl("Login");
        }
    }
}
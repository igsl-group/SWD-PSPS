using MvcRefApp.Web.Core.ActionFilters;
using Psps.Web.Core.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            FilterProviders.Providers.Add(new AntiForgeryTokenFilterProvider());
            filters.Add(new CompressResponseAttribute());
            //filters.Add(new PasswordExpireAttribute());

            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new UserFilter());
        }
    }
}
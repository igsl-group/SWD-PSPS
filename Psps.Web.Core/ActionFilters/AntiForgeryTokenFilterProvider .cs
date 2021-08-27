using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcRefApp.Web.Core.ActionFilters
{
    //This will add ValidateAntiForgeryToken Attribute to all HttpPost action methods
    public class AntiForgeryTokenFilterProvider : System.Web.Mvc.IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (controllerContext.HttpContext.Request.HttpMethod.Equals("Post", StringComparison.CurrentCultureIgnoreCase)
                && controllerContext.Controller.ToString().Equals("ErrorController", StringComparison.CurrentCultureIgnoreCase))
            {
                yield return new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null);
            }
        }
    }
}
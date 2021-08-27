using Autofac.Integration.Mvc;
using Psps.Core.JqGrid.Models;
using System.Web.Mvc;

namespace Psps.Core.JqGrid
{
    [ModelBinderType(typeof(GridSettings))]
    public class GridModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request.Unvalidated;
                return new GridSettings
                {
                    IsSearch = bool.Parse(request["_search"] ?? "false"),
                    PageIndex = int.Parse(request["page"] ?? "1"),
                    PageSize = int.Parse(request["rows"] ?? "10"),
                    SortColumn = request["sidx"] ?? "",
                    SortOrder = request["sord"] ?? "asc",
                    Where = Psps.Core.JqGrid.Models.Filter.Create(request["filters"] ?? "")
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
using Autofac.Integration.Mvc;
using Psps.Core.JqGrid.Models;
using System.Web.Mvc;

namespace Psps.Core.JqGrid
{
    [ModelBinderType(typeof(ExportSettings))]
    public class ExportModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request.Unvalidated;
                var exportSettings = new ExportSettings
                {
                    GridSettings = GridSettings.Create(request["gridSettings"] ?? ""),
                    ColumnModel = ColumnModel.Create(request["columnModel"] ?? ""),
                };

                if (exportSettings.GridSettings == null)
                {
                    exportSettings.GridSettings = new GridSettings();
                }
                exportSettings.GridSettings.PageSize = 9999999;
                exportSettings.GridSettings.PageIndex = 1;

                return exportSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}
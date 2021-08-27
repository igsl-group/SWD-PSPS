using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Infrastructure.DI
{
    public class DiModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return modelType.IsInterface
                       ? DependencyResolver.Current.GetService(modelType)
                       : base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}
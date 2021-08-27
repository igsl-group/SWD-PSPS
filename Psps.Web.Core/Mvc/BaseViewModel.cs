using System.Collections.Generic;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc
{
    /// <summary>
    /// Base model
    /// </summary>
    public partial class BaseViewModel
    {
        public BaseViewModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Use this property to store any custom value for your models.
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
    }
}
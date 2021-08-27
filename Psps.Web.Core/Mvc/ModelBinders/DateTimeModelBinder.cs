using System;
using System.Globalization;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc.ModelBinders
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        private string[] _customFormats;

        public DateTimeModelBinder(string[] customFormats)
        {
            _customFormats = customFormats;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (!string.IsNullOrEmpty(value.AttemptedValue))
                return DateTime.ParseExact(value.AttemptedValue, _customFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            return null;
        }
    }
}
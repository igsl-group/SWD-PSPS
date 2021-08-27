using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Psps.Core.Helper;

namespace Psps.Web.Mappings
{
    public class DMYDateConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        private const String dateFormat = @"dd/MM/yyyy";

        public override bool CanConvertFrom(Type type)
        {
            return typeof(String) == type;
        }

        public override bool CanConvertTo(Type type)
        {
            return typeof(String) == type;
        }

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            DateTime newDate = default(System.DateTime);

            if (!string.IsNullOrEmpty(text))
                try
                {
                    newDate = DateTime.ParseExact(text, dateFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format(@"Error parsing date '{0}': {1}", text, ex.Message));
                }

            return newDate;
        }

        public override string ConvertToString(TypeConverterOptions options, object value)
        {
            DateTime oldDate = (System.DateTime)value;
            return oldDate.ToString(dateFormat);
        }
    }

    public class NullableDMYDateConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        private const String dateFormat = @"dd/MM/yyyy";

        public override bool CanConvertFrom(Type type)
        {
            return typeof(String) == type;
        }

        public override bool CanConvertTo(Type type)
        {
            return typeof(String) == type;
        }

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            DateTime? newDate = null;

            if (!string.IsNullOrEmpty(text))
                try
                {
                    //newDate = DateTime.ParseExact(text, dateFormat, CultureInfo.InvariantCulture);
                    newDate = CommonHelper.ConvertStringToDateTime(text);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format(@"Error parsing date '{0}': {1}", text, ex.Message));
                }

            return newDate;
        }

        public override string ConvertToString(TypeConverterOptions options, object value)
        {
            DateTime? oldDate = (System.DateTime?)value;
            return oldDate.HasValue ? oldDate.Value.ToString(dateFormat) : string.Empty;
        }
    }
}
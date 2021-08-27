using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxGenerator.Library
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using DocxGenerator.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class SimpleDocumentGenerator<T> : DocumentGenerator where T : class
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDocumentGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public SimpleDocumentGenerator(DocumentGenerationInfo generationInfo)
            : base(generationInfo)
        {
        }

        #endregion Constructor

        #region Overridden methods

        /// <summary>
        /// Gets the place holder tag to type collection.
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, PlaceHolderType> GetPlaceHolderTagToTypeCollection(Type dataContextType, string parentName = null)
        {
            Dictionary<string, PlaceHolderType> placeHolderTagToTypeCollection = new Dictionary<string, PlaceHolderType>(StringComparer.InvariantCultureIgnoreCase);

            PropertyInfo[] props = dataContextType.GetProperties();
            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo prp in props)
            {
                var fqn = string.IsNullOrEmpty(parentName) ? prp.Name : parentName + "." + prp.Name;

                if (prp.CanRead && !placeHolderTagToTypeCollection.ContainsKey(prp.Name))
                {
                    Type t = prp.PropertyType;
                    if (t != typeof(Byte[]) && (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                        t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)))
                    {
                        placeHolderTagToTypeCollection.Add(fqn, PlaceHolderType.Recursive);
                        var innerCollection = GetPlaceHolderTagToTypeCollection(t.GetGenericArguments().Single(), prp.Name);
                        foreach (var item in innerCollection)
                        {
                            if (!placeHolderTagToTypeCollection.ContainsKey(item.Key))
                                placeHolderTagToTypeCollection.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        placeHolderTagToTypeCollection.Add(fqn, PlaceHolderType.NonRecursive);
                    }
                }
            }

            return placeHolderTagToTypeCollection;
        }

        /// <summary>
        /// Non recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void NonRecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagValue = string.Empty;
            string content = string.Empty;

            //Find Customerizee Date format
            string custDateFormat = "";
            Regex regex = new Regex("w:dateFormat w:val=\"([^\"]*)\"");
            Match match = regex.Match(openXmlElementDataContext.Element.InnerXml);

            if (match.Success)
            {
                custDateFormat = match.Groups[1].Value;
            }
            //Find Customerizee culture
            string culture = "";
            regex = new Regex("w:lid w:val=\"([^\"]*)\"");
            match = regex.Match(openXmlElementDataContext.Element.InnerXml);
            if (match.Success)
            {
                culture = match.Groups[1].Value;
            }

            content = GetPropertyValue(openXmlElementDataContext.DataContext, placeholderTag, custDateFormat, culture);

            // Set the tag for the content control
            if (!string.IsNullOrEmpty(tagValue))
            {
                this.SetTagValue(openXmlElementDataContext.Element as SdtElement, GetFullTagValue(placeholderTag, tagValue));
            }

            // Set text without data binding
            this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
        }

        /// <summary>
        /// Recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void RecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            var childerns = GetChilderns(openXmlElementDataContext.DataContext, placeholderTag);
            if (childerns != null)
            {
                var parentTag = string.IsNullOrEmpty(openXmlElementDataContext.ParentTag) ? placeholderTag : openXmlElementDataContext.ParentTag + "." + placeholderTag;
                foreach (var c in childerns)
                {
                    SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = c, ParentTag = parentTag });
                }
                openXmlElementDataContext.Element.Remove();
            }
        }

        protected override void IgnorePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
        }

        protected override void ContainerPlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
        }

        #endregion Overridden methods

        public void ToFile(string outputPath)
        {
            byte[] fileContents = this.GenerateDocument();

            if (fileContents != null)
            {
                File.WriteAllBytes(outputPath, fileContents);
            }
        }

        internal int ParseRepeatPattern(String format, int pos, char patternChar)
        {
            int len = format.Length;
            int index = pos + 1;
            while ((index < len) && (format[index] == patternChar))
            {
                index++;
            }
            return (index - pos);
        }

        internal void FormatChiDigits(StringBuilder outputBuffer, int value, bool fillTen)
        {
            int n = value;
            string p = "";
            string[] chi = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string pos = "十";

            do
            {
                if (fillTen && p.Length == 1)
                {
                    if (n % 10 > 1)
                        p = chi[n % 10] + pos + p;
                    else
                        p = pos + p;
                }
                else
                {
                    p = chi[n % 10] + p;
                }

                n /= 10;
            } while (n != 0);

            if (fillTen && value / 10 >= 1 && value % 10 == 0)
            {
                // TIR: PSUAT00035-12
                // Delete “零” when the date is ended with 10, 20 or 30 in Chinese
                p = p.Substring(0, p.Length - 1);
            }

            outputBuffer.Append(p);
        }

        private string GetPropertyValue(object o, string p, string dateFormat, string cultureStr)
        {
            var value = o.GetType().GetProperties().Where(x => x.Name.Equals(p, StringComparison.CurrentCultureIgnoreCase)).Single().GetValue(o, null);

            if (value != null)
            {
                if (value.GetType().Name == "DateTime")
                {
                    string dateTimeFormat = "dd-MM-yyyy";

                    var dateTime = (DateTime)value;
                    if (dateTime.Hour != 0 && dateTime.Minute != 0 && dateTime.Second != 0 && dateTime.Millisecond != 0)
                        dateTimeFormat = "dd-MM-yyyy HH:mm:ss";

                    if (dateFormat != "")
                    {
                        try
                        {
                            if (cultureStr != "")
                            {
                                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureStr);
                                return FormatCustomized(dateTime, dateFormat, culture);
                            }
                            else return FormatCustomized(dateTime, dateFormat);
                        }
                        catch (Exception)
                        {
                            return value.ToString();
                        }
                    }
                    else
                        return dateTime.ToString(dateTimeFormat);
                }

                return value.ToString();
            }
            return "";
        }

        private String FormatCustomized(DateTime dateTime, String format, System.Globalization.CultureInfo culture = null)
        {
            StringBuilder result = new StringBuilder();
            // This is a flag to indicate if we are format the dates using Hebrew calendar.

            int i = 0;
            int tokenLen;
            string[] formatMask = new string[] { "E", "O", "A" };

            if (formatMask.Any(format.Contains))
            {
                while (i < format.Length)
                {
                    char ch = format[i];
                    switch (ch)
                    {
                        case 'E':
                            //
                            tokenLen = ParseRepeatPattern(format, i, ch);

                            // tokenLen >= 4 : Full Year (EEEE)
                            if (tokenLen >= 4)  
                                FormatChiDigits(result, dateTime.Year, false);
                            else if (tokenLen == 2)
                                // (EE) Year in two digits
                                FormatChiDigits(result, Convert.ToInt16(dateTime.ToString("yy")), false);
                            else
                                FormatChiDigits(result, dateTime.Year, false);

                            break;

                        case 'O':
                            //
                            // tokenLen == 1 : Month as digits with no leading zero.
                            // tokenLen == 2 : Month as digits with leading zero for single-digit months.
                            // tokenLen == 3 : Month as a three-letter abbreviation.
                            // tokenLen >= 4 : Month as its full name.
                            //
                            tokenLen = ParseRepeatPattern(format, i, ch);
                            FormatChiDigits(result, dateTime.Month, true);

                            break;

                        case 'A':
                            // Day
                            tokenLen = ParseRepeatPattern(format, i, ch);
                            FormatChiDigits(result, dateTime.Day, true);
                            break;

                        default:
                            // NOTENOTE : we can remove this rule if we enforce the enforced quote
                            // character rule.
                            // That is, if we ask everyone to use single quote or double quote to insert characters,
                            // then we can remove this default block.
                            tokenLen = 1;
                            result.Append(ch);
                            break;
                    }
                    i += tokenLen;
                }

                return result.ToString();
            }
            else
                return dateTime.ToString(format, culture);
        }

        private ICollection GetChilderns(object o, string p)
        {
            return (IList)o.GetType().GetProperty(p).GetValue(o, null);
        }
    }
}
using FluentValidation;
using Psps.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Psps.Web.Core.Extensions
{
    public static class NamedMessageExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithNamedMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, string format)
        {
            return rule.WithMessage("{0}", x => format.FormatNamedMessage(x));
        }
    }

    public static class NamedMessageFormatter
    {
        public static string FormatNamedMessage(this string format, object source)
        {
            return FormatWith(format, null, source);
        }

        public static string FormatWith(this string format
            , IFormatProvider provider, object source)
        {
            Ensure.Argument.NotNull(format, "format");

            List<object> values = new List<object>();
            string rewrittenFormat = Regex.Replace(format,
              @"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
              delegate(Match m)
              {
                  Group startGroup = m.Groups["start"];
                  Group propertyGroup = m.Groups["property"];
                  Group formatGroup = m.Groups["format"];
                  Group endGroup = m.Groups["end"];

                  values.Add((propertyGroup.Value == "0")
                    ? source
                    : Eval(source, propertyGroup.Value));

                  int openings = startGroup.Captures.Count;
                  int closings = endGroup.Captures.Count;

                  return openings > closings || openings % 2 == 0
                     ? m.Value
                     : new string('{', openings) + (values.Count - 1)
                       + formatGroup.Value
                       + new string('}', closings);
              },
              RegexOptions.Compiled
              | RegexOptions.CultureInvariant
              | RegexOptions.IgnoreCase);

            return string.Format(provider, rewrittenFormat, values.ToArray());
        }

        private static object Eval(object source, string expression)
        {
            try
            {
                return DataBinder.Eval(source, expression);
            }
            catch (HttpException e)
            {
                throw new FormatException(null, e);
            }
        }
    }
}
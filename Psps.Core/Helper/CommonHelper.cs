using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Core.Helper
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {
        public static readonly string DefaultDateFormat = "dd/MM/yyyy";

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = "";
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return "";

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return "";

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        //public static string GenerateRandomDigitCode(int length)
        //{
        //    var random = new Random();
        //    string str = "";
        //    for (int i = 0; i < length; i++)
        //        str = String.Concat(str, random.Next(10).ToString());
        //    return str;
        //}

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        //public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        //{
        //    var randomNumberBuffer = new byte[10];
        //    new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
        //    return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        //}

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Clean up path by removing all the invalid chars
        /// </summary>
        /// <param name="path">Path to clean up</param>
        /// <returns>A valid path</returns>
        public static string CleanPath(string path)
        {
            return System.IO.Path.GetInvalidPathChars().Aggregate(path, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static DateTime ConvertStringToDateTime(string str)
        {
            DateTime outDate;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(str, DefaultDateFormat, enUS, DateTimeStyles.None, out outDate);
            return outDate;
        }

        public static DateTime ConvertStringToDateTime(string str, string dateTimeFormat)
        {
            DateTime outDate;
            CultureInfo enUS = new CultureInfo("en-US");
            if (!String.IsNullOrEmpty(dateTimeFormat))
            {
                DateTime.TryParseExact(str, dateTimeFormat, enUS, DateTimeStyles.None, out outDate);
            }
            else
            {
                DateTime.TryParseExact(str, DefaultDateFormat, enUS, DateTimeStyles.None, out outDate);
            }
            return outDate;
        }

        public static string ConvertDateTimeToString(DateTime data)
        {
            string str = "";
            if (data != null)
            {
                str = data.ToString(DefaultDateFormat);
            }
            return str;
        }

        public static string ConvertDateTimeToString(DateTime data, string dateTimeFormat)
        {
            string str = "";
            if (data != null)
            {
                if (!String.IsNullOrEmpty(dateTimeFormat))
                {
                    str = data.ToString(dateTimeFormat);
                }
                else
                {
                    str = data.ToString(DefaultDateFormat);
                }
            }
            return str;
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        public static bool IsValidDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ConvertHtmlToString(string str)
        {
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "&lt;br&gt;");
            return str;
        }

        public static bool HasValueChanged<T>(T from, T to)
        {
            if (from != null)
                return !from.Equals(to);

            if (to != null)
                return !to.Equals(from);

            return false;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}
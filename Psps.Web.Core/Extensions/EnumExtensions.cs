﻿using Psps.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Web;

namespace Psps.Web.Core.Extensions
{
    public static class EnumExtensions
    {
        // This extension method is broken out so you can use a similar pattern with
        // other MetaData elements in the future. This is your base method for each.
        //In short this is generic method to get any type of attribute.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();//attributes.Length > 0 ? (T)attributes[0] : null;
        }

        // This method creates a specific call to the above method, requesting the
        // Display MetaData attribute.
        //e.g. [Display(Name = "Sunday")]
        public static string ToDisplayName(this Enum value)
        {
            var attribute = value.GetAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.GetName();
        }
    }
}
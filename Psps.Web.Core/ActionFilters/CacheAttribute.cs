using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Psps.Web.Core.ActionFilters
{
    /// <summary>
    /// Represents an attribute that is used to mark a controller method whose output will be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerCacheAttribute : OutputCacheAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ControllerCacheAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="profile">The name of an output cache profile.</param>
        public ControllerCacheAttribute(string profile)
        {
            if (String.IsNullOrWhiteSpace(profile))
                throw new ArgumentException("Not a valid profile name.", "profile");
            CacheProfile = profile;
        }
    }

    /// <summary>
    /// Represents an attribute that is used to mark an action method whose output will be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionCacheAttribute : OutputCacheAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ActionCacheAttribute()
        {
            VaryByParam = "none";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="profile">The name of an output cache profile.</param>
        /// <param name="childAction">A value indicates, if the attribute is being used for the child action.</param>
        public ActionCacheAttribute(string profile, bool childAction = false)
        {
            if (String.IsNullOrWhiteSpace(profile))
                throw new ArgumentException("Not a valid profile name.", "profile");
            if (childAction)
            {
                var settings = WebConfigurationManager
                    .GetSection("system.web/caching/outputCacheSettings")
                    as OutputCacheSettingsSection;
                var section = settings.OutputCacheProfiles[profile];
                Duration = section.Duration;
                VaryByParam = section.VaryByParam;
                VaryByCustom = section.VaryByCustom;
            }
            else
                CacheProfile = profile;
        }
    }
}
using System;
using System.Configuration;
using System.Xml;

namespace Psps.Core.Configuration
{
    /// <summary>
    /// Represents a PspsConfig
    /// </summary>
    public partial class PspsConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Appliction name.
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// Application version number.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Fras Certification Name
        /// </summary>
        public string FrasCert { get; private set; }

        /// <summary>
        /// In addition to configured assemblies examine and load assemblies in the bin directory.
        /// </summary>
        public bool DynamicDiscovery { get; private set; }

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new PspsConfig();

            var appNode = section.SelectSingleNode("AppName");
            if (appNode != null && appNode.Attributes != null)
            {
                var attribute = appNode.Attributes["value"];
                if (attribute != null)
                    config.AppName = attribute.Value;
            }

            var frasNode = section.SelectSingleNode("FrasCert");
            if (frasNode != null && frasNode.Attributes != null)
            {
                var attribute = frasNode.Attributes["value"];
                if (attribute != null)
                    config.FrasCert = attribute.Value;
            }

            var versionNode = section.SelectSingleNode("Version");
            if (versionNode != null && versionNode.Attributes != null)
            {
                var attribute = versionNode.Attributes["value"];
                if (attribute != null)
                    config.Version = attribute.Value;
            }

            var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
            if (dynamicDiscoveryNode != null && dynamicDiscoveryNode.Attributes != null)
            {
                var attribute = dynamicDiscoveryNode.Attributes["enabled"];
                if (attribute != null)
                    config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
            }

            return config;
        }
    }
}
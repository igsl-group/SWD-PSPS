using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Psps.Core.JqGrid.Models
{
    public class Filter
    {
        public Filter()
        {
            rules = new List<Rule>();
            groups = new List<Filter>();
        }

        public string groupOp { get; set; }

        public List<Rule> rules { get; set; }

        public List<Filter> groups { get; set; }

        public static Filter Create(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData))
                return null;

            try
            {
                Filter deserializedFilter = JsonConvert.DeserializeObject<Filter>(jsonData);
                return deserializedFilter;
            }
            catch
            {
                return null;
            }
        }
    }
}
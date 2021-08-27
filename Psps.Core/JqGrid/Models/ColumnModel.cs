using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Psps.Core.JqGrid.Models
{
    public class Formatoptions
    {
        public string srcformat { get; set; }

        public string newformat { get; set; }
    }

    public class Editoptions
    {
        public int size { get; set; }
    }

    public class Searchoptions
    {
        public List<string> sopt { get; set; }
    }

    public class Formatoptions2
    {
        public string srcformat { get; set; }

        public string newformat { get; set; }
    }

    public class Editoptions2
    {
        public int size { get; set; }
    }

    public class Searchoptions2
    {
        public List<string> sopt { get; set; }
    }

    public class Template
    {
        public string sorttype { get; set; }

        public string formatter { get; set; }

        public Formatoptions2 formatoptions { get; set; }

        public Editoptions2 editoptions { get; set; }

        public Searchoptions2 searchoptions { get; set; }
    }

    public class Model
    {
        public string index { get; set; }
        
        public string name { get; set; }

        public int width { get; set; }

        public bool sortable { get; set; }

        public bool resizable { get; set; }

        public bool hidedlg { get; set; }

        public bool search { get; set; }

        public string align { get; set; }

        public bool @fixed { get; set; }

        public bool title { get; set; }

        public string lso { get; set; }

        public bool hidden { get; set; }

        public bool exporthidden { get; set; }

        public int widthOrg { get; set; }

        public string sorttype { get; set; }

        public string formatter { get; set; }

        public Formatoptions formatoptions { get; set; }

        public Editoptions editoptions { get; set; }

        public Searchoptions searchoptions { get; set; }

        public Template template { get; set; }
    }

    public class ColumnModel
    {
        public List<string> names { get; set; }

        public List<Model> models { get; set; }

        public static ColumnModel Create(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData))
                return null;

            try
            {
                ColumnModel deserializedData = JsonConvert.DeserializeObject<ColumnModel>(jsonData);
                return deserializedData;
            }
            catch
            {
                return null;
            }
        }
    }
}
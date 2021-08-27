using System.Collections.Generic;
using System.Web.Mvc;

namespace Psps.Core.JqGrid.Models
{
    public class ExportSettings
    {
        public GridSettings GridSettings { get; set; }

        public ColumnModel ColumnModel { get; set; }
    }
}
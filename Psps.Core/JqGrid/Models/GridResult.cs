namespace Psps.Core.JqGrid.Models
{
    public class GridResult
    {
        public int CurrentPageIndex { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public object Data { get; set; }
    }
}
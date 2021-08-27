using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Psps.Core.JqGrid.Models
{
    public class GridSettings
    {
        public GridSettings()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public bool IsSearch { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public Filter Where { get; set; }

        public static GridSettings Create(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData))
                return null;

            try
            {
                dynamic deserializedData = JsonConvert.DeserializeObject<dynamic>(jsonData);
                GridSettings grid = new GridSettings();

                grid.IsSearch = deserializedData._search ?? false;
                grid.PageIndex = deserializedData.page;
                grid.PageSize = deserializedData.rows;
                grid.SortColumn = deserializedData.sidx;
                grid.SortOrder = deserializedData.sord;
                grid.Where = Filter.Create(deserializedData.filters.ToString());
                return grid;
            }
            catch
            {
                return null;
            }
        }

        public void AddDefaultRule(Rule rule, GroupOp groupOp = GroupOp.AND)
        {
            this.AddDefaultRule(new List<Rule> { rule }, groupOp);
        }

        public void AddDefaultRule(List<Rule> rules, GroupOp groupOp = GroupOp.AND)
        {
            this.IsSearch = true;

            var defaultFilter = new Filter();
            defaultFilter.groupOp = groupOp.GetName();
            defaultFilter.rules.AddRange(rules);

            var newWhere = new Filter();
            newWhere.groupOp = GroupOp.AND.GetName();
            newWhere.groups.Add(defaultFilter);

            if (this.Where != null)
                newWhere.groups.Add(this.Where);

            this.Where = newWhere;
        }
    }
}
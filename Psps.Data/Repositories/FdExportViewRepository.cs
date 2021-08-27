using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.Common;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.FdStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IFdExportViewRepository : IRepository<FdExportView, int>
    {

        IPagedList<FdExportView> GetExportData(GridSettings grid);
        
   }

    public class FdExportViewRepository : BaseRepository<FdExportView, int>, IFdExportViewRepository
    {
        private ILookupRepository _lookupRepository;

        public FdExportViewRepository(ISession session, ILookupRepository lookupRepository)
            : base(session)
        {
            _lookupRepository = lookupRepository;
        }

        public IPagedList<FdExportView> GetExportData(GridSettings grid)
        {
            var query = this.Table;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            //if (!string.IsNullOrEmpty(grid.SortColumn))
            //    query = query.OrderBy<FdExportView>(grid.SortColumn, grid.SortOrder);
            query = query.OrderBy<FdExportView>("FdYear desc, WithPermit desc, FlagDay, TWR, TWRDistrict", "asc");

            var page = new PagedList<FdExportView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }     

    }
}
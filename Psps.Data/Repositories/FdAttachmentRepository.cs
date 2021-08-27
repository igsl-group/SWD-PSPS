using NHibernate;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IFdAttachmentRepository : IRepository<FdAttachment, int>
    {
        IPagedList<FdAttachment> GetPageByFdMasterId(GridSettings grid, int fdMasterId);
    }

    public class FdAttachmentRepository : BaseRepository<FdAttachment, int>, IFdAttachmentRepository
    {
        public FdAttachmentRepository(ISession session)
            : base(session)
        {
            //this.session = session;
        }

        public IPagedList<FdAttachment> GetPageByFdMasterId(GridSettings grid, int fdMasterId)
        {
            var query = this.Table;

            //filtring
            //if (fdMasterId != null)
            //{
            //    query = query.Where(x => x.fdMaster.fdMasterId == fdMasterId);
            //}

            //if (grid.IsSearch)
            //    query = query.Where(grid.Where);

            ////sorting
            //if (!string.IsNullOrEmpty(grid.SortColumn))
            //    query = query.OrderBy<fdAttachment>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdAttachment>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
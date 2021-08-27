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
    public interface IPspAttachmentRepository : IRepository<PspAttachment, int>
    {
        IPagedList<PspAttachment> GetPageByPspMasterId(GridSettings grid, int pspMasterId);
    }

    public class PspAttachmentRepository : BaseRepository<PspAttachment, int>, IPspAttachmentRepository
    {
        public PspAttachmentRepository(ISession session)
            : base(session)
        {
            //this.session = session;
        }

        public IPagedList<PspAttachment> GetPageByPspMasterId(GridSettings grid, int pspMasterId)
        {
            var query = this.Table;

            //filtring
            //if (pspMasterId != null)
            //{
            //    query = query.Where(x => x.PspMaster.PspMasterId == pspMasterId);
            //}

            //if (grid.IsSearch)
            //    query = query.Where(grid.Where);

            ////sorting
            //if (!string.IsNullOrEmpty(grid.SortColumn))
            //    query = query.OrderBy<PspAttachment>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspAttachment>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
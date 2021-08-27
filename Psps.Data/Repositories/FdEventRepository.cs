using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IFdEventRepository : IRepository<FdEvent, int>
    {
        IPagedList<FdReadEventDto> GetPageByFdMasterId(GridSettings grid, int fdMasterId);

        FdEvent GetRecByFdMasterId(int fdMasterId);

        int GetFdEventCountByFlagDay(DateTime targetDay);
    }

    public class FdEventRepository : BaseRepository<FdEvent, int>, IFdEventRepository
    {
        public FdEventRepository(ISession session)
            : base(session)
        {
        }

        public IPagedList<FdReadEventDto> GetPageByFdMasterId(GridSettings grid, int fdMasterId)
        {
            //DateTime? nullDate = (DateTime?)null;

            var query = from u in this.Table
                        where u.FdMaster.FdMasterId == fdMasterId
                        select new FdReadEventDto
                        {
                            FdEventId = u.FdEventId,
                            FlagDay = u.FlagDay,
                            EventStartTime = u.FlagTimeFrom,
                            EventEndTime = u.FlagTimeTo,
                            TWR = u.TWR,
                            District = u.TwrDistrict,
                            PermitNum = u.PermitNum,
                            BagColour = u.BagColour,
                            FlagColour = u.FlagColour,  
                            CollectionMethod = u.CollectionMethod,
                            Remark = u.Remarks, 
                            PermitRevokeIndicator = u.PermitRevokeIndicator 
                        }

                        ;

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<FdReadEventDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<FdReadEventDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public FdEvent GetRecByFdMasterId(int fdMasterId)
        {
            Ensure.Argument.NotNull(fdMasterId, "fdMasterId");
            return this.Table.Where(x => x.FdMaster.FdMasterId == fdMasterId).FirstOrDefault();
        }

        public int GetFdEventCountByFlagDay(DateTime targetDay)
        {
            return this.Table.Where(u => u.FlagDay == targetDay && u.IsDeleted == false).ToList().Count();
        }
    }
}
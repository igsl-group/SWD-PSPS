using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IDisasterStatisticsRepository : IRepository<DisasterStatistics, int>
    {
        DisasterStatistics GetDisasterStatisticsById(int DisasterStatisticsId);

        IList<DisasterStatistics> GetDisasterStatisticsByMasterId(int disasterMasterId);
    }

    public class DisasterStatisticsRepository : BaseRepository<DisasterStatistics, int>, IDisasterStatisticsRepository
    {
        public DisasterStatisticsRepository(ISession session)
            : base(session)
        {
        }

        public DisasterStatistics GetDisasterStatisticsById(int DisasterStatisticsId)
        {
            return this.Table.Where(x => x.DisasterStatisticsId == DisasterStatisticsId).Fetch(x => x.DisasterMaster).FirstOrDefault();
        }

        public IList<DisasterStatistics> GetDisasterStatisticsByMasterId(int disasterMasterId)
        {
            return this.Table.Where(x => x.DisasterMaster.DisasterMasterId == disasterMasterId).ToList();
        }

        public override IPagedList<DisasterStatistics> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //eager fetching
            query = query.Fetch(x => x.DisasterMaster);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<DisasterStatistics>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<DisasterStatistics>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
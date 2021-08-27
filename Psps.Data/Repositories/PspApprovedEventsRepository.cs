using NHibernate;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Data.Repositories
{
    public interface IPspApprovedEventsRepository : IRepository<PspApprovedEvents, string>
    {
        List<PspApprovedEvents> getListByPspMasterId(int pspMasterId);
    }

    public class PspApprovedEventsRepository : BaseRepository<PspApprovedEvents, string>, IPspApprovedEventsRepository
    {
        public PspApprovedEventsRepository(ISession session)
            : base(session)
        {
        }

        public List<PspApprovedEvents> getListByPspMasterId(int pspMasterId)
        {
            //var query = this.Table.Where(x => x.PspMaster.PspMasterId == pspMasterId).OrderBy(x => x.Location).ThenBy(x => x.EventStartYear).ThenBy(x => x.EventStartMonth).ThenBy(x => x.EventDays);

            var query = from x in this.Table
                        where x.PspMaster.PspMasterId == pspMasterId
                        orderby x.District, x.EventStartYear, x.EventStartMonth, x.EventDays, x.EventStartTime, x.EventEndTime, x.Location
                        select x;

            return query.ToList();
        }
    }
}   
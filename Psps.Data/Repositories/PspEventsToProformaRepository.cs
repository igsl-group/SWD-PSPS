using NHibernate;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IPspEventsToProformaRepository : IRepository<PspEventsToProforma, string>
    {
        List<PspEventsToProforma> getListByPspMasterId(int pspMasterId);
    }

    public class PspEventsToProformaRepository : BaseRepository<PspEventsToProforma, string>, IPspEventsToProformaRepository
    {
        public PspEventsToProformaRepository(ISession session)
            : base(session)
        {
        }

        public List<PspEventsToProforma> getListByPspMasterId(int pspMasterId)
        {
            var query = this.Table.Where(x => x.PspMaster.PspMasterId == pspMasterId);

            //var query2 = query.OrderBy(x => x.EventStartYear)
            //                            .ThenBy(x => x.EventStartMonth)
            //                            ;
            //.ThenBy(x => x.EventDays)
            //.ThenBy(x => x.EventStartTime);

            return query.ToList();
        }
    }
}
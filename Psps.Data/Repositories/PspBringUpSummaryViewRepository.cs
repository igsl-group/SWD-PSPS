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
    public interface IPspBringUpSummaryViewRepository : IRepository<PspBringUpSummaryView, string> { }

    public class PspBringUpSummaryViewRepository : BaseRepository<PspBringUpSummaryView, string>, IPspBringUpSummaryViewRepository
    {
        public PspBringUpSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
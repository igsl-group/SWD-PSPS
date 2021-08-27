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
    public interface IPspDocSummaryViewRepository : IRepository<PspDocSummaryView, int> { }

    public class PspDocSummaryViewRepository : BaseRepository<PspDocSummaryView, int>, IPspDocSummaryViewRepository
    {
        public PspDocSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
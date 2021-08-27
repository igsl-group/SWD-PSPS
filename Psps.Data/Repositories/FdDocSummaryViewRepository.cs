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
    public interface IFdDocSummaryViewRepository : IRepository<FdDocSummaryView, int> { }

    public class FdDocSummaryViewRepository : BaseRepository<FdDocSummaryView, int>, IFdDocSummaryViewRepository
    {
        public FdDocSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface ILookupRepository : IRepository<Lookup, int>
    {
    }

    public class LookupRepository : BaseRepository<Lookup, int>, ILookupRepository
    {
        public LookupRepository(ISession session)
            : base(session)
        {
        }
    }
}
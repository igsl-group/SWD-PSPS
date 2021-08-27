using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IRankRepository : IRepository<Rank, string>
    {
    }

    public class RankRepository : BaseRepository<Rank, string>, IRankRepository
    {
        public RankRepository(ISession session)
            : base(session)
        {
        }
    }
}
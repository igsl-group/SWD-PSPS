using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface ISystemMessageRepository : IRepository<SystemMessage, int>
    {
    }

    public class SystemMessageRepository : BaseRepository<SystemMessage, int>, ISystemMessageRepository
    {
        public SystemMessageRepository(ISession session)
            : base(session)
        {
        }
    }
}
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IRoleRepository : IRepository<Role, string>
    {
    }

    public class RoleRepository : BaseRepository<Role, string>, IRoleRepository
    {
        public RoleRepository(ISession session)
            : base(session)
        {
        }
    }
}
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IComplaintFdPermitNumSearchViewRepository : IRepository<ComplaintFdPermitNumSearchView, int>
    {

    }

    public class ComplaintFdPermitNumSearchViewRepository : BaseRepository<ComplaintFdPermitNumSearchView, int>, IComplaintFdPermitNumSearchViewRepository
    {
        public ComplaintFdPermitNumSearchViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
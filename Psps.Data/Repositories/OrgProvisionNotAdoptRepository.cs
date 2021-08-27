using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IOrgProvisionNotAdoptRepository : IRepository<OrgProvisionNotAdopt, int>
    {
        void DeleteByGuidePromulgationId(int OrgRefGuidePromulgationId);
    }

    public class OrgProvisionNotAdoptRepository : BaseRepository<OrgProvisionNotAdopt, int>, IOrgProvisionNotAdoptRepository
    {
        public OrgProvisionNotAdoptRepository(ISession session)
            : base(session)
        {
        }

        public void DeleteByGuidePromulgationId(int OrgRefGuidePromulgationId) 
        {
            string hql = "from OrgProvisionNotAdopt where OrgRefGuidePromulgationId=" + OrgRefGuidePromulgationId;
            this.Session.Delete(hql);
        }
    }
}
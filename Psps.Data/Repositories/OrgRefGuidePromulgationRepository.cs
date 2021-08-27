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
    public interface IOrgRefGuidePromulgationRepository : IRepository<OrgRefGuidePromulgation, int> { }

    public class OrgRefGuidePromulgationRepository : BaseRepository<OrgRefGuidePromulgation, int>, IOrgRefGuidePromulgationRepository
    {
        public OrgRefGuidePromulgationRepository(ISession session)
            : base(session)
        {
        }
    }
}

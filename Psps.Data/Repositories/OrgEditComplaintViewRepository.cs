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
    public interface IOrgEditComplaintViewRepository : IRepository<OrgEditComplaintView, int> { }

    public class OrgEditComplaintViewRepository : BaseRepository<OrgEditComplaintView, int>, IOrgEditComplaintViewRepository
    {
        public OrgEditComplaintViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
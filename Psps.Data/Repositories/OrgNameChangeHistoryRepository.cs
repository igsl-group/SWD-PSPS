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
    public interface IOrgNameChangeHistoryRepository : IRepository<OrgNameChangeHistory, int> { }

    public class OrgNameChangeHistoryRepository : BaseRepository<OrgNameChangeHistory, int>, IOrgNameChangeHistoryRepository
    {
        public OrgNameChangeHistoryRepository(ISession session)
            : base(session)
        {
        }
    }
}
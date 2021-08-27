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
    public interface IOrgDocSummaryViewRepository : IRepository<OrgDocSummaryView, int> { }

    public class OrgDocSummaryViewRepository : BaseRepository<OrgDocSummaryView, int>, IOrgDocSummaryViewRepository
    {
        public OrgDocSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
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
    public interface IComplaintDocSummaryViewRepository : IRepository<ComplaintDocSummaryView, int> { }

    public class ComplaintDocSummaryViewRepository : BaseRepository<ComplaintDocSummaryView, int>, IComplaintDocSummaryViewRepository
    {
        public ComplaintDocSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
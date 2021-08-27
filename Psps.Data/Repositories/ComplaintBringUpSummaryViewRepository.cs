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
    public interface IComplaintBringUpSummaryViewRepository : IRepository<ComplaintBringUpSummaryView, string> { }

    public class ComplaintBringUpSummaryViewRepository : BaseRepository<ComplaintBringUpSummaryView, string>, IComplaintBringUpSummaryViewRepository
    {
        public ComplaintBringUpSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;
using Psps.Core.Models;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;

namespace Psps.Data.Repositories
{
    public interface IWithholdingHistoryRepository : IRepository<WithholdingHistory, int>
    {

    }

    public class WithholdingHistoryRepository : BaseRepository<WithholdingHistory, int>, IWithholdingHistoryRepository
    {
        public WithholdingHistoryRepository(ISession session)
            : base(session)
        {
        }


    }


}
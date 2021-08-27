using Lfpis.Data.Infrastructure;
using Lfpis.Models.Domain;
using NHibernate;
using System;

namespace Lfpis.Data.Repositories
{
    public interface IBlockGrantRepository : IRepository<BlockGrant, int>
    {
    }

    public class BlockGrantRepository : BaseRepository<BlockGrant, int>, IBlockGrantRepository
    {
        public BlockGrantRepository(ISession session)
            : base(session)
        {
        }
    }
}
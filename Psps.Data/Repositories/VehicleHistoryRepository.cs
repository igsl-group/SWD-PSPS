using Lfpis.Data.Infrastructure;
using Lfpis.Models.Domain;
using NHibernate;
using System;

namespace Lfpis.Data.Repositories
{
    public interface IVehicleHistoryRepository : IRepository<VehicleHistory, int>
    {
    }

    public class VehicleHistoryRepository : BaseRepository<VehicleHistory, int>, IVehicleHistoryRepository
    {
        public VehicleHistoryRepository(ISession session)
            : base(session)
        {

        }
    }
}

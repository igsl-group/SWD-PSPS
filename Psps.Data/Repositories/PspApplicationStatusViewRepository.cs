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
    public interface IPspApplicationStatusViewRepository : IRepository<PspApplicationStatusView, string> { }

    public class PspApplicationStatusViewRepository : BaseRepository<PspApplicationStatusView, string>, IPspApplicationStatusViewRepository
    {
        public PspApplicationStatusViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
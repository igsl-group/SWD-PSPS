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
    public interface ISsafApplicationStatusViewRepository : IRepository<SsafApplicationStatusView, string> { }

    public class SsafApplicationStatusViewRepository : BaseRepository<SsafApplicationStatusView, string>, ISsafApplicationStatusViewRepository
    {
        public SsafApplicationStatusViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
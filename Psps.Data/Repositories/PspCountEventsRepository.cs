using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Models.Dto.PspMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IPspCountEventsRepository : IRepository<PspCountEvents, int>
    {
    }

    public class PspCountEventsRepository : BaseRepository<PspCountEvents, int>, IPspCountEventsRepository
    {
        public PspCountEventsRepository(ISession session)
            : base(session)
        {
        }
    }
}
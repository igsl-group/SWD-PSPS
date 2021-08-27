using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IPspDocViewRepository : IRepository<PspDocView, int>
    {
    }

    public class PspDocViewRepository : BaseRepository<PspDocView, int>, IPspDocViewRepository
    {
        public PspDocViewRepository(ISession session)
            : base(session)
        {
        }
    }
}
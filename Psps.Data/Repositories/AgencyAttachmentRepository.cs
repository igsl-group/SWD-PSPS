using Lfpis.Core.JqGrid.Extensions;
using Lfpis.Core.JqGrid.Models;
using Lfpis.Core.Models;
using Lfpis.Data.Infrastructure;
using Lfpis.Models;
using Lfpis.Models.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Lfpis.Data.Repositories
{
    public interface IAgencyAttachmentRepository : IRepository<AgencyAttachment, int>
    {
    }

    public class AgencyAttachmentRepository : BaseRepository<AgencyAttachment, int>, IAgencyAttachmentRepository
    {
        public AgencyAttachmentRepository(ISession session)
            : base(session)
        {
        }
    }
}
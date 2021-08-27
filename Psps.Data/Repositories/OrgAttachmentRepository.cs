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
    public interface IOrgAttachmentRepository : IRepository<OrgAttachment, int> { }

    public class OrgAttachmentRepository : BaseRepository<OrgAttachment, int>, IOrgAttachmentRepository
    {
        public OrgAttachmentRepository(ISession session)
            : base(session)
        {
        }
    }
}
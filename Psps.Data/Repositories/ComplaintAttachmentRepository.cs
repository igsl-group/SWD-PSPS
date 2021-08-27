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
    public interface IComplaintAttachmentRepository : IRepository<ComplaintAttachment, int> { }

    public class ComplaintAttachmentRepository : BaseRepository<ComplaintAttachment, int>, IComplaintAttachmentRepository
    {
        public ComplaintAttachmentRepository(ISession session)
            : base(session)
        {
        }
    }
}
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

    public interface ISuggestionAttachmentRepository : IRepository<SuggestionAttachment, int>
    {

    }

    public class SuggestionAttachmentRepository : BaseRepository<SuggestionAttachment, int>, ISuggestionAttachmentRepository
    {
        public SuggestionAttachmentRepository(ISession session)
            : base(session)
        {
        }
    }
}

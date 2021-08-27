using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IDocumentRepository : IRepository<Document, int>
    {
    }

    public class DocumentRepository : BaseRepository<Document, int>, IDocumentRepository
    {
        public DocumentRepository(ISession session)
            : base(session)
        {
        }
    }
}
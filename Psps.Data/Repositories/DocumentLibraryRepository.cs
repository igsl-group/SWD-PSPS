using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IDocumentLibraryRepository : IRepository<DocumentLibrary, int>
    {
    }

    public class DocumentLibraryRepository : BaseRepository<DocumentLibrary, int>, IDocumentLibraryRepository
    {
        public DocumentLibraryRepository(ISession session)
            : base(session)
        {
        }
    }
}
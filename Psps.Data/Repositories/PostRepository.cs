using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;

namespace Psps.Data.Repositories
{
    public interface IPostRepository : IRepository<Post, string>
    {
    }

    public class PostRepository : BaseRepository<Post, string>, IPostRepository
    {
        public PostRepository(ISession session)
            : base(session)
        {
        }
    }
}
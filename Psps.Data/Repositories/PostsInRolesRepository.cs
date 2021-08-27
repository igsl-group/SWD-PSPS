using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using NHibernate;


namespace Psps.Data.Repositories
{
    public interface IPostsInRolesRepository : IRepository<PostsInRoles, int>
    {

    }

    public class PostsInRolesRepository : BaseRepository<PostsInRoles, int>, IPostsInRolesRepository
    {
        public PostsInRolesRepository(ISession session)
            : base(session)
        {
            
        }

    }
}

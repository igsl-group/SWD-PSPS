using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
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
    public interface IUserRepository : IRepository<User, string>
    {
        User GetUserAndPostById(string userId);
    }

    public class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(ISession session)
            : base(session)
        {
        }

        public User GetUserAndPostById(string userId)
        {
            return this.Table.Where(x => x.UserId == userId).Fetch(x => x.Post).FirstOrDefault();
        }

        public override IPagedList<User> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //eager fetching
            query = query.Fetch(x => x.Post);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<User>(grid.SortColumn, grid.SortOrder);
            else
                query = query.OrderByDescending(x => x.IsActive).ThenBy(x => x.Post.Rank != null ? x.Post.Rank.RankLevel : 99);

            var page = new PagedList<User>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}
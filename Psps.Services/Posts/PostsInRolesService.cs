using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Posts
{
    public partial class PostsInRolesService : IPostsInRolesService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IPostRepository _postRepository;

        private readonly IPostsInRolesRepository _postsInRolesRepository;

        #endregion Fields

        #region Ctor

        public PostsInRolesService(ICacheManager cacheManager, IEventPublisher eventPublisher, IPostRepository postRepository, IPostsInRolesRepository postsInRolesRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._postRepository = postRepository;
            this._postsInRolesRepository = postsInRolesRepository;
        }

        #endregion Ctor

        public List<PostsInRoles> GetByRoleId(string roleId)
        {
            return _postsInRolesRepository.Table.Where(p => p.RoleId == roleId).ToList();
        }

        public List<PostsInRoles> GetByPostId(string postId)
        {
            return _postsInRolesRepository.Table.Where(p => p.PostId == postId).ToList();
        }

        public void DeleteByRoleId(string roleId)
        {
            Ensure.Argument.NotNull(roleId, "roleId");
        }

        public bool ValidatePostIsExisted(string roleId, string postId)
        {
            bool flag = false;
            Ensure.Argument.NotNull(roleId, "roleId");
            Ensure.Argument.NotNull(postId, "postId");
            List<PostsInRoles> list = _postsInRolesRepository.Table.Where(p => p.RoleId == roleId && p.PostId == postId).ToList();
            if (list != null && list.Count() > 0)
            {
                flag = true;
            }
            return flag;
        }

        public void CreatePostsInRoles(PostsInRoles postsInRoles)
        {
            _postsInRolesRepository.Add(postsInRoles);
        }

        public IPagedList<PostsInRoles> GetPage(GridSettings grid)
        {
            return _postsInRolesRepository.GetPage(grid);
        }

        public PostsInRoles GetById(int postsInRolesId)
        {
            return _postsInRolesRepository.GetById(postsInRolesId);
        }

        public void DeletePostsInRoles(PostsInRoles postsInRoles)
        {
            Ensure.Argument.NotNull(postsInRoles, "postsInRoles");

            _postsInRolesRepository.Delete(postsInRoles);
            //event notification
            _eventPublisher.EntityUpdated<PostsInRoles>(postsInRoles);
        }
    }
}
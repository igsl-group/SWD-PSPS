using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
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
    /// <summary>
    /// Default implementation of post service interface
    /// </summary>
    public partial class PostService : IPostService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string POST_ALL_KEY = "Psps.post.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string POST_PATTERN_KEY = "Psps.post.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string POST_FOR_DROWDROP_KEY = "Psps.post.dropdown.all";
        
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string POST_FOR_DROWDROP_PATTERN_KEY = "Psps.post.dropdown.";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IPostRepository _postRepository;

        #endregion Fields

        #region Ctor

        public PostService(ICacheManager cacheManager, IEventPublisher eventPublisher, IPostRepository postRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._postRepository = postRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual void DeletePost(Post post)
        {
            Ensure.Argument.NotNull(post, "post");

            post.IsDeleted = true;
            UpdatePost(post);
        }

        public virtual IPagedList<Post> GetPage(GridSettings grid)
        {
            return _postRepository.GetPage(grid);
        }

        public virtual void CreatePost(Post post)
        {
            Ensure.Argument.NotNull(post, "post");

            _postRepository.Add(post);

            //event notification
            _eventPublisher.EntityInserted<Post>(post);
        }

        public virtual void UpdatePost(Post post)
        {
            Ensure.Argument.NotNull(post, "post");

            _postRepository.Update(post);

            //event notification
            _eventPublisher.EntityUpdated<Post>(post);
        }

        public virtual Post GetPostById(string postId)
        {
            return _postRepository.GetById(postId);
        }

        public virtual List<Post> GetPostByOwnerUserId(string ownerUserId)
        {
            return _postRepository.Table.Where(p => p.Owner.UserId == ownerUserId).ToList();
        }

        public bool IsUniquePostId(string postId)
        {
            Ensure.Argument.NotNullOrEmpty(postId);

            return _postRepository.Table.Count(l => l.PostId == postId) == 0;
        }

        /// <summary>
        /// Gets all posts for dropdown
        /// </summary>
        /// <returns>Posts</returns>
        public IDictionary<string, string> GetAllPostsForDropdown()
        {
            string key = POST_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._postRepository.Table
                    .OrderBy(p => p.Rank.RankLevel).ThenBy(p => p.PostId)
                    .Select(p => new { Key = p.PostId, Value = p.PostId })
                    .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetProcessingOfficerPostsForDropdown()
        {
            string key = Constant.POST_FOR_DROWDROP_KEY_PROCESSING_OFFICER;
            return _cacheManager.Get(key, () =>
            {
                return this._postRepository.Table
                    .OrderBy(p => p.Rank.RankLevel).ThenBy(p => p.PostId)
                    .Where(p => p.Roles.Any(q => q.RoleId == "PSP Processing Officer"))
                    .Select(p => new { Key = p.PostId, Value = p.PostId })
                    .ToDictionary(k => k.Key, v => v.Value);
            });
        }



        #endregion Methods
    }
}
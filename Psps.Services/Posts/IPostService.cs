using Psps.Core;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Posts
{
    /// <summary>
    /// Post service interface
    /// </summary>
    public partial interface IPostService
    {
        /// <summary>
        /// Marks post as deleted
        /// </summary>
        /// <param name="post">Post</param>
        void DeletePost(Post post);

        /// <summary>
        /// List posts by type
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Posts</returns>
        IPagedList<Post> GetPage(GridSettings grid);

        /// <summary>
        /// Inserts a post
        /// </summary>
        /// <param name="post">Post</param>
        void CreatePost(Post post);

        /// <summary>
        /// Updates a post
        /// </summary>
        /// <param name="post">Post</param>
        void UpdatePost(Post post);

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="postId">Post identifier</param>
        /// <returns>Post</returns>
        Post GetPostById(string postId);

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="ownerUserId">Post identifier</param>
        /// <returns>Post</returns>
        List<Post> GetPostByOwnerUserId(string ownerUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetAllPostsForDropdown();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetProcessingOfficerPostsForDropdown();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        bool IsUniquePostId(string postId);
    }
}
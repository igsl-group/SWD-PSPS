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
    public interface IPostsInRolesService
    {
        /// <summary>
        /// get records by roleId
        /// </summary>
        /// <param name="roleId">string</param>
        /// <returns>PostsInRoles list</returns>
        List<PostsInRoles> GetByRoleId(string roleId);

        /// <summary>
        /// get records by roleId
        /// </summary>
        /// <param name="postId">string</param>
        /// <returns>PostsInRoles list</returns>
        List<PostsInRoles> GetByPostId(string postId);

        /// <summary>
        /// delete records by roleId
        /// </summary>
        /// <param name="roleId">string</param>
        /// <returns>void</returns>
        void DeleteByRoleId(string roleId);

        /// <summary>
        /// Validate PostsInRoles is Existed or not by roleId and postId
        /// </summary>
        /// <param name="roleId,postId">string</param>
        /// <returns>bool</returns>
        bool ValidatePostIsExisted(string roleId, string postId);

        /// <summary>
        /// Validate PostsInRoles is Existed or not by roleId and postId
        /// </summary>
        /// <param name="roleId,postId">string</param>
        /// <returns>bool</returns>
        void CreatePostsInRoles(PostsInRoles postsInRoles);

        /// <summary>
        /// List PostsInRoles by roleId
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>PostsInRoles</returns>
        IPagedList<PostsInRoles> GetPage(GridSettings grid);

        /// <summary>
        /// get record by roleId
        /// </summary>
        /// <param name="postsInRolesId">int</param>
        /// <returns>PostsInRoles</returns>
        PostsInRoles GetById(int postsInRolesId);

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="postsInRoles">PostsInRoles</param>
        /// <returns>void</returns>
        void DeletePostsInRoles(PostsInRoles postsInRoles);
    }
}
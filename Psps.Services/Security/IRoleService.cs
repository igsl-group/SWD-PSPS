using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Security
{
    public interface IRoleService
    {
        IDictionary<string, string> GetAllRolesForDropdown();

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="taskId">Post identifier</param>
        /// <returns>Post</returns>
        Role GetRoleById(string roleId);

        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="role">role</param>
        void CreateRole(RoleInfoDto roleInfoDto);

        /// <summary>
        /// delete role
        /// </summary>
        /// <param name="role">role</param>
        void DeleteRole(Role role);
    }
}
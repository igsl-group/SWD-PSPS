using AutoMapper;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Security
{
    public class RoleService : IRoleService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string ROLE_ALL_KEY = "Psps.role.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ROLE_PATTERN_KEY = "Psps.role.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string ROLE_FOR_DROWDROP_KEY = "Psps.role.dropdown.all";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IRoleRepository _roleRepository;

        #endregion Fields

        #region Ctor

        public RoleService(ICacheManager cacheManager, IEventPublisher eventPublisher, IRoleRepository roleRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._roleRepository = roleRepository;
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// Gets all roles for dropdown
        /// </summary>
        /// <returns>Posts</returns>
        public IDictionary<string, string> GetAllRolesForDropdown()
        {
            string key = ROLE_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._roleRepository.Table
                    .OrderBy(p => p.RoleId)
                    .Select(p => new { Key = p.RoleId, Value = p.RoleId })
                    .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        /// <summary>
        /// Gets a role
        /// </summary>
        /// <param name="roleId">Role identifier</param>
        /// <returns>Role</returns>
        public Role GetRoleById(string roleId)
        {
            return _roleRepository.GetById(roleId);
        }

        /// <summary>
        /// create a role
        /// </summary>
        /// <param name="RoleInfoDto">RoleInfoDto</param>
        /// <returns>void</returns>
        public void CreateRole(RoleInfoDto roleInfoDto)
        {
            Ensure.Argument.NotNull(roleInfoDto, "roleInfoDto");

            var role = new Role
            {
                RoleId = roleInfoDto.RoleId,
                Description = roleInfoDto.Description,
                IsDeleted = roleInfoDto.IsDeleted,
                CreatedById = roleInfoDto.CreatedById,
                CreatedByPost = roleInfoDto.CreatedByPost,
                CreatedOn = DateTime.Now
            };

            _roleRepository.Add(role);
            _eventPublisher.EntityInserted<Role>(role);
        }

        public void DeleteRole(Role role)
        {
            Ensure.Argument.NotNull(role, "role");
            _roleRepository.Delete(role);
            _eventPublisher.EntityDeleted<Role>(role);
        }

        public void UpdateRole(Role role)
        {
            Ensure.Argument.NotNull(role, "role");
            _roleRepository.Update(role);
            _eventPublisher.EntityUpdated<Role>(role);
        }

        # endregion Methods
    }
}
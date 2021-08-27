using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Security
{
    public interface IFunctionsInRolesService
    {
        /// <summary>
        /// List FunctionsInRoles
        /// </summary>
        /// <param name="FunctionsInRoles">list</param>
        /// <returns>void</returns>
        void CreateOrUpdateFunctionsInRoles(IList<FunctionsInRolesDto> list);

        /// <summary>
        /// check FunctionsInRoles table if table exist the records by roleId
        /// </summary>
        /// <param name="roleId">Role Id stored at Role table</param>
        /// <returns>bool</returns>
        IList<FunctionsInRoles> GetByRoleId(string roleId);
    }
}
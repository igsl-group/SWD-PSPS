using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using System.Collections.Generic;

namespace Psps.Services.Security
{
    public interface IFunctionService
    {
        /// <summary>
        /// List functions
        /// </summary>
        /// <param name="function">Function type</param>
        /// <returns>Functions</returns>
        IPagedList<Function> GetPage(GridSettings grid);

        /// <summary>
        /// List functions
        /// </summary>
        /// <param name="function">Function type</param>
        /// <returns>Functions</returns>
        List<Function> GetFunctionList();

        /// <summary>
        /// get function by functionId
        /// </summary>
        /// <param name="funcId">string</param>
        /// <returns>Function</returns>
        Function GetFunctionById(string funcId);

        /// <summary>
        /// get current record exists in Role.Functions
        /// </summary>
        /// <param name="roleId">string</param>
        /// <returns>FunctionDto</returns>
        IList<FunctionDto> GetFunctionsByRoleId(string roleId);
    }
}
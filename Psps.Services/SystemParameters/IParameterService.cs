using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;

namespace Psps.Services.SystemParameters
{
    /// <summary>
    /// System message service interface
    /// </summary>
    public partial interface IParameterService
    {
        /// <summary>
        /// Get system Parameter by id
        /// </summary>
        /// <param name="systemMessageId">System Parameter Id</param>
        /// <returns>System Parameter entity</returns>
        SystemParameter GetParameterById(int systemParameterId);

        /// <summary>
        /// Get system Parameter by code
        /// </summary>
        /// <param name="systemMessageId">code</param>
        /// <returns>System Parameter entity</returns>
        SystemParameter GetParameterByCode(string code);

        /// <summary>
        /// Updates the system Parameter
        /// </summary>
        /// <param name="SystemMessage">System Parameter entity</param>
        void UpdateParameter(SystemParameter systemParameter);

        /// <summary>
        /// List Parameter
        /// </summary>
        /// <param name="grid">jqGrid Parameter</param>
        /// <returns>Parameter</returns>
        IPagedList<SystemParameter> GetPage(GridSettings grid);
    }
}
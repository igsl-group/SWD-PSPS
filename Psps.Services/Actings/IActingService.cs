using Psps.Core;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Actings
{
    /// <summary>
    /// Acting service interface
    /// </summary>
    public partial interface IActingService
    {
        /// <summary>
        /// Marks Acting as deleted
        /// </summary>
        /// <param name="Acting">Acting</param>
        void DeleteActing(Acting Acting);

        /// <summary>
        /// List Actings by type
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<Acting> GetPage(GridSettings grid);

        /// <summary>
        /// Inserts a Acting
        /// </summary>
        /// <param name="Acting">Acting</param>
        void CreateActing(Acting Acting);

        /// <summary>
        /// Updates a Acting
        /// </summary>
        /// <param name="Acting">Acting</param>
        void UpdateActing(Acting Acting);

        /// <summary>
        /// Gets a Acting
        /// </summary>
        /// <param name="ActingId">Acting identifier</param>
        /// <returns>Acting</returns>
        Acting GetActingById(int ActingId);

        /// <summary>
        /// Gets Acting List
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>Actings</returns>
        IList<Acting> GetActingsByUserId(string userId);

        /// <summary>
        /// The combination of Assign To, Effective From, and Effective To must be unique
        /// </summary>
        /// <param name="userId">Assign To User id </param>
        /// <param name="fromDate">Assign To FromDate</param>
        /// <param name="toDate">Assign To toDate </param>
        /// <returns>bool</returns>
        bool ValidateIsAssigned(int actingId, string postId, DateTime fromDate, DateTime toDate);
    }
}
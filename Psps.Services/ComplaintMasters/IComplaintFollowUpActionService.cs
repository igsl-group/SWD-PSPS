using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.ComplaintMasters
{
    public partial interface IComplaintFollowUpActionService
    {
        /// <summary>
        /// List ComplaintFollowUpActions
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintFollowUpActions</returns>
        IPagedList<ComplaintFollowUpAction> GetPage(GridSettings grid);

        /// <summary>
        /// Create a ComplaintFollowUpAction
        /// </summary>
        /// <param name="complaintFollowUpAction">ComplaintFollowUpAction</param>
        void Create(ComplaintFollowUpAction complaintFollowUpAction);

        /// <summary>
        /// Update a ComplaintFollowUpAction
        /// </summary>
        /// <param name="complaintFollowUpAction">ComplaintFollowUpAction</param>
        void Update(ComplaintFollowUpAction complaintFollowUpAction);

        /// <summary>
        /// Loically Delete a ComplaintFollowUpAction
        /// </summary>
        /// <param name="complaintFollowUpAction">ComplaintFollowUpAction</param>
        void Delete(ComplaintFollowUpAction complaintFollowUpAction);

        /// <summary>
        /// Get a ComplaintFollowUpAction
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ComplaintFollowUpAction</returns>
        ComplaintFollowUpAction GetById(int id);

        /// <summary>
        /// Generate EnclosureNum
        /// </summary>
        /// <returns>string</returns>
        string GenerateEnclosureNum(int complaintMasterId);
    }
}
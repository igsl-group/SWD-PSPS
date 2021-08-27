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
    public partial interface IComplaintPoliceCaseService
    {
        /// <summary>
        /// List ComplaintPoliceCases
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintPoliceCases</returns>
        IPagedList<ComplaintPoliceCase> GetPage(GridSettings grid);

        /// <summary>
        /// Create a ComplaintPoliceCase
        /// </summary>
        /// <param name="complaintPoliceCase">ComplaintPoliceCase</param>
        void Create(ComplaintPoliceCase complaintPoliceCase);

        /// <summary>
        /// Update a ComplaintPoliceCase
        /// </summary>
        /// <param name="complaintPoliceCase">ComplaintPoliceCase</param>
        void Update(ComplaintPoliceCase complaintPoliceCase);

        /// <summary>
        /// Logically delete a ComplaintPoliceCase
        /// </summary>
        /// <param name="complaintPoliceCase">ComplaintPoliceCase</param>
        void Delete(ComplaintPoliceCase complaintPoliceCase);

        /// <summary>
        /// Get a ComplaintPoliceCase
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ComplaintPoliceCase</returns>
        ComplaintPoliceCase GetById(int id);

        /// <summary>
        /// Generate RefNum
        /// </summary>
        /// <returns>string</returns>
        string GenerateRefNum(int complaintMasterId);
    }
}
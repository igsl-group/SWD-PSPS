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
    public partial interface IComplaintOtherDepartmentEnquiryService
    {
        /// <summary>
        /// List ComplaintOtherDepartmentEnquirys
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintOtherDepartmentEnquirys</returns>
        IPagedList<ComplaintOtherDepartmentEnquiry> GetPage(GridSettings grid);

        /// <summary>
        /// Create a complaintOtherDepartmentEnquiry
        /// </summary>
        /// <param name="complaintOtherDepartmentEnquiry">ComplaintOtherDepartmentEnquiry</param>
        void Create(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry);

        /// <summary>
        /// Update a ComplaintOtherDepartmentEnquiry
        /// </summary>
        /// <param name="complaintOtherDepartmentEnquiry">ComplaintOtherDepartmentEnquiry</param>
        void Update(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry);

        /// <summary>
        /// Loically delete a ComplaintOtherDepartmentEnquiry
        /// </summary>
        /// <param name="complaintOtherDepartmentEnquiry">ComplaintOtherDepartmentEnquiry</param>
        void Delete(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry);

        /// <summary>
        /// Get a ComplaintOtherDepartmentEnquiry
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ComplaintTelRecord</returns>
        ComplaintOtherDepartmentEnquiry GetById(int id);

        /// <summary>
        /// Generate RefNum
        /// </summary>
        /// <returns>string</returns>
        string GenerateRefNum();
    }
}
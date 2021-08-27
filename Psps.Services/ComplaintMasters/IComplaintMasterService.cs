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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.ComplaintMasters
{
    public partial interface IComplaintMasterService
    {
        /// <summary>
        /// Create a ComplaintMaster
        /// </summary>
        /// <param name="model">ComplaintMaster</param>
        void Create(ComplaintMaster complaintMaster);

        /// <summary>
        /// Update a ComplaintMaster
        /// </summary>
        /// <param name="model">ComplaintMaster</param>
        void Update(ComplaintMaster complaintMaster);

        /// <summary>
        /// Updates the ComplaintMaster
        /// </summary>
        /// <param name="oldComplaintMaster">The old ComplaintMaster</param>
        /// <param name="newComplaintMaster">The new ComplaintMaster</param>
        void Update(ComplaintMaster oldComplaintMaster, ComplaintMaster newComplaintMaster);

        /// <summary>
        /// List ComplaintMasters
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintMasters</returns>
        IPagedList<ComplaintMaster> GetPage(GridSettings grid);

        /// <summary>
        /// List ComplaintMasters
        /// </summary>
        /// <param name="grid,fundRaisingLocation">jqGrid parameters,string</param>
        /// <returns>ComplaintMasters</returns>
        IPagedList<ComplaintMaster> GetPage(GridSettings grid, string fundRaisingLocation);

        /// <summary>
        /// Generate ComplaintRef
        /// </summary>
        /// <returns>string</returns>
        string GenerateComplaintRef(int year);

        /// <summary>
        /// Get ComplaintMaster by Id
        /// </summary>
        /// <param name="complaintMasterId">int</param>
        /// <returns>ComplaintMaster</returns>
        ComplaintMaster GetComplaintMasterById(int complaintMasterId);

        /// <summary>
        /// List ComplaintMasterDto
        /// </summary>
        /// <param name="param">string</param>
        /// <returns>ComplaintMasterDtos</returns>
        IList<ComplaintMaster> GetRecordsByParam(string param);

        /// <summary>
        /// Delete a ComplaintMaster
        /// </summary>
        /// <param name="model">ComplaintMaster</param>
        void Delete(ComplaintMaster complaintMaster);

        /// <summary>
        /// Get OrgEditComplaintView Amount By OrgId
        /// </summary>
        /// <param name="complaintMasterId">int</param>
        /// <returns>int</returns>
        Hashtable CalEditEnquiryComplaintRelevantRecordsAmount(int complaintMasterId);

        /// <summary>
        /// List ComplaintMasterSearchDtos
        /// </summary>
        /// <param name="grid,fundRaisingLocation">jqGrid parameters,string</param>
        /// <returns>ComplaintMasterSearchDto</returns>
        IPagedList<ComplaintMasterSearchDto> GetPageByComplaintMasterSearchDto(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OthersFollowUpIndicator);

        IPagedList<ComplaintMasterSearchView> GetPageByComplaintMasterSearchView(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OthersFollowUpIndicator,
                                                                                 DateTime? followUpFromDate = null, DateTime? followUpToDate = null);

        ComplaintMaster GetByComplaintRef(string complaintRef);
    }
}
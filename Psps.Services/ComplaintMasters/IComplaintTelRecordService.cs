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
    public partial interface IComplaintTelRecordService
    {
        /// <summary>
        /// List ComplaintTelRecords
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintTelRecords</returns>
        IPagedList<ComplaintTelRecord> GetPage(GridSettings grid);

        /// <summary>
        /// Create a ComplaintTelRecord
        /// </summary>
        /// <param name="complaintTelRecord">ComplaintTelRecord</param>
        void Create(ComplaintTelRecord complaintTelRecord);

        /// <summary>
        /// Update a ComplaintTelRecord
        /// </summary>
        /// <param name="complaintTelRecord">ComplaintTelRecord</param>
        void Update(ComplaintTelRecord complaintTelRecord);

        /// <summary>
        /// Logically mark the record to be deleted
        /// </summary>
        /// <param name="complaintTelRecord"></param>
        void Delete(ComplaintTelRecord complaintTelRecord);

        /// <summary>
        /// Get a ComplaintTelRecord
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ComplaintTelRecord</returns>
        ComplaintTelRecord GetById(int id);

        /// <summary>
        /// Generate TelComplaintRef
        /// </summary>
        /// <returns>string</returns>
        string GenerateTelComplaintRef(int year);
    }
}
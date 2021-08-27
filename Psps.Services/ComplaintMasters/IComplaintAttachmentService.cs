using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Dto.Complaint;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.ComplaintMasters
{
    public partial interface IComplaintAttachmentService
    {

        /// <summary>
        /// List ComplaintAttachments
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>ComplaintAttachments</returns>
        IPagedList<ComplaintAttachment> GetPage(GridSettings grid);

        /// <summary>
        /// Create a ComplaintAttachment
        /// </summary>
        /// <param name="complaintAttachment">ComplaintAttachment</param>
        void Create(ComplaintAttachment complaintAttachment);

        /// <summary>
        /// Update a ComplaintAttachment
        /// </summary>
        /// <param name="complaintAttachment">ComplaintAttachment</param>
        void Update(ComplaintAttachment complaintAttachment);

        /// <summary>
        /// Get a ComplaintAttachment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ComplaintAttachment</returns>
        ComplaintAttachment GetById(int id);

        /// <summary>
        /// Delete a ComplaintAttachment
        /// </summary>
        /// <param name="complaintAttachment">ComplaintAttachment</param>
        void Delete(ComplaintAttachment complaintAttachment);
    }
}

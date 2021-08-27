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
    public partial interface IComplaintResultService
    {
        void Create(ComplaintResult complaintResult);

        void Delete(ComplaintResult complaintResult);

        void Update(ComplaintResult complaintResult);

        IPagedList<ComplaintResult> GetPageByComplaintMasterId(GridSettings Grid, int complaintMasterId);

        ComplaintResult GetComplaintResultById(int complaintResultId);

        bool IsDistinctByCmIdAndResult(int complaintMasterId, int? complaintResultId, IList<string> nonComplianceNature);
    }
}
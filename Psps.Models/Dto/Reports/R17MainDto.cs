using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17MainDto : BaseReportDto
    {
        public string ComplaintRef { get; set; }

        public string ComplaintRecordType { get; set; }

        public string ComplaintSource { get; set; }

        public string ComplaintSourceRemark { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public string ComplaintTime { get; set; }

        public DateTime? FirstComplaintDate { get; set; }

        public string SwdUnit { get; set; }

        public string ComplainantName { get; set; }

        public string ComplainantTelNum { get; set; }

        public string OrgName { get; set; }

        public string PspPermitNum { get; set; }

        public string FdPermitNum { get; set; }

        public string ActivityConcern { get; set; }

        public string CollectionMethod { get; set; }

        public DateTime? RaisingDate { get; set; }

        public string RaisingTime { get; set; }

        public string FundRaisingLocation { get; set; }

        public int? FundRaiserInvolve { get; set; }

        public string ComplaintContentRemark { get; set; }

        public string ProcessingStatus { get; set; }

        public string ComplaintResultRemark { get; set; }

        public DateTime? ReplyDueDate { get; set; }

        public string RelatedComplaintRef { get; set; }

        public DateTime? WithholdingBeginDate { get; set; }

        public DateTime? WithholdingEndDate { get; set; }

        public string WithholdingRemark { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R16PspDto : BaseDto
    {
        public int OrgId { get; set; }

        public string EngOrgName { get; set; }

        public DateTime? EventEndDate { get; set; }

        public string PermitNum { get; set; }

        public string FileRef { get; set; }

        public DateTime? WithholdingBeginDate { get; set; }

        public DateTime? WithholdingEndDate { get; set; }

        public string ComplaintIndicator { get; set; }

        public string PoliceIndicator { get; set; }

        public string AuditedReportIndicator { get; set; }

        public string OfficialReceiptIndicator { get; set; }

        public string NewspaperCuttingIndicator { get; set; }

        public string FileRefLmNum { get; set; }

        public string QualityOpinionDetail { get; set; }

        public string DocRemark { get; set; }
    }
}
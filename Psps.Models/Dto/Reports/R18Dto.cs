using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R18Dto : BaseReportDto
    {
        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string OrgRef { get; set; }

        public int ComplaintsReceivedB4 { get; set; }

        public DateTime? ComplaintsReceivedSince { get; set; }

        public int NoOfWarningLetterB4 { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public string PspRef { get; set; }

        public string PermitNum { get; set; }

        public DateTime? EventPeriodFrom { get; set; }

        public DateTime? EventPeriodTo { get; set; }

        public int NoOfEvents { get; set; }

        public int ComplaintsReceivedAF { get; set; }

        public int NoOfWarningLetterAF { get; set; }

        public string PspRef2nd { get; set; }

        public DateTime? PermitIssueDate2nd { get; set; }

        public string PermitNum2nd { get; set; }

        public DateTime? EventPeriodFrom2nd { get; set; }

        public DateTime? EventPeriodTo2nd { get; set; }

        public int NoOfEvents2nd { get; set; }

        public DateTime? RejectionLetterDate { get; set; }

        public DateTime? RepresentationReceiveDate { get; set; }

        public string Remark { get; set; }
    }
}
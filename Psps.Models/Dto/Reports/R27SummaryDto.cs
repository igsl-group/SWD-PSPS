using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R27SummaryDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int PspApplyParent { get; set; }
        public int PspApplyAmend { get; set; }

        public int PspApprovalParent { get; set; }
        public int PspApprovalAmend { get; set; }

        public int PspFailParent { get; set; }
        public int PspFailAmend { get; set; }

        public int PspEventParent { get; set; }
        public int PspEventAmend { get; set; }

        public decimal PspGrossProceedMParent { get; set; }
        public decimal PspGrossProceedMAmend { get; set; }

        public decimal PspNetProceedMParent { get; set; }
        public decimal PspNetProceedMAmend { get; set; }
    }
}
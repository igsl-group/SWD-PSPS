using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R27SummaryFinDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int SsafApplyNormal { get; set; }

        public int SsafApplyAmend { get; set; }

        public int SsafApprovalNormal { get; set; }

        public int SsafApprovalAmend { get; set; }

        public int SsafFailNormal { get; set; }

        public int SsafFailAmend { get; set; }

        public int SsafEventNormal { get; set; }

        public int SsafEventAmend { get; set; }

        public decimal SsafGrossProceedMNormal { get; set; }

        public decimal SsafGrossProceedMAmend { get; set; }

        public decimal SsafNetProceedMNormal { get; set; }

        public decimal SsafNetProceedMAmend { get; set; }
    }
}
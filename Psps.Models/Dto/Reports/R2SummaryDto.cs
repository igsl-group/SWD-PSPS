using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R2SummaryDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int PspApply { get; set; }

        public int PspApproval { get; set; }

        public int PspFail { get; set; }

        public int PspEvent { get; set; }

        public decimal PspGrossProceedM { get; set; }

        public decimal PspNetProceedM { get; set; }

        public int ApplyTWR { get; set; }

        public int ApplyRFD { get; set; }

        public int FailTWR { get; set; }

        public int FailRFD { get; set; }

        public int FlagDayTWR { get; set; }

        public int FlagDayRFD { get; set; }

        public decimal SumGrossTWRM { get; set; }

        public decimal SumGrossRFDM { get; set; }

        public decimal SumNetTWRM { get; set; }

        public decimal SumNetRFDM { get; set; }

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
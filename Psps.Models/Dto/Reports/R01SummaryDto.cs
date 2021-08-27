using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R01SummaryDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int PspApply { get; set; }

        public int PspApproval { get; set; }

        public int PspFail { get; set; }

        public int PspEvent { get; set; }

        public decimal PspGrossProceedM { get; set; }

        public decimal PspNetProceedM { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R2SummaryDisasterDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public string DisasterName { get; set; }

        public int Approval { get; set; }

        public int Event { get; set; }

        public decimal NetProceed { get; set; }
    }
}
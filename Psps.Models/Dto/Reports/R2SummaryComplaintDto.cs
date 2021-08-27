using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R2SummaryComplaintDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int Police { get; set; }

        public int Convicted { get; set; }

        public int NFA { get; set; }

        public int VerbalWarning { get; set; }

        public int WrittenWarning { get; set; }

        public int VerbalAdvice { get; set; }

        public int WrittenAdvice { get; set; }

        public int NoResult { get; set; }
    }
}
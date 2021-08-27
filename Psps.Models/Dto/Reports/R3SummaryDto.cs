using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R3SummaryDto : BaseReportDto
    {
        public string FdYear { get; set; }

        public int FlagDayTWR { get; set; }

        public int TWRSat { get; set; }

        public int TWRWeekday { get; set; }

        public int TWRWeekdayPledging { get; set; }

        public int ApplyTWR { get; set; }

        public int OrgTWR { get; set; }

        public decimal SumTWR { get; set; }

        public int FlagDayRFD { get; set; }

        public int RFDSat { get; set; }

        public int RFDWeekday { get; set; }

        public int RFDWeekdayPledging { get; set; }

        public int ApplyRFD { get; set; }

        public int OrgRFD { get; set; }

        public decimal SumRFD { get; set; }
    }
}
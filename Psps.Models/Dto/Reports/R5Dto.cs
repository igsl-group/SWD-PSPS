using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R05_PspSummaryDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int PspApproved { get; set; }

        public int PspCalApproved { get; set; }

        public int PspOrgCnt { get; set; }

        public decimal NetProceed { get; set; }
    }

    public partial class R05_FdSummaryDto : BaseReportDto
    {
        public int FdYear { get; set; }

        public int FlagDayTWR { get; set; }

        public int FlagDayRFD { get; set; }

        public decimal NetProceed { get; set; }
    }

    public partial class R05_SummaryDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int Permit { get; set; }

        public int OrgCnt { get; set; }

        public decimal NetProceed { get; set; }
    }

    public partial class R05_ComplaintDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int Permit { get; set; }

        public int OrgCnt { get; set; }

        public decimal NetProceed { get; set; }

        public int ComplaintCnt { get; set; }

        public int ChargeCnt { get; set; }
    }

    public partial class R05_SentenceDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public string SentenceDetail { get; set; }

        public string OrgName { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R6MonthlyDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int Level { get; set; }

        public int PspReceived { get; set; }

        public int PspNotRequired { get; set; }

        public int PspRejected { get; set; }

        public int PspRevoked { get; set; }

        public int EventCancelled { get; set; }

        public int AppWithdrawn { get; set; }

        public int CaseClose { get; set; }

        public int CaseCloseOthers { get; set; }

        public int PspApproved { get; set; }

        public int PspSubvented { get; set; }

        public int NoOfEvent { get; set; }

        public int UnderProcess { get; set; }

        public decimal GrossProceed { get; set; }

        public decimal NetProceed { get; set; }

        public int PoliceCase { get; set; }

        public int NFA { get; set; }

        public int Verbal { get; set; }

        public int Convicted { get; set; }
    }
}
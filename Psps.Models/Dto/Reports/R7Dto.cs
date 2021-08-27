using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R7Dto : BaseDto
    {
        public int DisasterMasterId { get; set; }

        public string DisasterName { get; set; }

        public string ChiDisasterName { get; set; }

        public DateTime? DisasterDate { get; set; }

        public string ProcessPeriod { get; set; }

        public int OrgSect88Ind { get; set; }

        public int OrgNonSect88Ind { get; set; }

        public int PspSect88Ind { get; set; }

        public int PspNonSect88Ind { get; set; }

        public int PermitSect88Issued { get; set; }

        public int PermitNonSect88Issued { get; set; }

        public int EventCount { get; set; }

        public decimal NetProceed { get; set; }

        public int AccRequiredCount { get; set; }

        public int AllCheckedInd { get; set; }

        public int WithholdingListIndicator { get; set; }
        
        public int ComplaintSect88Count { get; set; }

        public int ComplaintNonSect88Count { get; set; }

        public int Cond8Sect88Count { get; set; }

        public int Cond8NonSect88Count { get; set; }

        public int AllDocOsSect88Count { get; set; }

        public int AllDocOsNonSect88Count { get; set; }

        public int AcProblemSect88Count { get; set; }

        public int AcProblemNonSect88Count { get; set; }
    }
}
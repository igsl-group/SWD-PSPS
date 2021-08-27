using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R21ARDto : BaseDto
    {
        public string PspYear { get; set; }

        public string OrgRef { get; set; }

        public bool SubventedIndicator { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string PspRef { get; set; }

        public string PermitNum { get; set; }

        public decimal? GrossProceed { get; set; }

        public decimal? Expenditure { get; set; }

        public decimal? NetProceed { get; set; }

        public decimal? ExpenditurePercent { get; set; }

        public string DocRemark { get; set; }

    }
}
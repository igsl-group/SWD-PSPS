using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R23Dto : BaseReportDto
    {
        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string OrgRef { get; set; }

        public string PspYear { get; set; }

        public string R1 { get; set; }

        public string R2 { get; set; }

        public string R3 { get; set; }

        public string R4 { get; set; }

        public DateTime? AfrDate { get; set; }

        public string PermitNums { get; set; }

        public decimal? OrgAnnualIncome { get; set; }

        public DateTime? LastVersionDate { get; set; }

        public string BelowInd { get; set; }
    }
}
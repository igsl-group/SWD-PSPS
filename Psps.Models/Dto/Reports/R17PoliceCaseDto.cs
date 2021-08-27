using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17PoliceCaseDto : BaseReportDto
    {
        public string RefNum { get; set; }

        public DateTime? ReferralDate { get; set; }

        public DateTime? MemoDate { get; set; }

        public string ConcernOrgName { get; set; }

        public string InvestigationResult { get; set; }

        public string ActionFile { get; set; }

        public string PoliceRefNum { get; set; }
    }
}
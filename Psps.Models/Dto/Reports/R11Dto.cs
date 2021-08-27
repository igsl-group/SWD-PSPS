using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R11Dto : BaseReportDto
    {
        public string OrgRef { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public int ComplaintCnt { get; set; }

        public string NonComplianceNature { get; set; }

        public int DisplayOrder { get; set; }

        public int Cnt { get; set; }
    }
}
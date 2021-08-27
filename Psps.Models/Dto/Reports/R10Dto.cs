using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R10Dto : BaseReportDto
    {
        public string ComplaintDate { get; set; }

        public int ComplaintCnt { get; set; }

        public int OrgCnt { get; set; }

        public string NonComplianceNature { get; set; }

        public int DisplayOrder { get; set; }

        public int Cnt { get; set; }
    }
}
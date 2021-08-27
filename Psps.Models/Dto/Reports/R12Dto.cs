using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R12Dto : BaseReportDto
    {
        public string ComplaintDate { get; set; }

        public int ComplaintCnt { get; set; }

        public int OrgCnt { get; set; }

        public string ComplaintResult { get; set; }

        public int DisplayOrder { get; set; }

        public int Cnt { get; set; }
    }
}
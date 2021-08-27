using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17ResultDto : BaseReportDto
    {
        public string NonComplianceNature { get; set; }

        public string OtherNonComplianceNature { get; set; }

        public string Result { get; set; }
    }
}
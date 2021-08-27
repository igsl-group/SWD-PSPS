using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17TelDto : BaseReportDto
    {
        public string TelComplaintRef { get; set; }

        public DateTime? TelComDate { get; set; }

        public string TelComTime { get; set; }

        public string TelComName { get; set; }

        public string TelComTelNum { get; set; }

        public string TelOrgName { get; set; }

        public string TelPspPermit { get; set; }

        public string TelFdPermit { get; set; }

        public string TelComplaintContentRemark { get; set; }

        public string OfficerName { get; set; }

        public string OfficerPost { get; set; }
    }
}
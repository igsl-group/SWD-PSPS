using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17FollowUpLetterDto : BaseReportDto
    {
        public string FollowUpLetterType { get; set; }

        public int Cnt { get; set; }

        public string IssueDates { get; set; }
    }
}
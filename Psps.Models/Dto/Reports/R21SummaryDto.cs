using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R21SummaryDto : BaseDto
    {
        public string PspYear { get; set; }

        public int? PspReceived { get; set; }

        public int? StatCount1 { get; set; }

        public int? StatCount2 { get; set; }

        public int? StatCount3 { get; set; }

        public int? StatCount4 { get; set; }

        public int? StatCount5 { get; set; }

    }
}
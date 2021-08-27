using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R7PspViewDto : BaseDto
    {
        public string PspYear { get; set; }

        public int? ApplicationCount { get; set; }

        public int? PermitNumCount { get; set; }

        public int? EventInvolveCount { get; set; }

        public decimal? NetAmountRaised { get; set; }
    }
}
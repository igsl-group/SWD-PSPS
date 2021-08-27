using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R7FdViewDto : BaseDto
    {
        public string FdYear { get; set; }

        public int? TWFD_TWRCount { get; set; }

        public int? FRD_TWRCount { get; set; }

        public int? TWFD_PermitNumCount { get; set; }

        public int? FRD_PermitNumCount { get; set; }

        public decimal? TWFD_NetAmountRaised { get; set; }

        public decimal? FRD_NetAmountRaised { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R25SummaryDto : BaseDto
    {
        public int Sort { get; set; }

        public int Cnt { get; set; }

        public int FullyAdopt { get; set; }

        public int PartiallyAdopt { get; set; }

        public int WillNotAdopt { get; set; }

        public int FullyAdoptNotInMailing { get; set; }

        public int PartiallyAdoptNotInMailing { get; set; }

        public int WillNotAdoptNotInMailing { get; set; }
    }
}
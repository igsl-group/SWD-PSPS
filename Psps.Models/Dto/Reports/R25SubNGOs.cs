using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R25SubNGOs : BaseDto
    {
        public string EngOrgName { get; set; }
        public string ChiOrgName { get; set; }
        public string FullyAdopt { get; set; }
        public string PartiallyAdopt { get; set; }
        public string WillNotAdopt { get; set; }
        public DateTime? ReplySlipReceiveDate { get; set; }
    }
}
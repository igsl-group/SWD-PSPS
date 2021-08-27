using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R22Dto : BaseDto
    {
        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string PspRef { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public string ApprovalType { get; set; }

        public DateTime? EventPeriodFrom { get; set; }

        public DateTime? EventPeriodTo { get; set; }

        public int FlagDayCount { get; set; }

        public int TotalEventCount { get; set; }
    }
}
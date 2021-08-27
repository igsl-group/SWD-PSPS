using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R16ComplaintDto : BaseDto
    {
        public int OrgId { get; set; }

        public string EngOrgName { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public string ComplaintSource { get; set; }

        public string ActivityConcern { get; set; }

        public string PermitNum { get; set; }

        public string ComplaintRef { get; set; }

        public DateTime? WithholdingBeginDate { get; set; }

        public DateTime? WithholdingEndDate { get; set; }

        public string WithholdingRemark { get; set; }

        public string OtherWithholdingRemark { get; set; }
    }
}
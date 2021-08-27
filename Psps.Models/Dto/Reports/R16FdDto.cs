using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R16FdDto : BaseDto
    {
        public int OrgId { get; set; }

        public string EngOrgName { get; set; }

        public DateTime? FlagDay { get; set; }

        public string TWR { get; set; }

        public string PermitNum { get; set; }

        public string FileRef { get; set; }

        public DateTime? WithholdingBeginDate { get; set; }

        public DateTime? WithholdingEndDate { get; set; }

        public string ComplaintIndicator { get; set; }

        public string PoliceIndicator { get; set; }

        public string ReviewReportIndicator { get; set; }

        public string NewspaperIndicator { get; set; }

        public string AfsReceiveIndicator { get; set; }

        public string DocReceiveRemark { get; set; }
    }
}
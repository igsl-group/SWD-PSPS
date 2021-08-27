using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R14Dto : BaseReportDto
    {
        public DateTime? ComplaintDate { get; set; }

        public string ComplainantName { get; set; }

        public string DcLcContent { get; set; }

        public string ConcernOrgName { get; set; }

        public string ConcernChiOrgName { get; set; }

        public string ComplaintEnclosureNum { get; set; }

        public string ComplaintPartNum { get; set; }

        public string ActionFileEnclosureNum { get; set; }

        public string ActionFilePartNum { get; set; }

        public string RecordTypeIndicator { get; set; }

        public string ComplaintResultRemark { get; set; }

        public bool FollowUp { get; set; }

        public bool ReportPolice { get; set; }

        public bool OtherFollowUp { get; set; }
    }
}
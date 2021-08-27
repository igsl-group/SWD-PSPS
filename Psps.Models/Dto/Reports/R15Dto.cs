using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R15Dto : BaseReportDto
    {
        public int SN { get; set; }

        public int InformedByPolice { get; set; }

        public DateTime? ReferralDate { get; set; }

        public DateTime? MemoDate { get; set; }

        public string ConcernOrgName { get; set; }

        public string ConcernChiOrgName { get; set; }

        public string CorrespondenceEnclosureNum { get; set; }

        public string CorrespondencePartNum { get; set; }

        public string PoliceRefNum { get; set; }

        public string CaseNature { get; set; }

        public string ResultDetail { get; set; }

        public string EnclosureNum { get; set; }

        public string PartNum { get; set; }

        public string Remark { get; set; }
    }
}
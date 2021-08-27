using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R19Dto : BaseReportDto
    {
        public DateTime? ComplaintDate { get; set; }

        public string OrgRef { get; set; }

        public string EngOrgNameSorting { get; set; }

        public string ChiOrgName { get; set; }

        public DateTime? FundRaisingDate { get; set; }

        public string ConvictedPersonName { get; set; }

        public string ConvictedPersonPosition { get; set; }

        public string OffenceDetail { get; set; }

        public string SentenceDetail { get; set; }

        public string PoliceRefNum { get; set; }

        public string CourtRefNum { get; set; }

        public DateTime? CourtHearingDate { get; set; }

        public string CaseReferralRemark { get; set; }

        public int InformedByPolice { get; set; }
    }
}
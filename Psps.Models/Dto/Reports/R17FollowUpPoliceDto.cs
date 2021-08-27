using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17FollwUpPoliceDto : BaseReportDto
    {
        public string EnclosureNum { get; set; }

        public DateTime? VDate { get; set; }

        public string PoliceStation { get; set; }

        public string PoliceOfficerName { get; set; }

        public string RnNum { get; set; }

        public string RnRemark { get; set; }

        public DateTime? RDate { get; set; }

        public string ReferralPoliceStation { get; set; }

        public string ActionFileRef { get; set; }

        public string PoliceInvestigation { get; set; }

        public string PoliceInvestigationResult { get; set; }

        public DateTime? PoliceReplyDate { get; set; }

        public string ConvictedPersonName { get; set; }

        public string ConvictedPersonPosition { get; set; }

        public string OffenceDetail { get; set; }

        public string SentenceDetail { get; set; }

        public string CourtRefNum { get; set; }

        public DateTime? CourtHearingDate { get; set; }

        public string PoliceRemark { get; set; }
    }
}
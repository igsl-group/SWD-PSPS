using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17FollowUpDto : BaseReportDto
    {
        public string EnclosureNum { get; set; }

        public string ContactOrgName { get; set; }

        public string CPersonName { get; set; }

        public DateTime? ContactDate { get; set; }

        public string OrgRemark { get; set; }

        public string FollowUpLetterType { get; set; }

        public string FollowUpLetterReceiver { get; set; }

        public DateTime? FollowUpLetterIssueDate { get; set; }

        public string FollowUpLetterRemark { get; set; }

        public string FollowUpLetterActionFileRef { get; set; }

        public string FollowUpOrgReply { get; set; }

        public DateTime? FollowUpOrgReplyDate { get; set; }
    }
}
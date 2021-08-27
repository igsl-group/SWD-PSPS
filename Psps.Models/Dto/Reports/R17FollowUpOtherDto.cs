using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17FollowUpOtherDto : BaseReportDto
    {
        public string EnclosureNum { get; set; }

        public string OtherFollowUpPartyName { get; set; }

        public string OtherFollowUpFileRef { get; set; }

        public DateTime? OtherFollowUpContactDate { get; set; }

        public string OtherFollowUpRemark { get; set; }
    }
}
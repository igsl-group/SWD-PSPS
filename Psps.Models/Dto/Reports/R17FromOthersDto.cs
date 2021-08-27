using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R17FromOthersDto : BaseReportDto
    {
        public string RefNum { get; set; }

        public DateTime? ReferralDate { get; set; }

        public DateTime? MemoDate { get; set; }

        public DateTime? MemoPoliceDate { get; set; }

        public string EnquiryDepartment { get; set; }

        public string OrgInvolved { get; set; }

        public string EnquiryContent { get; set; }

        public string EnclosureNum { get; set; }

        public string Remark { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R25AllDto : BaseDto
    {
        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string SendWithPsp { get; set; }

        public string Subvented { get; set; }

        public string FullyAdopt { get; set; }

        public string PartiallyAdopt { get; set; }

        public string WillNotAdopt { get; set; }

        public string Result { get; set; }

        public string A1 { get; set; }

        public string A2 { get; set; }

        public string A3 { get; set; }

        public string A4 { get; set; }

        public string A5 { get; set; }

        public string A6 { get; set; }

        public string A7 { get; set; }

        public string A8 { get; set; }

        public string B1 { get; set; }

        public string B2 { get; set; }

        public string B3 { get; set; }

        public string B4 { get; set; }

        public string B5 { get; set; }

        public string B6 { get; set; }

        public string C1 { get; set; }

        public string C2 { get; set; }

        public string C3 { get; set; }

        public string C4 { get; set; }

        public string C5 { get; set; }

        public string C6 { get; set; }

        public string C7 { get; set; }

        public string Others { get; set; }

        public string PromulgationReason { get; set; }

        public string ReturnMail { get; set; }

        public string Remarks { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? ReplySlipReceiveDate { get; set; }

        public string RemarkB { get; set; }

        public DateTime? ReplySlipDate { get; set; }

        public string PartNum { get; set; }

        public string EnclosureNum { get; set; }

        public string OrgRefGuidePromulgation { get; set; }
    }
}
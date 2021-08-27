using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Organisation
{
    public partial class OrgFlagDaySearchDto : BaseDto
    {
        public string OrgRef { get; set; }

        public string OrgName { get; set; }

        public string OrgEngName { get; set; }

        public string OrgChiName { get; set; }

        public bool SubventedIndicator { get; set; }

        public string FdRef { get; set; }

        public string PermitNum { get; set; }

        public DateTime? FlagDay { get; set; }

        public string TWR { get; set; }

        public bool? NewApplicantIndicator { get; set; }

        public string ApplicationResult { get; set; }

        public string Id
        {
            get
            {
                return OrgRef;
            }
            set
            {
                OrgRef = value;
            }
        }
    }
}
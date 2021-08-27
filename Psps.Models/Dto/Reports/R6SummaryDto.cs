using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R6SummaryDto : BaseReportDto
    {
        public DateTime? ApplicationReceiveDate { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public int PspNonSect88Ind { get; set; }

        public int PspSect88Ind { get; set; }

        public int OrgNonSect88Ind { get; set; }

        public int OrgSect88Ind { get; set; }

        public string BeneficiaryOrg { get; set; }

        public string ProcessingOfficerPost { get; set; }

        public int AppNum { get; set; }

        public int PermitIssued { get; set; }

        public DateTime? ApplicationDisposalDate { get; set; }

        public string PermitNum { get; set; }

        public string PspRef { get; set; }

        public DateTime? EventPeriodFrom { get; set; }

        public DateTime? EventPeriodTo { get; set; }

        public string SpecialRemark { get; set; }

        public int PspNotReqInd { get; set; }

        public int AppWithdrawnInd { get; set; }

        public int AppRejectInd { get; set; }

        public int CloseMergeInd { get; set; }

        public int CloseOtherInd { get; set; }

        public int AllCheckedInd { get; set; }

        public Boolean WithholdingListIndicator { get; set; }

        public decimal NetProceed { get; set; }
    }
}
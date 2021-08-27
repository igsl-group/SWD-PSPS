using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Psp
{
    public partial class PspSearchDto : BaseDto
    {
        /// <summary>
        /// grid properties
        /// </summary>
        public string PspYear { get; set; }

        public int PspMasterId { get; set; }

        public string SortPspRef { get; set; }

        public string OrgRef { get; set; }

        public string OrgName { get; set; }

        public string EngOrgNameSorting { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public string PspRef { get; set; }

        public DateTime? ApplicationReceiveDate { get; set; }

        public bool? DisableIndicator { get; set; } // 1 postponed, 0 active

        public bool? SubventedIndicator { get; set; }

        //public bool? AddressProofIndicator { get; set; }

        //public bool? AnnualReportIndicator { get; set; }

        //public bool? AfsIndicator { get; set; }

        public string ContactPersonChiName { get; set; }

        public string ContactPersonName { get; set; }

        public bool? Section88Indicator { get; set; }

        public string RegType1 { get; set; }

        public string RegOtherName1 { get; set; }

        public string RegType2 { get; set; }

        public string RegOtherName2 { get; set; }

        public string PermitNum { get; set; }

        public DateTime? EventStartDate { get; set; }

        public DateTime? EventEndDate { get; set; }

        public int? TotalLocation{ get; set; }

        public int? TotEvent { get; set; }

        public string ApprovalStatus { get; set; }

        public string RejectReason { get; set; }

        public string RejectRemark { get; set; }

        public int? PspApprovalHistoryId { get; set; }

        public int? EventApprovedNum { get; set; }

        public int? EventHeldNum { get; set; }

        public int? EventCancelledNum { get; set; }

        public decimal? EventHeldPercent { get; set; }

        public bool? OverdueIndicator { get; set; }

        public bool? LateIndicator { get; set; }

        public DateTime? ApplicationDisposalDate { get; set; }

        public DateTime? ApplicationCompletionDate { get; set; }

        public string QualityOpinionDetail { get; set; }

        public string ProcessingOfficerPost { get; set; }

        public string ContactPersonEmailAddress { get; set; }

        public string ContactPerson { get; set; }

        public string EngRegisteredAddress1 { get; set; }

        public string EngRegisteredAddress2 { get; set; }

        public string ChiRegisteredAddress1 { get; set; }

        public string ChiRegisteredAddress2 { get; set; }

        public string OrgEmailAddress { get; set; }

        public int? PreviousPspMasterId { get; set; }

        public string PreviousPspRef { get; set; }

        public bool? NewApplicantIndicator { get; set; }

        public int? DisasterId { get; set; }

        public int ReSubmit { get; set; }

        public int ReEvents { get; set; }
        
        public DateTime? ApplicationDate { get; set; }

        public string BeneficiaryOrg { get; set; }
        
        public string SpecialRemark { get; set; }

        public bool? IsSsaf { get; set; }

        public int Id
        {
            get
            {
                return PspMasterId;
            }
            set
            {
                PspMasterId = value;
            }
        }
    }
}
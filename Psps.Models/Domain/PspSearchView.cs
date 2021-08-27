using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspSearchView : BaseEntity<int>
    {
        public PspSearchView()
        {
            PspApprovalHistory = new List<PspApprovalHistory>();
            PspAttachment = new List<PspAttachment>();
            PspEvent = new List<PspEvent>();
        }

        public virtual int PspMasterId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string SortPspRef { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual string BeneficiaryOrg { get; set; }

        public virtual DateTime? EventStartDate { get; set; }

        public virtual DateTime? EventEndDate { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual int? TotalLocation { get; set; }

        public virtual int? TotEvent { get; set; }

        public virtual int? EventApprovedNum { get; set; }

        public virtual int? EventHeldNum { get; set; }

        public virtual int? EventCancelledNum { get; set; }

        public virtual decimal? EventHeldPercent { get; set; }

        public virtual bool? OverdueIndicator { get; set; }

        public virtual bool? LateIndicator { get; set; }

        public virtual string ApplicationResult { get; set; }

        public virtual string RejectReason { get; set; }

        public virtual string RejectRemark { get; set; }

        public virtual DateTime? ApplicationDisposalDate { get; set; }

        public virtual DateTime? ApplicationCompletionDate { get; set; }

        public virtual string QualityOpinionDetail { get; set; }

        public virtual string ProcessingOfficerPost { get; set; }

        public virtual IList<PspApprovalHistory> PspApprovalHistory { get; set; }

        public virtual IList<PspAttachment> PspAttachment { get; set; }

        public virtual IList<PspEvent> PspEvent { get; set; }

        public virtual string ContactPersonName { get; set; } // for mapping using formula  (ContactPersonFirstName + ContactPersonLastName)

        public virtual string ContactPersonChiName { get; set; } // for mapping using formula

        public virtual string ContactPersonEmailAddress { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual int? PreviousPspMasterId { get; set; }

        public virtual string PreviousPspRef { get; set; }

        public virtual bool? NewApplicantIndicator { get; set; }

        public virtual int? DisasterMasterId { get; set; }

        public virtual int ReSubmit { get; set; }

        public virtual int ReEvents { get; set; }

        public virtual DateTime? ApplicationDate { get; set; }

        public virtual string SpecialRemark { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual bool? IsSsaf { get; set; }

        public override int Id
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
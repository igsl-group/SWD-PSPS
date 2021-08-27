using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspMaster : BaseAuditEntity<int>
    {
        public PspMaster()
        {
            PspApprovalHistory = new List<PspApprovalHistory>();
            PspAttachment = new List<PspAttachment>();
            PspEvent = new List<PspEvent>();
        }

        public virtual int PspMasterId { get; set; }

        public virtual int? PreviousPspMasterId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual DisasterMaster DisasterMaster { get; set; }

        public virtual ReferenceGuideSearchView ReferenceGuideSearchView { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string ContactPersonSalute { get; set; }

        public virtual string ContactPersonFirstName { get; set; }

        public virtual string ContactPersonLastName { get; set; }

        public virtual string ContactPersonChiFirstName { get; set; }

        public virtual string ContactPersonChiLastName { get; set; }

        public virtual string ContactPersonPosition { get; set; }

        public virtual string ContactPersonTelNum { get; set; }

        public virtual string ContactPersonFaxNum { get; set; }

        public virtual string ContactPersonEmailAddress { get; set; }

        public virtual bool? NewApplicantIndicator { get; set; }

        public virtual string ProcessingOfficerPost { get; set; }

        public virtual string PspYear { get; set; }

        public virtual DateTime? EventPeriodFrom { get; set; }

        public virtual DateTime? EventPeriodTo { get; set; }

        public virtual bool? BypassValidationIndicator { get; set; }

        public virtual DateTime? ApplicationDate { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual DateTime? ApplicationCompletionDate { get; set; }

        public virtual DateTime? ActionBuDate { get; set; }

        public virtual string BeneficiaryOrg { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual string EngFundRaisingPurpose { get; set; }

        public virtual string ChiFundRaisingPurpose { get; set; }

        public virtual string EventTitle { get; set; }

        public virtual string EngCharitySalesItem { get; set; }

        public virtual string ChiCharitySalesItem { get; set; }

        public virtual string UsedLanguage { get; set; }

        public virtual DateTime? ApplicationDisposalDate { get; set; }

        public virtual string RejectReason { get; set; }

        public virtual string RejectRemark { get; set; }

        public virtual string OtherRejectReason { get; set; }

        public virtual string PspNotRequireReason { get; set; }

        public virtual string OtherPspNotRequireReason { get; set; }

        public virtual string CaseCloseReason { get; set; }

        public virtual string OtherCaseCloseReason { get; set; }

        public virtual string SpecialRemark { get; set; }

        public virtual string OtherSpecialRemark { get; set; }

        public virtual DateTime? RejectionLetterDate { get; set; }

        public virtual DateTime? RepresentationReceiveDate { get; set; }

        public virtual DateTime? RepresentationReplyDate { get; set; }

        public virtual DateTime? TrackRecordStartDate { get; set; }

        public virtual DateTime? TrackRecordEndDate { get; set; }

        public virtual string TrackRecordDetails { get; set; }

        public virtual DateTime? AfsRecordStartDate { get; set; }

        public virtual DateTime? AfsRecordEndDate { get; set; }

        public virtual string AfsRecordDetails { get; set; }

        public virtual string FundUsed { get; set; }

        public virtual string DocSubmission { get; set; }

        public virtual DateTime? SubmissionDueDate { get; set; }

        public virtual DateTime? FirstReminderIssueDate { get; set; }

        public virtual DateTime? FirstReminderDeadline { get; set; }

        public virtual DateTime? SecondReminderIssueDate { get; set; }

        public virtual DateTime? SecondReminderDeadline { get; set; }

        public virtual string AuditedReportReceivedDate { get; set; }

        public virtual string PublicationReceivedDate { get; set; }

        public virtual string OfficialReceiptReceivedDate { get; set; }

        public virtual string NewspaperCuttingReceivedDate { get; set; }

        public virtual string DocReceivedRemark { get; set; }

        public virtual decimal? GrossProceed { get; set; }

        public virtual decimal? Expenditure { get; set; }

        public virtual decimal? NetProceed { get; set; }

        public virtual decimal? OrgAnnualIncome { get; set; }

        public virtual bool SanctionListIndicator { get; set; }

        public virtual bool? QualifyOpinionIndicator { get; set; }

        public virtual string QualityOpinionDetail { get; set; }

        public virtual bool? WithholdingListIndicator { get; set; }

        public virtual string WithholdingRemark { get; set; }

        public virtual string ArCheckIndicator { get; set; }

        public virtual string PublicationCheckIndicator { get; set; }

        public virtual string OfficialReceiptCheckIndicator { get; set; }

        public virtual string NewspaperCheckIndicator { get; set; }

        public virtual bool? PermitRevokeIndicator { get; set; }

        public virtual string DocRemark { get; set; }

        public virtual IList<PspApprovalHistory> PspApprovalHistory { get; set; }

        public virtual IList<PspAttachment> PspAttachment { get; set; }

        public virtual IList<PspEvent> PspEvent { get; set; }

        public virtual string ContactPersonName { get; set; } // for mapping using formula  (ContactPersonFirstName + ContactPersonLastName)

        public virtual string ContactPersonChiName { get; set; } // for mapping using formula

        public virtual string ApplicationResult { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }
        
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
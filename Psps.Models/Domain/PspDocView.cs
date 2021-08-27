using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspDocView : BaseEntity<int>
    {
        public virtual DateTime? DocumentDate { get; set; }

        public virtual DateTime ThreeWeekAfterDocumentDate { get; set; }

        public virtual int PspMasterId { get; set; }

        public virtual int? PreviousPspMasterId { get; set; }

        public virtual int? OrgId { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual string EngRegisteredAddress1 { get; set; }

        public virtual string EngRegisteredAddress2 { get; set; }

        public virtual string EngRegisteredAddress3 { get; set; }

        public virtual string EngRegisteredAddress4 { get; set; }

        public virtual string EngRegisteredAddress5 { get; set; }

        public virtual string EngRegisteredAddressFull { get; set; }

        public virtual string ChiRegisteredAddress1 { get; set; }

        public virtual string ChiRegisteredAddress2 { get; set; }

        public virtual string ChiRegisteredAddress3 { get; set; }

        public virtual string ChiRegisteredAddress4 { get; set; }

        public virtual string ChiRegisteredAddress5 { get; set; }

        public virtual string ChiRegisteredAddressFull { get; set; }

        public virtual string EngMailingAddress1 { get; set; }

        public virtual string EngMailingAddress2 { get; set; }

        public virtual string EngMailingAddress3 { get; set; }

        public virtual string EngMailingAddress4 { get; set; }

        public virtual string EngMailingAddress5 { get; set; }

        public virtual string EngMailingAddressFull { get; set; }

        public virtual string ChiMailingAddress1 { get; set; }

        public virtual string ChiMailingAddress2 { get; set; }

        public virtual string ChiMailingAddress3 { get; set; }

        public virtual string ChiMailingAddress4 { get; set; }

        public virtual string ChiMailingAddress5 { get; set; }

        public virtual string ChiMailingAddressFull { get; set; }

        public virtual string TelNum { get; set; }

        public virtual string URL { get; set; }

        public virtual string ApplicantFirstName { get; set; }

        public virtual string ApplicantLastName { get; set; }

        public virtual string ApplicantEngSalute { get; set; }

        public virtual string ApplicantChiLastName { get; set; }

        public virtual string ApplicantChiFirstName { get; set; }

        public virtual string ApplicantChiSalute { get; set; }

        public virtual string ApplicantPosition { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string PreviousPspRef { get; set; }

        public virtual string PreviousPermitNum { get; set; }

        public virtual DateTime? PreviousPermitIssueDate { get; set; }

        public virtual string PreviousPermitNumList { get; set; }

        public virtual string ContactPersonFirstName { get; set; }

        public virtual string ContactPersonLastName { get; set; }

        public virtual string EngSalute { get; set; }

        public virtual string ContactPersonChiFirstName { get; set; }

        public virtual string ContactPersonChiLastName { get; set; }

        public virtual string ChiSalute { get; set; }

        public virtual string ContactPersonPosition { get; set; }

        public virtual string ContactPersonTelNum { get; set; }

        public virtual string ContactPersonFaxNum { get; set; }

        public virtual string ContactPersonEmailAddress { get; set; }

        public virtual bool? NewApplicantIndicator { get; set; }

        public virtual string EngProcessingOfficerPost { get; set; }

        public virtual string ChiProcessingOfficerPost { get; set; }

        public virtual string PspYear { get; set; }

        public virtual DateTime? FirstEventDate { get; set; }

        public virtual DateTime? LastEventDate { get; set; }

        public virtual DateTime? PendingFirstEventDate { get; set; }

        public virtual DateTime? PendingLastEventDate { get; set; }

        public virtual DateTime? EventPeriodFrom { get; set; }

        public virtual DateTime? EventPeriodTo { get; set; }

        public virtual DateTime? PreviousEventPeriodFrom { get; set; }

        public virtual DateTime? PreviousEventPeriodTo { get; set; }

        public virtual bool? BypassValidationIndicator { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual DateTime? ApplicationCompletionDate { get; set; }

        public virtual DateTime? ActionBuDate { get; set; }

        public virtual int? DisasterMasterId { get; set; }

        public virtual string BeneficiaryOrg { get; set; }

        public virtual string EngFundRaisingPurpose { get; set; }

        public virtual string ChiFundRaisingPurpose { get; set; }

        public virtual string EventTitle { get; set; }

        public virtual string EngCharitySalesItem { get; set; }

        public virtual string ChiCharitySalesItem { get; set; }

        public virtual string EngUsedLanguage { get; set; }

        public virtual string ChiUsedLanguage { get; set; }

        public virtual DateTime? ApplicationDisposalDate { get; set; }

        public virtual string EngApplicationResult { get; set; }

        public virtual string ChiApplicationResult { get; set; }

        public virtual string EngRejectReason { get; set; }

        public virtual string ChiRejectReason { get; set; }

        public virtual string OtherRejectReason { get; set; }

        public virtual string EngPspNotRequireReason { get; set; }

        public virtual string ChiPspNotRequireReason { get; set; }

        public virtual string OtherPspNotRequireReason { get; set; }

        public virtual string EngCaseCloseReason { get; set; }

        public virtual string ChiCaseCloseReason { get; set; }

        public virtual string OtherCaseCloseReason { get; set; }

        public virtual bool? PermitRevokeIndicator { get; set; }

        public virtual string EngSpecialRemark { get; set; }

        public virtual string ChiSpecialRemark { get; set; }

        public virtual string OtherSpecialRemark { get; set; }

        public virtual DateTime? TrackRecordStartDate { get; set; }

        public virtual DateTime? TrackRecordEndDate { get; set; }

        public virtual string TrackRecordDetails { get; set; }

        public virtual DateTime? AfsRecordStartDate { get; set; }

        public virtual DateTime? AfsRecordEndDate { get; set; }

        public virtual string AfsRecordDetails { get; set; }

        public virtual string FundUsed { get; set; }

        public virtual string EngDocSubmission { get; set; }

        public virtual string ChiDocSubmission { get; set; }

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

        public virtual bool? SanctionListIndicator { get; set; }

        public virtual bool? QualifyOpinionIndicator { get; set; }

        public virtual string QualityOpinionDetail { get; set; }

        public virtual bool? WithholdingListIndicator { get; set; }

        public virtual string ArCheckIndicator { get; set; }

        public virtual string PublicationCheckIndicator { get; set; }

        public virtual string OfficialReceiptCheckIndicator { get; set; }

        public virtual string NewspaperCheckIndicator { get; set; }

        public virtual string DocRemark { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        public virtual string EngSFCName { get; set; }

        public virtual string ChiSFCName { get; set; }

        public virtual string SFCTel { get; set; }

        public virtual string SFCEmail { get; set; }

        public virtual string EngEOIILF5Name { get; set; }

        public virtual string ChiEOIILF5Name { get; set; }

        public virtual string EOIILF5Tel { get; set; }

        public virtual string EOIILF5Email { get; set; }

        public virtual bool? DDAFormIndicator { get; set; }

        /* Singal Day Events */

        public virtual List<PspApprovedEvents> Proformas1 { get; set; }

        /* Continuous event 24hrs  */

        public virtual List<PspEvent> Proformas2 { get; set; }

        /* Continuous event but not 24hrs */

        public virtual List<PspEvent> Proformas3 { get; set; }

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
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdDocView : BaseEntity<int>
    {
        public virtual DateTime DocumentDate { get; set; }

        public virtual DateTime ThreeWeekAfterDocumentDate { get; set; }

        public virtual int FdMasterId { get; set; }

        public virtual int OrgId { get; set; }

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

        public virtual string ApplicantChiFirstName { get; set; }

        public virtual string ApplicantChiLastName { get; set; }

        public virtual string ApplicantPosition { get; set; }

        public virtual string ApplicantSalute { get; set; }

        public virtual string ApplicantChiSalute { get; set; }

        public virtual string ApplicantTelNum { get; set; }

        public virtual string OrgFaxNum { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string FdRefNo { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual string EngUsedLanguage { get; set; }

        public virtual string ChiUsedLanguage { get; set; }

        public virtual string ContactPersonSalute { get; set; }

        public virtual string ContactPersonFirstName { get; set; }

        public virtual string ContactPersonLastName { get; set; }

        public virtual string ContactPersonChiSalute { get; set; }

        public virtual string ContactPersonChiLastName { get; set; }

        public virtual string ContactPersonChiFirstName { get; set; }

        public virtual string ContactPersonName { get; set; }

        public virtual string ContactPersonChineseName { get; set; }

        public virtual string ContactPersonPosition { get; set; }

        public virtual string ContactPersonTelNum { get; set; }

        public virtual string ContactPersonFaxNum { get; set; }

        public virtual string ContactPersonEmailAddress { get; set; }

        public virtual string FundRaisingPurpose { get; set; }

        public virtual string ChiFundRaisingPurpose { get; set; }

        public virtual string ApplyForTwr { get; set; }

        public virtual string EngApplyForTwr { get; set; }

        public virtual string ChiApplyForTwr { get; set; }

        public virtual DateTime? FlagDay { get; set; }

        public virtual string TWR { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        public virtual string EngTWR { get; set; }

        public virtual string ChiTWR { get; set; }

        public virtual string TwrDistrict { get; set; }

        public virtual string EngTwrDistrict { get; set; }

        public virtual string ChiTwrDistrict { get; set; }

        public virtual string EngApplicationResult { get; set; }

        public virtual string ChiApplicationResult { get; set; }

        public virtual bool? VettingPanelCaseIndicator { get; set; }

        public virtual bool? ReviewCaseIndicator { get; set; }

        public virtual string FdYear { get; set; }

        public virtual string FdYearChi { get; set; }

        public virtual string FdYear1st { get; set; }

        public virtual DateTime FdYear1stDateFormat { get; set; }

        public virtual string FdYearHeaderFormat { get; set; }

        public virtual string LotGroup { get; set; }

        public virtual string EngFdCategory { get; set; }

        public virtual string ChiFdCategory { get; set; }

        public virtual string EngFdDistrict { get; set; }

        public virtual string ChiFdDistrict { get; set; }

        public virtual decimal? TargetIncome { get; set; }

        public virtual string TargetIncomeCurrencyFormat { get; set; }

        public virtual string TargetIncomeChi { get; set; }

        public virtual bool? NewApplicantIndicator { get; set; }

        public virtual DateTime? TrackRecordStartDate { get; set; }

        public virtual DateTime? TrackRecordEndDate { get; set; }

        public virtual string TrackRecordDetails { get; set; }

        public virtual DateTime? AfsRecordStartDate { get; set; }

        public virtual DateTime? AfsRecordEndDate { get; set; }

        public virtual string AfsRecordDetails { get; set; }

        public virtual string EngFdGroup { get; set; }

        public virtual string ChiFdGroup { get; set; }

        public virtual decimal? FdGroupPercentage { get; set; }

        public virtual string GroupingResult { get; set; }

        public virtual bool? JointApplicationIndicator { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual bool? CommunityChest { get; set; }

        public virtual bool? ConsentLetter { get; set; }

        public virtual string ApplicationRemark { get; set; }

        public virtual string FdLotNum { get; set; }

        public virtual string EngFdLotResult { get; set; }

        public virtual string ChiFdLotResult { get; set; }

        public virtual string PriorityNum { get; set; }

        public virtual DateTime? AcknowledgementReceiveDate { get; set; }

        public virtual bool? ApplyPledgingMechanismIndicator { get; set; }

        public virtual decimal? PledgedAmt { get; set; }

        public virtual string PledgedAmtCurrencyFormat { get; set; }

        public virtual string PledgedAmtChi { get; set; }

        public virtual decimal? PledgingAmt { get; set; }

        public virtual string PledgingAmtCurrencyFormat { get; set; }

        public virtual string PledgingAmtChi { get; set; }

        public virtual string PledgingProposal { get; set; }

        public virtual string ChiPledgingProposal { get; set; }

        public virtual string PledgingApplicationRemark { get; set; }

        public virtual string EngDocSubmission { get; set; }

        public virtual string ChiDocSubmission { get; set; }

        public virtual DateTime? SubmissionDueDate { get; set; }

        public virtual DateTime? FirstReminderIssueDate { get; set; }

        public virtual DateTime? FirstReminderDeadline { get; set; }

        public virtual DateTime? SecondReminderIssueDate { get; set; }

        public virtual DateTime? SecondReminderDeadline { get; set; }

        public virtual string AuditReportReceivedDate { get; set; }

        public virtual string PublicationReceivedDate { get; set; }

        public virtual string DocReceiveRemark { get; set; }

        public virtual string DocRemark { get; set; }

        public virtual decimal? StreetCollection { get; set; }

        public virtual string StreetCollectionCurrencyFormat { get; set; }

        public virtual string StreetCollectionChi { get; set; }

        public virtual decimal? GrossProceed { get; set; }

        public virtual string GrossProceedCurrencyFormat { get; set; }

        public virtual string GrossProceedChi { get; set; }

        public virtual decimal? Expenditure { get; set; }

        public virtual string ExpenditureCurrencyFormat { get; set; }

        public virtual string ExpenditureChi { get; set; }

        public virtual decimal? NetProceed { get; set; }

        public virtual string NetProceedCurrencyFormat { get; set; }

        public virtual string NetProceedChi { get; set; }

        public virtual string ExpenditurePerGrossProceed { get; set; }

        public virtual DateTime? NewspaperPublishDate { get; set; }

        public virtual DateTime? AcknowledgementEmailIssueDate { get; set; }

        public virtual bool? AfsReceiveIndicator { get; set; }

        public virtual bool? RequestPermitteeIndicator { get; set; }

        public virtual bool? AfsReSubmitIndicator { get; set; }

        public virtual bool? AfsReminderIssueIndicator { get; set; }

        public virtual bool? WithholdingListIndicator { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }

        public virtual string Remark { get; set; }

        public virtual string EngEOLF2Name { get; set; }

        public virtual string ChiEOLF2Name { get; set; }

        public virtual string EngEOIILF5Name { get; set; }

        public virtual string ChiEOIILF5Name { get; set; }

        public virtual string EngCEOLFName { get; set; }

        public virtual string ChiCEOLFName { get; set; }

        public virtual string LastFdYear { get; set; }

        public virtual string LastFdYearChi { get; set; }

        public virtual DateTime? LastFlagDay { get; set; }

        public virtual decimal? LastNetProceed { get; set; }

        public virtual string LastNetProceedCurrencyFormat { get; set; }

        public virtual string LastNetProceedChi { get; set; }

        public virtual string Past1FdYear { get; set; }

        public virtual string Past2FdYear { get; set; }

        public virtual string Past3FdYear { get; set; }

        public virtual bool EligibleInCurrentYear { get; set; }

        public virtual bool EligibleInPast1Year { get; set; }

        public virtual bool EligibleInPast2Year { get; set; }

        public virtual bool EligibleInPast3Year { get; set; }

        public virtual bool AllEligibleIn3Year { get; set; }

        public virtual DateTime Next1stFdYearDateFormat { get; set; }

        public virtual DateTime Next2ndFdYearDateFormat { get; set; }

        public virtual DateTime Next3rdFdYearDateFormat { get; set; }

        public virtual DateTime Next4thFdYearDateFormat { get; set; }

        public virtual string BenchmarkRFD { get; set; }

        public virtual string BenchmarkTWFD { get; set; }

        public override int Id
        {
            get
            {
                return FdMasterId;
            }
            set
            {
                FdMasterId = value;
            }
        }
    }
}
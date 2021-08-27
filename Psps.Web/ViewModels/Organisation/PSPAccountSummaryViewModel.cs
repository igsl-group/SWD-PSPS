using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Web.ViewModels.Organisation
{
    //[Validator(typeof(PSPViewModelValidator))]
    public partial class PSPAccountSummaryViewModel : BaseViewModel
    {
        public string Id { get; set; }

        public bool isFirstSearch { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileReference")]
        public string FileReference { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrganisationCode")]
        public string OrganisationCode { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PositionofProcessingOfficer")]
        public string PositionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PositionofProcessingOfficer")]
        public IDictionary<string, string> Positions { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofReceivingApplication")]
        public string DateofReceivingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofCompletingApplication")]
        public string DateofCompletingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_BUDateforAction")]
        public string BUDateforAction { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section88")]
        public bool Section88 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PurposeofFundRaising")]
        public string PurposeofFundRaising { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TitleofEventofProposedActivity")]
        public string EventTitle { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CharitySalesItems")]
        public string CharitySalesItems { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LanguageUsed")]
        public string LanguageUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LanguageUsed")]
        public IDictionary<string, string> LanguageUseds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofApplicationDisposal")]
        public string DateofApplicationDisposal { get; set; }

        public string DateofApplicationDisposalFr { get; set; }

        public string DateofApplicationDisposalTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public string ApplicationResultId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public IDictionary<string, string> ApplicationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectReason")]
        public string RejectReasonId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectReason")]
        public IDictionary<string, string> RejectReasons { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectReason")]
        public string OtherRejectReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PspNotRequireReason")]
        public string PspNotRequireReasonId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PspNotRequireReason")]
        public IDictionary<string, string> PspNotRequireReasons { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PspNotRequireReason")]
        public string OtherPspNotRequireReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CaseCloseReason")]
        public string CaseCloseReasonId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CaseCloseReason")]
        public IDictionary<string, string> CaseCloseReasons { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CaseCloseReason")]
        public string OtherCaseCloseReason { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PermitRevoked")]
        public bool PermitRevoked { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationName")]
        public string ApplicationName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationDonation")]
        public string ApplicationDonation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationOthers")]
        public string ApplicationOthers { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OtherSpecialRemark")]
        public string OtherSpecialRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public string FundUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public string FundUsed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public IDictionary<string, string> FundUseds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public string DocSubmissionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public string DocSubmission { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public IDictionary<string, string> DocSubmissions { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SubmissionDueDate")]
        public string SubmissionDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FirstReminderIssueDate")]
        public string FirstReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FirstReminderDeadline")]
        public string FirstReminderDeadline { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SecondReminderIssueDate")]
        public string SecondReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SecondReminderDeadline")]
        public string SecondReminderDeadline { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AuditedReportReceivedDate")]
        public string AuditedReportReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PublicationReceivedDate")]
        public string PublicationReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OfficialReceiptReceivedDate")]
        public string OfficialReceiptReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewspaperCuttingReceivedDate")]
        public string NewspaperCuttingReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocReceivedRemark")]
        public string DocReceivedRemark { get; set; }

        public decimal? GrossProceed { get; set; }

        public decimal? Expenditure { get; set; }

        public decimal? NetProceed { get; set; }

        public decimal? OrgAnnualIncome { get; set; }

        public string SanctionListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PurposeofFund")]
        public string PurposeofFund { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualifyOpinionIndicator")]
        public string QualifyOpinionIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualityOpinionDetail")]
        public string QualityOpinionDetail { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingListIndicator")]
        public string WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingBeginDate")]
        public string WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingEndDate")]
        public string WithholdingEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemark { get; set; }

        public string ArCheckIndicator { get; set; }

        public string PublicationCheckIndicator { get; set; }

        public string OfficialReceiptCheckIndicator { get; set; }

        public string NewspaperCheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocRemark")]
        public string DocRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Overdue")]
        public string Overdue { get; set; }

        public IDictionary<string, string> Overdues { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Late")]
        public string Late { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventYear")]
        public string EventYear { get; set; }

        public IDictionary<string, string> EventYears { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventStartDate")]
        public string EventStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventEndDate")]
        public string EventEndDate { get; set; }

        #region PSP A/C Summary

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgName")]
        public string OrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TelNo")]
        public string TelNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Address")]
        public string Address { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public bool Subvented { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public string SubventedIndicatorId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public IDictionary<string, string> SubventedIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgStatus")]
        public string OrgStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgStatus")]
        public IDictionary<string, string> OrgStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public string SectionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public IDictionary<string, string> Sections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileReference")]
        public string PSPRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PrincipalActivities")]
        public string PrincipalActivities { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonName")]
        public string ContactPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationId { get; set; }

        public string Registration { get; set; }

        public string RegistrationOtherName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public IDictionary<string, string> Registrations { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RefGuide")]
        public IDictionary<string, string> RefGuides { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RefGuide")]
        public string RefGuideId { get; set; }

        public IDictionary<string, string> YesNo { get; set; }

        public IDictionary<string, string> CheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public string DisasterMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public IDictionary<string, string> DisasterNames { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventCancel")]
        public string EventCancel { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_IsSsaf")]
        //public bool? IsSsaf { get; set; }
        public string IsSsaf { get; set; }       

        #endregion PSP A/C Summary

        public byte[] RowVersion { get; set; }
    }
}
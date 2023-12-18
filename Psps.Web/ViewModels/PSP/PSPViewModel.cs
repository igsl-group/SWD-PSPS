using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using Psps.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Psps.Web.ViewModels.PSP
{
    [Validator(typeof(PSPViewModelValidator))]
    public partial class PSPViewModel : BaseViewModel
    {
        public PSPViewModel()
        {
            PspEventViewModel = new PspEventViewModel();
            PspAttachmentViewModel = new PspAttachmentViewModel();
        }

        //public bool IsShowNewApplicant { get; set; }
        
        public string OrgMasterId { get; set; }

        public bool isFirstSearch { get; set; }

        public string PrePage { get; set; }

        public string CreateMode { get; set; }

        public int PspMasterId { get; set; }

        public int? PrevPspMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileReference")]
        public string PSPRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PreviousFileReference")]
        public string PreviousPspRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileReference")]
        public int MaxRefSeqNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrganisationCode")]
        public string OrganisationCode { get; set; }

        public string OrgEngName { get; set; }

        public string OrgChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileReference")]
        public string CreateModelReferenceNumber { get; set; }

        public string PreviousReferenceNumber { get; set; }

        public String PreviousPermitNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string CreateModelOrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PositionofProcessingOfficer")]
        public string PositionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PositionofProcessingOfficer")]
        public IDictionary<string, string> Positions { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofEventPeriodFrom")]
        public DateTime? DateofEventPeriodFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofEventPeriodTo")]
        public DateTime? DateofEventPeriodTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofApplication")]
        public DateTime? DateofApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofReceivingApplication")]
        public DateTime? DateofReceivingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofReceivingApplication")]
        public DateTime? RecevAppFromDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "To")]
        public DateTime? RecevAppToDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofCompletingApplication")]
        public DateTime? DateofCompletingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofApplicationDisposal")]
        public DateTime? ApplicationCompletionDateFromDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "To")]
        public DateTime? ApplicationCompletionDateToDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_BUDateforAction")]
        public DateTime? BUDateforAction { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_BeneficiaryOrgId")]
        public string BeneficiaryOrg { get; set; }

        public string BeneficiaryOrgName { get; set; }

        public string BeneficiaryOrgChiName { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public bool? Section88 { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_IsSsaf")]
        public bool? IsSsaf { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public IDictionary<bool, string> Section88s { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public bool? CreateModelSection88 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PurposeofFundRaising")]
        public string PurposeofFundRaising { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PurposeofChiFundRaising")]
        public string PurposeofChiFundRaising { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TitleofEventofProposedActivity")]
        public string EventTitle { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CharitySalesItems")]
        public string CharitySalesItems { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_CharitySalesItemsChi")]
        public string CharitySalesItemsChi { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LanguageUsed")]
        public string LanguageUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LanguageUsed")]
        public IDictionary<string, string> LanguageUseds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PermitNo")]
        public int MaxRefPermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofApplicationDisposal")]
        public DateTime? DateofApplicationDisposal { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofApplicationDisposal")]
        public DateTime? DisposAppFromDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "To")]
        public DateTime? DisposAppToDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public string ApplicationResultId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public IDictionary<string, string> ApplicationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectReason")]
        public string RejectReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectReason")]
        public string RejectReasonId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RejectRemark")]
        public string RejectRemark { get; set; }

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

        //[UIHint("YesNo")]
        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TwoBatchApproachEx")]
        //public bool TwoBatchApproachEx { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_UnderPoliceInvest")]
        public bool UnderPoliceInvest { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_BreachedPspConds")]
        public bool BreachedPspConds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationName")]
        public string ApplicationName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationDonation")]
        public string ApplicationDonation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationOthers")]
        public string ApplicationOthers { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SpecialRemark")]
        public List<string> SpecialRemark { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SpecialRemark")]
        public string[] SpecialRemarkChkBx { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SpecialRemark")]
        public IDictionary<string, string> SpecialRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OtherSpecialRemark")]
        public string OtherSpecialRemark { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OtherSpecialRemark")]
        public bool OtherSpecialRemarkIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RejectionLetterDate")]
        public DateTime? RejectionLetterDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RepresentationReceiveRepliedDate")]
        //public DateTime? RepresentationReceiveRepliedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RepresentationReceiveDate")]
        public DateTime? RepresentationReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PspApprove_RepresentationReplyDate")]
        public DateTime? RepresentationReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TrackRecordStartDate")]
        public DateTime? TrackRecordStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TrackRecordEndDate")]
        public DateTime? TrackRecordEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TrackRecordDetails")]
        public string TrackRecordDetails { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AfsRecordStartDate")]
        public DateTime? AfsRecordStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AfsRecordEndDate")]
        public DateTime? AfsRecordEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AfsRecordDetails")]
        public string AfsRecordDetails { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public string FundUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public string FundUsed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FundUsed")]
        public IDictionary<string, string> FundUseds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public string DocSubmissionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public List<string> DocSubmission { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocSubmission")]
        public IDictionary<string, string> DocSubmissions { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SubmissionDueDate")]
        public DateTime? SubmissionDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FirstReminderIssueDate")]
        public DateTime? FirstReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FirstReminderDeadline")]
        public DateTime? FirstReminderDeadline { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SecondReminderIssueDate")]
        public DateTime? SecondReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SecondReminderDeadline")]
        public DateTime? SecondReminderDeadline { get; set; }

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

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_GrossProceed")]
        public decimal? GrossProceed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Expenditure")]
        public decimal? Expenditure { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NetProceed")]
        public decimal? NetProceed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ExpPerGpPercentage")]
        public decimal? ExpPerGpPercentage { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgAnnualIncome")]
        public decimal? OrgAnnualIncome { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SanctionListIndicator")]
        public bool SanctionListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SanctionListIndicator")]
        public string SanctionListId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_SanctionListIndicator")]
        public IDictionary<bool, string> SanctionListIndicators { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualifyOpinionIndicator")]
        public bool? QualifyOpinionIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualifyOpinionIndicator")]
        public string QualifyOpinionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualifyOpinionIndicator")]
        public IDictionary<bool, string> QualifyOpinionIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_QualityOpinionDetail")]
        public string QualityOpinionDetail { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingListIndicator")]
        public bool? WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingListIndicator")]
        public string WithholdingListId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingListIndicator")]
        public IDictionary<bool, string> WithholdingListIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingBeginDate")]
        public DateTime? WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingEndDate")]
        public DateTime? WithholdingEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemark { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ArCheckIndicator_Edit")]
        public string ArCheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ArCheckIndicator")]
        public string ArCheckId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ArCheckIndicator")]
        public IDictionary<string, string> ArCheckIndicators { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PublicationCheckIndicator_Edit")]
        public string PublicationCheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PublicationCheckIndicator")]
        public string PublicationCheckId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PublicationCheckIndicator")]
        public IDictionary<string, string> PublicationCheckIndicators { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OfficialReceiptCheckIndicator_Edit")]
        public string OfficialReceiptCheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OfficialReceiptCheckIndicator")]
        public string OfficialReceiptCheckId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OfficialReceiptCheckIndicator")]
        public IDictionary<string, string> OfficialReceiptCheckIndicators { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewspaperCuttingCheckIndicator_Edit")]
        public string NewspaperCuttingCheckIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewspaperCuttingCheckIndicator")]
        public string NewspaperCuttingCheckId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewspaperCuttingCheckIndicator")]
        public IDictionary<string, string> NewspaperCuttingCheckIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocRemark")]
        public string DocRemark { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TwoBatchApproachEx")]
        public bool TwoBatchEx { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Psp_Amendment")]
        public bool Amendment { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Psp_CancelEvent")]
        public bool CancelEvent { get; set; }

        public bool prevApHistExistInd { get; set; }

        public bool HasPspChild { get; set; }

        public int prevTwRecCount { get; set; }

        public int twEventsRecCount { get; set; }

        public int approvedEventsCount { get; set; }

        public bool? hasRecomApproFlag { get; set; }

        //public bool? hasCancelledFlag { get; set; }

        #region PSP A/C Summary

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgName")]
        public string OrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TelNo")]
        public string TelNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FaxNo")]
        public string FaxNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonEmailAddress")]
        public string ContactPersonEmailAddress { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Address")]
        public string Address { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public bool? Subvented { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public IDictionary<bool, string> Subventeds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public string SubventedIndicatorId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Subvented")]
        public IDictionary<string, string> SubventedIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgStatus")]
        public string OrgStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgStatus")]
        public IDictionary<string, string> OrgStatus { get; set; }

        //public IDictionary<bool, string> SearchOrgStatus { get; set; }

        //public IDictionary<string, string> SearchOrgStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public string SectionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section")]
        public IDictionary<string, string> Sections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PrincipalActivities")]
        public string PrincipalActivities { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonPost")]
        public string ContactPersonPost { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonName")]
        public string ContactPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonNameEng")]
        public string ContactPersonNameEng { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonNameChi")]
        public string ContactPersonNameChi { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonFirstName")]
        public string ContactPersonFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonLastName")]
        public string ContactPersonLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonChiName")]
        public string ContactPersonChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonFirstName")]
        public string ContactPersonChiFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ContactPersonLastName")]
        public string ContactPersonChiLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationId { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public bool? Registration { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration1")]
        public string RegistrationType1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration2")]
        public string RegistrationType2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationOtherName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationOtherName2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public IDictionary<string, string> Registrations { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RefGuide")]
        public IDictionary<string, string> RefGuides { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_RefGuide")]
        public string RefGuideId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventDate")]
        public DateTime? EvenStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventStartDate")]
        public DateTime? EvenStartFromDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EventStartDate")]
        public DateTime? EvenStartToDate { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_BypassValidation")]
        public bool BypassValidation { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EngSalutes")]
        public string EngSalute { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_EngSalutes")]
        public IDictionary<string, string> EngSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ChiSalutes")]
        public string ChiSalute { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ChiSalutes")]
        public IDictionary<string, string> ChiSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Grouping")]
        public string GroupingId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Grouping")]
        public IDictionary<string, string> Groupings { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplyForTwr")]
        public string ApplyForTwr { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplyForTwr")]
        public IDictionary<string, string> ApplyForTwrs { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AddressProofIndicator")]
        public bool AddressProofIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AnnualReportIndicator")]
        public bool AnnualReportIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AfsIndicator")]
        public bool AfsIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public IDictionary<string, string> PspApplicationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationResult")]
        public string PspApplicationResult { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TotEvent")]
        public bool TotEvent { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_VettingPanelCaseIndicator")]
        public bool VettingPanelCaseIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_YearofPsp")]
        public string YearofPsp { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_YearofPsp")]
        public IDictionary<string, string> YearofPspList { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_GroupingwithNoofLot")]
        public string GroupingwithNoofLot { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public int? DisasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "R6_Disaster")]
        public IDictionary<string, string> DisasterNames { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TargetIncome")]
        public decimal? TargetIncome { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewApplicant")]
        public bool NewApplicant { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AFS")]
        public string AFS { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PercentageforGrouping")]
        public decimal? PercentageForGrouping { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TheCommunityChestofHKMember")]
        public bool CommunityChest { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ConsentLetterfromtheCommunityChestofHK")]
        public bool ConsentLetter { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofOutstandingEmailIssued")]
        public DateTime? OutstandingEmailIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofOutstandingEmailReply")]
        public DateTime? OutstandingEmailReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofReminderforOutstandingEmailIssued")]
        public DateTime? OutstandingEmailReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DateofReminderforOutstandingEmailReply")]
        public DateTime? OutstandingEmailReminderReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplicationRemark")]
        public string ApplicationRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LotNum")]
        public string PspLotNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LogResult")]
        public string PspLogResult { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LogResult")]
        public IDictionary<string, string> PspLogResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PriorityNum")]
        public string PriorityNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ProposalDetail")]
        public string ProposalDetail { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ChiProposalDetail")]
        public string ChiProposalDetail { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FlagSalePurpose")]
        public string FlagSalePurpose { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ChiFlagSalePurpose")]
        public string ChiFlagSalePurpose { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AcknowledgementReceiveDate")]
        public DateTime? AcknowledgementReceiveDate { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ApplyPledgingMechanismIndicator")]
        public bool ApplyPledgingMechanismIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PledgingAmt")]
        public decimal? PledgingAmt { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PledgedAmt")]
        public decimal? PledgedAmt { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PledgingProposal")]
        public string PledgingProposal { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PledgingApplicationRemark")]
        public string PledgingApplicationRemark { get; set; }

        public IDictionary<string, string> YesNo { get; set; }

        #endregion PSP A/C Summary

        public PspEventViewModel PspEventViewModel { get; set; }

        public PspAttachmentViewModel PspAttachmentViewModel { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_ProcessingOfficerPost")]
        public string ProcessingOfficerPost { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PermitRevokeIndicator")]
        public bool PermitRevokeIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OverdueIndicator")]
        public bool? OverdueIndicator { get; set; }

        public IDictionary<bool, string> OverdueIndicators { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_LateIndicator")]
        public bool? LateIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

        public Dictionary<int, string> PspEveLst { get; set; }

        public IDictionary<bool, string> LateIndicators { get; set; }

        public IDictionary<bool, string> YesNos { get; set; }

        public byte[] RowVersion { get; set; }

        #region CR-005
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TotalEvent")]
        public int TotalEvent { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_TotalLocations")]
        public int TotalLocation { get; set; }

        #endregion CR-005

        public int? OrgValidTo_Month { get; set; }
        public int? OrgValidTo_Year { get; set; }
        public int? IVP { get; set; }


        #region edit table

        public string LblWithholdingBeginDate { get; set; }

        public string LblWithholdingEndDate { get; set; }

        public string LblPspRef { get; set; }

        public string LblPspContactPersonName { get; set; }

        public string LblPspContactPersonEmailAddress { get; set; }

        public string LblFdRef { get; set; }

        public string LblFdContactPersonName { get; set; }

        public string LblFdContactPersonEmailAddress { get; set; }

        #endregion edit table

        #region Complaint/Enquiry tab

        public IDictionary<string, string> ActivityConcerns { get; set; }

        public IDictionary<string, string> ComplaintSources { get; set; }

        public IDictionary<string, string> ComplaintResults { get; set; }

        public IDictionary<string, string> ProcessStatus { get; set; }

        #endregion Complaint/Enquiry tab
    }
}
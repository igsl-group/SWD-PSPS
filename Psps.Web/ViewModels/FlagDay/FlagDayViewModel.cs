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

namespace Psps.Web.ViewModels.FlagDay
{
    [Validator(typeof(FlagDayViewModelValidator))]
    public partial class FlagDayViewModel : BaseViewModel
    {
        public FlagDayViewModel()
        {
            FlagDayEventViewModel = new FlagDayEventViewModel();
            FlagDayAttachmentViewModel = new FlagDayAttachmentViewModel();
        }

        //public bool IsShowNewApplicant { get; set; }

        public string OrgMasterId { get; set; }

        public bool isFirstSearch { get; set; }

        public string PrePage { get; set; }

        public string CreateMode { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdMasterId")]
        public int FdMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgRef")]
        public string OrgRef { get; set; }

        public string OrgEngName { get; set; }

        public string OrgChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofReceiveingApplication")]
        public DateTime? DateofReceivingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofReceiveingApplication")]
        public DateTime? FromDateofReceiveingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofReceiveingApplication")]
        public DateTime? ToDateofReceiveingApplication { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_LanguageUsed")]
        public string LanguageUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_LanguageUsed")]
        public IDictionary<string, string> LanguageUseds { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Subvented")]
        public bool? Subvented { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Subvented")]
        public Dictionary<bool, string> Subventeds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PrincipalActivities")]
        public string PrincipalActivities { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PermitIssueDate")]
        public DateTime? PermitIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationResult")]
        public string ApplicationResultId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationResult")]
        public IDictionary<string, string> ApplicationResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_VettingPanelCases")]
        public string VettingPanelCases { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_VettingPanelCaseIndicator")]
        public bool VettingPanelCaseIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ReviewCaseIndicator")]
        public bool ReviewCaseIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_YearofFlagDay")]
        public string YearofFlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_YearofFlagDay")]
        public IDictionary<string, string> YearofFlagDays { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_GroupingwithNoofLot")]
        public string GroupingwithNoofLot { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagDay")]
        public string FromFlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagDay")]
        public string ToFlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Category")]
        public string CategoryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagDay")]
        public DateTime? createFrmFlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_TWR")]
        public string createFrmEventTWR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Category")]
        public IDictionary<string, string> Categorys { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_District")]
        public string DistrictId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_District")]
        public IDictionary<string, string> Districts { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TargetIncome")]
        public decimal? TargetIncome { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_NewApplicant")]
        public bool? NewApplicant { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_NewApplicant")]
        public Dictionary<bool, string> NewApplicants { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TerritoryRegion_Application")]
        public string ApplyForTWR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TerritoryRegion")]
        public string TWR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TerritoryRegion_Permit")]
        public IDictionary<string, string> TerritoryRegion { get; set; }

        //[UIHint("YesNo")]
        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TrackRecordofCharitableActivities")]
        //public bool TrackRecordofCharitableActivities { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TrackRecordStartDate")]
        public DateTime? TrackRecordStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TrackRecordEndDate")]
        public DateTime? TrackRecordEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TrackRecordDetails")]
        public string TrackRecordDetails { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsRecordStartDate")]
        public DateTime? AfsRecordStartDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsRecordEndDate")]
        public DateTime? AfsRecordEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsRecordDetails")]
        public string AfsRecordDetails { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Grouping")]
        public string GroupingId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Grouping")]
        public IDictionary<string, string> Groupings { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PercentageforGrouping")]
        public decimal? PercentageForGrouping { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_GroupingResult")]
        public string GroupingResult { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TheCommunityChestofHKMember")]
        public string CommunityChestId { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TheCommunityChestofHKMember")]
        public bool CommunityChest { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_JointApplicationIndicator")]
        public bool JointApplicationIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Section88Indicator")]
        public bool Section88Indicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ConsentLetterfromtheCommunityChestofHK")]
        public string ConsentLetterId { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ConsentLetterfromtheCommunityChestofHK")]
        public bool ConsentLetter { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofOutstandingEmailIssued")]
        //public DateTime? OutstandingEmailIssueDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofOutstandingEmailReply")]
        //public DateTime? OutstandingEmailReplyDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofReminderforOutstandingEmailIssued")]
        //public DateTime? OutstandingEmailReminderIssueDate { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DateofReminderforOutstandingEmailReply")]
        //public DateTime? OutstandingEmailReminderReplyDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DocSubmission")]
        public string DocSubmission { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DocSubmission")]
        public IDictionary<string, string> DocSubmissions { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_SubmissionDueDate")]
        public DateTime? SubmissionDueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FirstReminderIssueDate")]
        public DateTime? FirstReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FirstReminderDeadline")]
        public DateTime? FirstReminderDeadline { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_SecondReminderIssueDate")]
        public DateTime? SecondReminderIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_SecondReminderDeadline")]
        public DateTime? SecondReminderDeadline { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AuditReportReceivedDate")]
        public string AuditReportReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PublicationReceivedDate")]
        public string PublicationReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DocReceiveRemark")]
        public string DocReceiveRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_DocRemark")]
        public string DocRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_StreetCollection")]
        public decimal? StreetCollection { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_GrossProceed")]
        public decimal? GrossProceed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Expenditure")]
        public decimal? Expenditure { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_NetProceed")]
        public decimal? NetProceed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ExpPerGpPercentage")]
        public decimal? ExpPerGpPercentage { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_NewspaperPublishDate")]
        public DateTime? NewspaperPublishDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AcknowledgementEmailIssueDate")]
        public DateTime? AcknowledgementEmailIssueDate { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_WithholdingListIndicator")]
        public bool? WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_WithholdingListIndicator")]
        public string WithholdingListId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_WithholdingListIndicator")]
        public IDictionary<bool, string> WithholdingListIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_WithholdingBeginDate")]
        public DateTime? WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_WithholdingEndDate")]
        public DateTime? WithholdingEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReceiveIndicator")]
        public bool? AfsReceiveIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReceiveIndicator")]
        public Dictionary<bool, string> AfsReceiveIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_RequestPermitteeIndicator")]
        public bool? RequestPermitteeIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_RequestPermitteeIndicator")]
        public Dictionary<bool, string> RequestPermitteeIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReSubmitIndicator")]
        public bool? AfsReSubmitIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReSubmitIndicator")]
        public Dictionary<bool, string> AfsReSubmitIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReminderIssueIndicator")]
        public bool? AfsReminderIssueIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AfsReminderIssueIndicator")]
        public Dictionary<bool, string> AfsReminderIssueIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Remark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdYear")]
        public string[] FdYear { get; set; }

        public IDictionary<string, string> FdYears { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PermitList")]
        public string PermitList { get; set; }

        public IDictionary<string, string> PermitLists { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

        // TIR #: PSUAT00035-1  The availability of FD File Import function will be controlled by FD Approver only.
        public bool IsFdApprover { get; set; }

        public long FlagDayEnableImport { get; set; }

        /// <summary>
        ///
        /// </summary>

        #region FD A/C Summary

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ReferenceNumber")]
        public string CreateModelReferenceNumber { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgRef")]
        public string CreateModelOrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgName")]
        public string OrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_TelNo")]
        public string TelNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FaxNo")]
        public string FaxNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonEmailAddress")]
        public string ContactPersonEmailAddress { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Section")]
        public string SectionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Section")]
        public IDictionary<string, string> Sections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgStatus")]
        public IDictionary<string, string> OrgStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_OrgStatus")]
        public string OrgStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonPost")]
        public string ContactPersonPost { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonName")]
        public string ContactPersonName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonChiName")]
        public string ContactPersonChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonFirstName")]
        public string ContactPersonFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonLastName")]
        public string ContactPersonLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonFirstName")]
        public string ContactPersonChiFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonLastName")]
        public string ContactPersonChiLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ContactPersonLastName")]
        public string ContactPersonChiFirstNameContactPersonLastChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_EngSalutes")]
        public string EngSalute { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_EngSalutes")]
        public IDictionary<string, string> EngSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiSalutes")]
        public string ChiSalute { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiSalutes")]
        public IDictionary<string, string> ChiSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Registration")]
        public string RegistrationId { get; set; }

        public string Registration { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Registration")]
        public IDictionary<string, string> Registrations { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_NewApplicant")]
        public bool CreateModelNewApplicant { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_RefGuide")]
        public IDictionary<string, string> RefGuides { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_RefGuide")]
        public string RefGuideId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FundRaisingPurpose")]
        public string FundRaisingPurpose { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiFundRaisingPurpose")]
        public string ChiFundRaisingPurpose { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ResultApplicationRemark")]
        public string ResultApplicationRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplicationRemark")]
        public string ApplicationRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PledgingApplicationRemark")]
        public string PledgingApplicationRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdLotNum")]
        public string FdLotNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdLotResult")]
        public string FdLotResult { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FdLotResult")]
        public IDictionary<string, string> FdLotResults { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PriorityNum")]
        public string PriorityNum { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ProposalDetail")]
        //public string ProposalDetail { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiProposalDetail")]
        //public string ChiProposalDetail { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagSalePurpose")]
        //public string FlagSalePurpose { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiFlagSalePurpose")]
        //public string ChiFlagSalePurpose { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AcknowledgementReceiveDate")]
        public DateTime? AcknowledgementReceiveDate { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplyPledgingMechanismIndicator")]
        public bool ApplyPledgingMechanismIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ApplyPledgingMechanismIndicator")]
        public bool? SearchApplyPledgingMechanismIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PledgingAmt")]
        public decimal? PledgingAmt { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PledgedAmt")]
        public decimal? PledgedAmt { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PledgingProposal")]
        public string PledgingProposal { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_ChiPledgingProposal")]
        public string ChiPledgingProposal { get; set; }

        #endregion FD A/C Summary

        /// <summary>
        ///
        /// </summary>

        #region FdEvent

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_No")]
        public int? FdEventId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_BatchNum")]
        public int BatchNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FlagDay")]
        public DateTime? FlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_PermitAcknowledgementReceiveDate")]
        public DateTime? PermitAcknowledgementReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_Time")]
        public string Time { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_Time")]
        public string FlagTimeFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_Time")]
        public string FlagTimeTo { get; set; }

        public string FlagDayEventStartTime { get; set; }

        public string FlagDayEventEndTime { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_TWR")]
        public string EventTWR { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_District")]
        public string District { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_MethodOfCollection")]
        public IList<string> MethodOfCollection { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_MethodOfCollection")]
        public IDictionary<string, string> MethodOfCollections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_EventStatus")]
        public string EventStatus { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_FlagColor")]
        public string FlagColour { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_BagColor")]
        public string BagColour { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_PermitRevokeIndicator")]
        public bool PermitRevokeIndicator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_PermitRevokeIndicator")]
        public bool? SearchPermitRevokeIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_PermitRevokeIndicator")]
        public Dictionary<bool, string> PermitRevokeIndicators { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_Remark")]
        public IList<string> EventRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FdRead_Remark")]
        public IDictionary<string, string> EventRemarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase EventImportFile { get; set; }

        public string FrasCharityEventId { get; set; }

        public string FrasStatus { get; set; }

        public byte[] EventRowVersion { get; set; }

        #endregion FdEvent

        public FlagDayEventViewModel FlagDayEventViewModel { get; set; }

        public FlagDayAttachmentViewModel FlagDayAttachmentViewModel { get; set; }

        public byte[] RowVersion { get; set; }

        #region edit table

        public string LblWithholdingBeginDate { get; set; }

        public string LblWithholdingEndDate { get; set; }

        public string LblPspRef { get; set; }

        public string LblPspContactPersonName { get; set; }

        public string LblPspContactPersonEmailAddress { get; set; }

        public string LblFdRef { get; set; }

        public string LblFdContactPersonName { get; set; }

        public string LblFdContactPersonEmailAddress { get; set; }

        public string LblFdBenchmarkStatusPast2nd { get; set; }

        public string LblFdBenchmarkStatusPast1st { get; set; }

        #endregion edit table

        #region Complaint & Enquiry

        public IDictionary<string, string> ActivityConcerns { get; set; }

        public IDictionary<string, string> ComplaintSources { get; set; }

        public IDictionary<string, string> ComplaintResults { get; set; }

        public IDictionary<string, string> ProcessStatus { get; set; }

        #endregion Complaint & Enquiry
    }
}
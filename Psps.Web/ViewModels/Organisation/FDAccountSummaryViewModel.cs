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
    public partial class FDAccountSummaryViewModel : BaseViewModel
    {
        public string Id { get; set; }

        public bool isFirstSearch { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Section88")]
        public bool Section88 { get; set; }

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
        public string AuditReportReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_PublicationReceivedDate")]
        public string PublicationReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OfficialReceiptReceivedDate")]
        public string OfficialReceiptReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_NewspaperCuttingReceivedDate")]
        public string NewspaperCuttingReceivedDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_DocReceivedRemark")]
        public string DocReceiveRemark { get; set; }

        public string DocRemark { get; set; }

        public decimal? StreetCollection { get; set; }

        public decimal? GrossProceed { get; set; }

        public decimal? Expenditure { get; set; }

        public decimal? NetProceed { get; set; }

        public string NewspaperPublishDate { get; set; }

        public decimal? PledgingAmt { get; set; }

        public string AcknowledgementReceiveDate { get; set; }

        public string AcknowledgementEmailIssueDate { get; set; }

        public string AfsReceiveIndicator { get; set; }

        public string RequestPermitteeIndicator { get; set; }

        public string AfsReSubmitIndicator { get; set; }

        public string AfsReminderIssueIndicator { get; set; }

        public string Remark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgRef")]
        public string OrgRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_OrgName")]
        public string OrgName { get; set; }

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

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FileReference")]
        public string FDRef { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public string RegistrationId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingListIndicator")]
        public string WithholdingListIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingBeginDate")]
        public string WithholdingBeginDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_WithholdingEndDate")]
        public string WithholdingEndDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EditEnquiryComplaint_WithholdingRemark")]
        public string WithholdingRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_Overdue")]
        public string Overdue { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_LateIndicator")]
        public string Late { get; set; }

        public IDictionary<string, string> Overdues { get; set; }

        public IDictionary<string, string> TWRs { get; set; }

        public string Registration { get; set; }

        public string RegistrationOtherName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_Registration")]
        public IDictionary<string, string> Registrations { get; set; }

        public IDictionary<string, string> YesNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDACSummary_PermitNo")]
        public string PermitNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDACSummary_FlagDay")]
        public string FlagDay { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FDACSummary_FlagDayYear")]
        public string FlagDayYear { get; set; }

        public IDictionary<string, string> FlagDayYears { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
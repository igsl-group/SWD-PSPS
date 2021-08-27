using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdMaster : BaseAuditEntity<int>
    {
        public FdMaster()
        {
            //FdApprovalHistory = new List<FdApprovalHistory>();
            FdAttachment = new List<FdAttachment>();
            FdEvent = new List<FdEvent>();
        }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual FdMasterComputeView FdMasterComputeView { get; set; }

        public virtual FdExportView FdExportView { get; set; }

        public virtual int FdMasterId { get; set; }

        public virtual string FdYear { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual string FdRef { get; set; }

        public virtual ReferenceGuideSearchView ReferenceGuideSearchView { get; set; }

        public virtual string ApplyForTwr { get; set; }

        public virtual string ContactPersonSalute { get; set; }

        public virtual string ContactPersonFirstName { get; set; }

        public virtual string ContactPersonLastName { get; set; }

        public virtual string ContactPersonChiFirstName { get; set; }

        public virtual string ContactPersonChiLastName { get; set; }

        public virtual string ContactPersonPosition { get; set; }

        public virtual string ContactPersonTelNum { get; set; }

        public virtual string ContactPersonFaxNum { get; set; }

        public virtual string ContactPersonEmailAddress { get; set; }

        public virtual bool? JointApplicationIndicator { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual bool? CommunityChest { get; set; }

        public virtual bool? ConsentLetter { get; set; }

        public virtual bool? NewApplicantIndicator { get; set; }

        public virtual DateTime? TrackRecordStartDate { get; set; }

        public virtual DateTime? TrackRecordEndDate { get; set; }

        public virtual string TrackRecordDetails { get; set; }

        public virtual DateTime? AfsRecordStartDate { get; set; }

        public virtual DateTime? AfsRecordEndDate { get; set; }

        public virtual string AfsRecordDetails { get; set; }

        public virtual decimal? TargetIncome { get; set; }

        public virtual string FundRaisingPurpose { get; set; }

        public virtual string ChiFundRaisingPurpose { get; set; }

        public virtual string UsedLanguage { get; set; }

        public virtual string FdGroup { get; set; }

        public virtual decimal? FdGroupPercentage { get; set; }

        public virtual string GroupingResult { get; set; }

        public virtual bool? VettingPanelCaseIndicator { get; set; }

        public virtual bool? ReviewCaseIndicator { get; set; }

        public virtual string ApplicationResult { get; set; }

        public virtual string ResultApplicationRemark { get; set; }

        public virtual string ApplicationRemark { get; set; }

        public virtual string FdLotNum { get; set; }

        public virtual string LotGroup { get; set; }

        public virtual string FdLotResult { get; set; }

        public virtual string PriorityNum { get; set; }

        public virtual bool? ApplyPledgingMechanismIndicator { get; set; }

        public virtual decimal? PledgedAmt { get; set; }

        //public virtual string FlagSalePurpose { get; set; }

        public virtual string PledgingApplicationRemark { get; set; }

        public virtual string DocSubmission { get; set; }

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

        public virtual decimal? GrossProceed { get; set; }

        public virtual decimal? Expenditure { get; set; }

        public virtual decimal? NetProceed { get; set; }

        public virtual DateTime? NewspaperPublishDate { get; set; }

        public virtual decimal? PledgingAmt { get; set; }

        public virtual DateTime? AcknowledgementEmailIssueDate { get; set; }

        public virtual bool? WithholdingListIndicator { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }

        public virtual string WithholdingRemark { get; set; }

        public virtual bool? AfsReceiveIndicator { get; set; }

        public virtual bool? RequestPermitteeIndicator { get; set; }

        public virtual bool? AfsReSubmitIndicator { get; set; }

        public virtual bool? AfsReminderIssueIndicator { get; set; }

        public virtual string Remark { get; set; }

        public virtual string FdCategory { get; set; }

        public virtual string FdDistrict { get; set; }

        //public virtual DateTime? OutstandingEmailIssueDate { get; set; }

        //public virtual DateTime? OutstandingEmailReplyDate { get; set; }

        //public virtual DateTime? OutstandingEmailReminderIssueDate { get; set; }

        //public virtual DateTime? OutstandingEmailReminderReplyDate { get; set; }

        //public virtual string ProposalDetail { get; set; }

        //public virtual string ChiProposalDetail { get; set; }

        public virtual DateTime? AcknowledgementReceiveDate { get; set; }

        public virtual string PledgingProposal { get; set; }

        public virtual string ChiPledgingProposal { get; set; }

        //public virtual string ChiFlagSalePurpose { get; set; }

        public virtual IList<FdAttachment> FdAttachment { get; set; }

        public virtual IList<FdEvent> FdEvent { get; set; }

        public virtual string ContactPersonName { get; set; } // for mapping using formula  (ContactPersonFirstName + ContactPersonLastName)

        public virtual string ContactPersonChiName { get; set; } // for mapping using formula

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
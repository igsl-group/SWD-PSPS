using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspMasterMap : BaseAuditEntityMap<PspMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspMasterId).GeneratedBy.Identity().Column("PspMasterId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            References(x => x.DisasterMaster).Column("DisasterMasterId");
            References(x => x.ReferenceGuideSearchView).Column("OrgId").ReadOnly();
            Map(x => x.PreviousPspMasterId).Column("PreviousPspMasterId");
            Map(x => x.PspRef).Column("PspRef").Length(10);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.ContactPersonSalute).Column("ContactPersonSalute").Length(20);
            Map(x => x.ContactPersonFirstName).Column("ContactPersonFirstName").Length(100);
            Map(x => x.ContactPersonLastName).Column("ContactPersonLastName").Length(50);
            Map(x => x.ContactPersonChiFirstName).Column("ContactPersonChiFirstName").Length(5);
            Map(x => x.ContactPersonChiLastName).Column("ContactPersonChiLastName").Length(10);
            Map(x => x.ContactPersonPosition).Column("ContactPersonPosition").Length(100);
            Map(x => x.ContactPersonTelNum).Column("ContactPersonTelNum").Length(50);
            Map(x => x.ContactPersonFaxNum).Column("ContactPersonFaxNum").Length(50);
            Map(x => x.ContactPersonEmailAddress).Column("ContactPersonEmailAddress").Length(100);
            Map(x => x.NewApplicantIndicator).Column("NewApplicantIndicator");
            Map(x => x.ProcessingOfficerPost).Column("ProcessingOfficerPost").Length(20);
            Map(x => x.PspYear).Column("PspYear").Length(5);
            Map(x => x.EventPeriodFrom).Column("EventPeriodFrom");
            Map(x => x.EventPeriodTo).Column("EventPeriodTo");
            Map(x => x.BypassValidationIndicator).Column("BypassValidationIndicator");
            Map(x => x.ApplicationDate).Column("ApplicationDate");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.ApplicationCompletionDate).Column("ApplicationCompletionDate");
            Map(x => x.ActionBuDate).Column("ActionBuDate");
            Map(x => x.BeneficiaryOrg).Column("BeneficiaryOrg").Length(500);
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.EngFundRaisingPurpose).Column("EngFundRaisingPurpose").Length(4000);
            Map(x => x.ChiFundRaisingPurpose).Column("ChiFundRaisingPurpose").Length(4000);
            Map(x => x.EventTitle).Column("EventTitle").Length(2000);
            Map(x => x.EngCharitySalesItem).Column("EngCharitySalesItem").Length(2000);
            Map(x => x.ChiCharitySalesItem).Column("ChiCharitySalesItem").Length(2000);
            Map(x => x.UsedLanguage).Column("UsedLanguage").Length(20);
            Map(x => x.ApplicationDisposalDate).Column("ApplicationDisposalDate");
            Map(x => x.RejectReason).Column("RejectReason").Length(20);
            Map(x => x.RejectRemark).Column("RejectRemark").Length(4000);
            Map(x => x.OtherRejectReason).Column("OtherRejectReason").Length(256);
            Map(x => x.PspNotRequireReason).Column("PspNotRequireReason").Length(20);
            Map(x => x.OtherPspNotRequireReason).Column("OtherPspNotRequireReason").Length(256);
            Map(x => x.CaseCloseReason).Column("CaseCloseReason").Length(20);
            Map(x => x.OtherCaseCloseReason).Column("OtherCaseCloseReason").Length(256);
            Map(x => x.SpecialRemark).Column("SpecialRemark").Length(60);
            Map(x => x.OtherSpecialRemark).Column("OtherSpecialRemark").Length(4000);
            Map(x => x.RejectionLetterDate).Column("RejectionLetterDate");
            Map(x => x.RepresentationReceiveDate).Column("RepresentationReceiveDate");
            Map(x => x.RepresentationReplyDate).Column("RepresentationReplyDate");
            Map(x => x.TrackRecordStartDate).Column("TrackRecordStartDate");
            Map(x => x.TrackRecordEndDate).Column("TrackRecordEndDate");
            Map(x => x.TrackRecordDetails).Column("TrackRecordDetails").Length(4000);
            Map(x => x.AfsRecordStartDate).Column("AfsRecordStartDate");
            Map(x => x.AfsRecordEndDate).Column("AfsRecordEndDate");
            Map(x => x.AfsRecordDetails).Column("AfsRecordDetails").Length(4000);
            Map(x => x.FundUsed).Column("FundUsed").Length(20);
            Map(x => x.DocSubmission).Column("DocSubmission").Length(20);
            Map(x => x.SubmissionDueDate).Column("SubmissionDueDate");
            Map(x => x.FirstReminderIssueDate).Column("FirstReminderIssueDate");
            Map(x => x.FirstReminderDeadline).Column("FirstReminderDeadline");
            Map(x => x.SecondReminderIssueDate).Column("SecondReminderIssueDate");
            Map(x => x.SecondReminderDeadline).Column("SecondReminderDeadline");
            Map(x => x.AuditedReportReceivedDate).Column("AuditedReportReceivedDate").Length(100);
            Map(x => x.PublicationReceivedDate).Column("PublicationReceivedDate").Length(100);
            Map(x => x.OfficialReceiptReceivedDate).Column("OfficialReceiptReceivedDate").Length(100);
            Map(x => x.NewspaperCuttingReceivedDate).Column("NewspaperCuttingReceivedDate").Length(100);
            Map(x => x.DocReceivedRemark).Column("DocReceivedRemark").Length(4000);
            Map(x => x.GrossProceed).Column("GrossProceed").Precision(12).Scale(2);
            Map(x => x.Expenditure).Column("Expenditure").Precision(12).Scale(2);
            Map(x => x.NetProceed).Column("NetProceed").Precision(12).Scale(2);
            Map(x => x.OrgAnnualIncome).Column("OrgAnnualIncome").Precision(12).Scale(2);
            Map(x => x.PermitRevokeIndicator).Column("PermitRevokeIndicator");
            Map(x => x.SanctionListIndicator).Column("SanctionListIndicator");
            Map(x => x.QualifyOpinionIndicator).Column("QualifyOpinionIndicator");
            Map(x => x.QualityOpinionDetail).Column("QualityOpinionDetail").Length(4000);
            Map(x => x.ArCheckIndicator).Column("ArCheckIndicator").Length(20);
            Map(x => x.PublicationCheckIndicator).Column("PublicationCheckIndicator").Length(20);
            Map(x => x.OfficialReceiptCheckIndicator).Column("OfficialReceiptCheckIndicator").Length(20);
            Map(x => x.NewspaperCheckIndicator).Column("NewspaperCheckIndicator").Length(20);
            Map(x => x.DocRemark).Column("DocRemark").Length(4000);
            Map(x => x.ApplicationResult).Column("ApplicationResult").Length(20);
            Map(x => x.ContactPersonName).Formula("isnull(ContactPersonFirstName,'') + ' ' + isnull(ContactPersonLastName,'') ");
            Map(x => x.ContactPersonChiName).Formula("isnull(ContactPersonChiFirstName,'') + isnull(ContactPersonChiLastName,'') ");
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark").Length(4000);
            Map(x => x.IsSsaf).Column("IsSsaf").Default("false");

            HasMany(x => x.PspApprovalHistory).KeyColumn("PspMasterId").Inverse();
            HasMany(x => x.PspAttachment).KeyColumn("PspMasterId").Inverse();
            HasMany(x => x.PspEvent).KeyColumn("PspMasterId").Inverse();
        }
    }
}
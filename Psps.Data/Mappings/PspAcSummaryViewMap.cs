using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspAcSummaryViewMap : BaseEntityMap<PspAcSummaryView, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspMasterId).GeneratedBy.Identity().Column("PspMasterId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.PreviousPspMasterId).Column("PreviousPspMasterId");
            Map(x => x.PspRef).Column("PspRef").Length(8);
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
            Map(x => x.PspYear).Column("PspYear");
            Map(x => x.EventPeriodFrom).Column("EventPeriodFrom");
            Map(x => x.EventPeriodTo).Column("EventPeriodTo");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.ApplicationCompletionDate).Column("ApplicationCompletionDate");
            Map(x => x.ActionBuDate).Column("ActionBuDate");
            Map(x => x.BeneficiaryOrg).Column("BeneficiaryOrg");
            Map(x => x.EngFundRaisingPurpose).Column("EngFundRaisingPurpose").Length(4000);
            Map(x => x.ChiFundRaisingPurpose).Column("ChiFundRaisingPurpose").Length(4000);
            Map(x => x.EventTitle).Column("EventTitle").Length(1024);
            Map(x => x.EngCharitySalesItem).Column("EngCharitySalesItem").Length(1024);
            Map(x => x.ChiCharitySalesItem).Column("ChiCharitySalesItem").Length(1024);
            Map(x => x.UsedLanguage).Column("UsedLanguage").Length(20);
            Map(x => x.ApplicationDisposalDate).Column("ApplicationDisposalDate");
            Map(x => x.RejectReason).Column("RejectReason").Length(20);
            Map(x => x.OtherRejectReason).Column("OtherRejectReason").Length(256);
            Map(x => x.PspNotRequireReason).Column("PspNotRequireReason").Length(20);
            Map(x => x.OtherPspNotRequireReason).Column("OtherPspNotRequireReason").Length(256);
            Map(x => x.CaseCloseReason).Column("CaseCloseReason").Length(20);
            Map(x => x.OtherCaseCloseReason).Column("OtherCaseCloseReason").Length(256);
            Map(x => x.SpecialRemark).Column("SpecialRemark").Length(1000);
            Map(x => x.OtherSpecialRemark).Column("OtherSpecialRemark").Length(4000);
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
            Map(x => x.AuditedReportReceivedDate).Column("AuditedReportReceivedDate");
            Map(x => x.PublicationReceivedDate).Column("PublicationReceivedDate");
            Map(x => x.OfficialReceiptReceivedDate).Column("OfficialReceiptReceivedDate");
            Map(x => x.NewspaperCuttingReceivedDate).Column("NewspaperCuttingReceivedDate");
            Map(x => x.DocReceivedRemark).Column("DocReceivedRemark").Length(4000);
            Map(x => x.GrossProceed).Column("GrossProceed").Precision(12).Scale(2);
            Map(x => x.Expenditure).Column("Expenditure").Precision(12).Scale(2);
            Map(x => x.NetProceed).Column("NetProceed").Precision(12).Scale(2);
            Map(x => x.PspPercent).Column("PspPercent").Precision(12).Scale(2);
            Map(x => x.OrgAnnualIncome).Column("OrgAnnualIncome").Precision(12).Scale(2);
            Map(x => x.SanctionListIndicator).Column("SanctionListIndicator");
            Map(x => x.QualifyOpinionIndicator).Column("QualifyOpinionIndicator");
            Map(x => x.QualityOpinionDetail).Column("QualityOpinionDetail").Length(2000);
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.ArCheckIndicator).Column("ArCheckIndicator");
            Map(x => x.PublicationCheckIndicator).Column("PublicationCheckIndicator");
            Map(x => x.OfficialReceiptCheckIndicator).Column("OfficialReceiptCheckIndicator");
            Map(x => x.NewspaperCheckIndicator).Column("NewspaperCheckIndicator");
            Map(x => x.DocRemark).Column("DocRemark").Length(2000);
            Map(x => x.ApplicationResult).Column("ApplicationResult").Length(20);
            Map(x => x.ContactPersonName).Formula("isnull(ContactPersonFirstName,'') + ' ' + isnull(ContactPersonLastName,'') ");
            Map(x => x.ContactPersonChiName).Formula("isnull(ContactPersonChiFirstName,'') + isnull(ContactPersonChiLastName,'') ");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark");
            Map(x => x.RelatedPermitNo).Column("RelatedPermitNo");
            Map(x => x.EventStartDate).Column("EventStartDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.PermitNo).Column("PermitNo");
            Map(x => x.Overdue).Column("Overdue");
            Map(x => x.Late).Column("Late");
            Map(x => x.DisasterMasterId).Column("DisasterMasterId");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.ChildIndicator).Column("ChildIndicator");
            Map(x => x.CancelIndicator).Column("CancelIndicator");
            Map(x => x.IsSsaf).Column("IsSsaf");
            HasMany(x => x.PspApprovalHistory).KeyColumn("PspMasterId").Inverse();
            HasMany(x => x.PspEvent).KeyColumn("PspMasterId").Inverse();
        }
    }
}
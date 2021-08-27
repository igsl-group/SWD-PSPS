using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdAcSummaryViewMap : BaseEntityMap<FdAcSummaryView, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdMasterId).GeneratedBy.Identity().Column("FdMasterId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.flagday).Column("flagday");
            Map(x => x.TWR).Column("TWR").Length(20);
            Map(x => x.FdYear).Column("FdYear").Length(5);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.FdRef).Column("FdRef").Not.Nullable().Length(12);
            Map(x => x.ApplyForTwr).Column("ApplyForTwr").Length(20);
            Map(x => x.ContactPersonSalute).Column("ContactPersonSalute").Length(20);
            Map(x => x.ContactPersonFirstName).Column("ContactPersonFirstName").Length(100);
            Map(x => x.ContactPersonLastName).Column("ContactPersonLastName").Length(50);
            Map(x => x.ContactPersonChiFirstName).Column("ContactPersonChiFirstName").Length(5);
            Map(x => x.ContactPersonChiLastName).Column("ContactPersonChiLastName").Length(10);
            Map(x => x.ContactPersonPosition).Column("ContactPersonPosition").Length(100);
            Map(x => x.ContactPersonTelNum).Column("ContactPersonTelNum").Length(50);
            Map(x => x.ContactPersonFaxNum).Column("ContactPersonFaxNum").Length(50);
            Map(x => x.ContactPersonEmailAddress).Column("ContactPersonEmailAddress").Length(100);
            Map(x => x.JointApplicationIndicator).Column("JointApplicationIndicator");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.CommunityChest).Column("CommunityChest");
            Map(x => x.ConsentLetter).Column("ConsentLetter");
            Map(x => x.NewApplicantIndicator).Column("NewApplicantIndicator");
            Map(x => x.TrackRecordStartDate).Column("TrackRecordStartDate");
            Map(x => x.TrackRecordEndDate).Column("TrackRecordEndDate");
            Map(x => x.TrackRecordDetails).Column("TrackRecordDetails").Length(4000);
            Map(x => x.AfsRecordStartDate).Column("AfsRecordStartDate");
            Map(x => x.AfsRecordEndDate).Column("AfsRecordEndDate");
            Map(x => x.AfsRecordDetails).Column("AfsRecordDetails").Length(4000);
            Map(x => x.TargetIncome).Column("TargetIncome").Precision(10).Scale(2);
            Map(x => x.FundRaisingPurpose).Column("FundRaisingPurpose").Length(4000);
            Map(x => x.ChiFundRaisingPurpose).Column("ChiFundRaisingPurpose").Length(4000);
            Map(x => x.UsedLanguage).Column("UsedLanguage").Length(20);
            Map(x => x.FdGroup).Column("FdGroup").Length(20);
            Map(x => x.FdGroupPercentage).Column("FdGroupPercentage").Precision(5).Scale(2);
            Map(x => x.GroupingResult).Column("GroupingResult").Length(2000);
            Map(x => x.VettingPanelCaseIndicator).Column("VettingPanelCaseIndicator");
            Map(x => x.ReviewCaseIndicator).Column("ReviewCaseIndicator");
            Map(x => x.ApplicationResult).Column("ApplicationResult").Length(20);
            Map(x => x.ApplicationRemark).Column("ApplicationRemark").Length(4000);
            Map(x => x.FdLotNum).Column("FdLotNum");
            Map(x => x.LotGroup).Column("LotGroup").Length(5);
            Map(x => x.FdLotResult).Column("FdLotResult").Length(20);
            Map(x => x.PriorityNum).Column("PriorityNum").Length(100);
            Map(x => x.ApplyPledgingMechanismIndicator).Column("ApplyPledgingMechanismIndicator");
            Map(x => x.PledgedAmt).Column("PledgedAmt").Precision(10).Scale(2);
            Map(x => x.PledgingApplicationRemark).Column("PledgingApplicationRemark").Length(4000);
            Map(x => x.DocSubmission).Column("DocSubmission").Length(60);
            Map(x => x.SubmissionDueDate).Column("SubmissionDueDate");
            Map(x => x.FirstReminderIssueDate).Column("FirstReminderIssueDate");
            Map(x => x.FirstReminderDeadline).Column("FirstReminderDeadline");
            Map(x => x.SecondReminderIssueDate).Column("SecondReminderIssueDate");
            Map(x => x.SecondReminderDeadline).Column("SecondReminderDeadline");
            Map(x => x.AuditReportReceivedDate).Column("AuditReportReceivedDate").Length(100);
            Map(x => x.PublicationReceivedDate).Column("PublicationReceivedDate").Length(100);
            Map(x => x.DocReceiveRemark).Column("DocReceiveRemark").Length(4000);
            Map(x => x.DocRemark).Column("DocRemark").Length(4000);
            Map(x => x.StreetCollection).Column("StreetCollection").Precision(12).Scale(2);
            Map(x => x.GrossProceed).Column("GrossProceed").Precision(12).Scale(2);
            Map(x => x.Expenditure).Column("Expenditure").Precision(12).Scale(2);
            Map(x => x.NetProceed).Column("NetProceed").Precision(12).Scale(2);
            Map(x => x.NewspaperPublishDate).Column("NewspaperPublishDate");
            Map(x => x.PledgingAmt).Column("PledgingAmt").Precision(12).Scale(2);
            Map(x => x.AcknowledgementEmailIssueDate).Column("AcknowledgementEmailIssueDate");
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark");
            Map(x => x.AfsReceiveIndicator).Column("AfsReceiveIndicator");
            Map(x => x.RequestPermitteeIndicator).Column("RequestPermitteeIndicator");
            Map(x => x.AfsReSubmitIndicator).Column("AfsReSubmitIndicator");
            Map(x => x.AfsReminderIssueIndicator).Column("AfsReminderIssueIndicator");
            Map(x => x.Remark).Column("Remark").Length(4000);
            Map(x => x.FdCategory).Column("FdCategory").Length(20);
            Map(x => x.FdDistrict).Column("FdDistrict").Length(20);
            Map(x => x.AcknowledgementReceiveDate).Column("AcknowledgementReceiveDate");
            Map(x => x.PledgingProposal).Column("PledgingProposal").Length(4000);
            Map(x => x.ChiPledgingProposal).Column("ChiPledgingProposal").Length(4000);
            Map(x => x.Overdue).Column("Overdue").Precision(10);
            Map(x => x.Late).Column("Late").Precision(10);
            Map(x => x.FdPercent).Column("FdPercent").Precision(18).Scale(2);
            Map(x => x.ContactPersonName).Formula("isnull(ContactPersonFirstName,'') + ' ' + isnull(ContactPersonLastName,'') ");
            Map(x => x.ContactPersonChiName).Formula("isnull(ContactPersonChiFirstName,'') + isnull(ContactPersonChiLastName,'') ");
            HasMany(x => x.FdAttachment).KeyColumn("FdMasterId").Inverse();
            HasMany(x => x.FdEvent).KeyColumn("FdMasterId").Inverse();
        }
    }
}
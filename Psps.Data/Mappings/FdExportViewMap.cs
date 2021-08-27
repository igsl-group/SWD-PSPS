using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdExportViewMap : BaseEntityMap<FdExportView, int>
    {
        public FdExportViewMap()
            : base()
        {
            ReadOnly();
        }

        protected override void MapId()
        {
            Id(x => x.FdMasterId).GeneratedBy.Identity().Column("FdMasterId");
        }

        protected override void MapEntity()
        {
            // Columns of FdMaster
            Map(x => x.FdMasterId).Column("FdMasterId");
            References(x => x.OrgMaster).Column("OrgId");
            References(x => x.ReferenceGuideSearchView).Column("OrgId").ReadOnly();
            Map(x => x.FdRef).Column("FdRef").Length(12).Not.Nullable();
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.UsedLanguage).Column("UsedLanguage").Length(20);
            Map(x => x.ContactPersonSalute).Column("ContactPersonSalute").Length(20);
            Map(x => x.ContactPersonFirstName).Column("ContactPersonFirstName").Length(100);
            Map(x => x.ContactPersonLastName).Column("ContactPersonLastName").Length(50);
            Map(x => x.ContactPersonChiFirstName).Column("ContactPersonChiFirstName").Length(5);
            Map(x => x.ContactPersonChiLastName).Column("ContactPersonChiLastName").Length(10);
            Map(x => x.ContactPersonPosition).Column("ContactPersonPosition").Length(100);
            Map(x => x.ContactPersonTelNum).Column("ContactPersonTelNum").Length(50);
            Map(x => x.ContactPersonFaxNum).Column("ContactPersonFaxNum").Length(50);
            Map(x => x.ContactPersonEmailAddress).Column("ContactPersonEmailAddress").Length(100);
            Map(x => x.FundRaisingPurpose).Column("FundRaisingPurpose").Length(4000);
            Map(x => x.ChiFundRaisingPurpose).Column("ChiFundRaisingPurpose").Length(4000);
            Map(x => x.ApplyForTwr).Column("ApplyForTwr").Length(20).ReadOnly();
            Map(x => x.ApplicationResult).Column("ApplicationResult").Length(20);
            Map(x => x.VettingPanelCaseIndicator).Column("VettingPanelCaseIndicator");
            Map(x => x.ReviewCaseIndicator).Column("ReviewCaseIndicator");
            Map(x => x.JointApplicationIndicator).Column("JointApplicationIndicator");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.FdYear).Column("FdYear").Length(5);
            Map(x => x.LotGroup).Column("LotGroup").Length(5);
            Map(x => x.FdCategory).Column("FdCategory").Length(20);
            Map(x => x.FdDistrict).Column("FdDistrict").Length(20);
            Map(x => x.TargetIncome).Column("TargetIncome");
            Map(x => x.NewApplicantIndicator).Column("NewApplicantIndicator");
            Map(x => x.TrackRecordStartDate).Column("TrackRecordStartDate");
            Map(x => x.TrackRecordEndDate).Column("TrackRecordEndDate");
            Map(x => x.TrackRecordDetails).Column("TrackRecordDetails").Length(4000);
            Map(x => x.AfsRecordStartDate).Column("AfsRecordStartDate");
            Map(x => x.AfsRecordEndDate).Column("AfsRecordEndDate");
            Map(x => x.AfsRecordDetails).Column("AfsRecordDetails").Length(4000);
            Map(x => x.FdGroup).Column("FdGroup").Length(20);
            Map(x => x.FdGroupPercentage).Column("FdGroupPercentage");
            Map(x => x.GroupingResult).Column("GroupingResult").Length(2000);
            Map(x => x.CommunityChest).Column("CommunityChest");
            Map(x => x.ConsentLetter).Column("ConsentLetter");
            Map(x => x.ApplicationRemark).Column("ApplicationRemark").Length(4000);
            Map(x => x.FdLotNum).Column("FdLotNum").Length(20);
            Map(x => x.FdLotResult).Column("FdLotResult").Length(20);
            Map(x => x.PriorityNum).Column("PriorityNum").Length(100);
            Map(x => x.ApplyPledgingMechanismIndicator).Column("ApplyPledgingMechanismIndicator");
            Map(x => x.PledgedAmt).Column("PledgedAmt");
            Map(x => x.PledgingProposal).Column("PledgingProposal").Length(4000);
            Map(x => x.ChiPledgingProposal).Column("ChiPledgingProposal").Length(4000);
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
            Map(x => x.StreetCollection).Column("StreetCollection");
            Map(x => x.GrossProceed).Column("GrossProceed");
            Map(x => x.Expenditure).Column("Expenditure");
            Map(x => x.NetProceed).Column("NetProceed");
            Map(x => x.NewspaperPublishDate).Column("NewspaperPublishDate");
            Map(x => x.PledgingAmt).Column("PledgingAmt");
            Map(x => x.AcknowledgementEmailIssueDate).Column("AcknowledgementEmailIssueDate");
            Map(x => x.AfsReceiveIndicator).Column("AfsReceiveIndicator");
            Map(x => x.RequestPermitteeIndicator).Column("RequestPermitteeIndicator");
            Map(x => x.AfsReSubmitIndicator).Column("AfsReSubmitIndicator");
            Map(x => x.AfsReminderIssueIndicator).Column("AfsReminderIssueIndicator");
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark").Length(4000);
            Map(x => x.Remark).Column("Remark").Length(4000);

            // Reference Lot Group No.
            Map(x => x.RefLotGroup).Column("RefLotGroup").Length(14);

            // Columns of FdEvent
            Map(x => x.FdEventId).Column("FdEventId").Nullable();
            Map(x => x.FlagDay).Column("FlagDay");
            Map(x => x.FlagTimeFrom).Column("FlagTimeFrom");
            Map(x => x.FlagTimeTo).Column("FlagTimeTo");
            Map(x => x.TWR).Column("TWR").Length(20);
            Map(x => x.TwrDistrict).Column("TwrDistrict").Length(20);
            Map(x => x.CollectionMethod).Column("CollectionMethod").Length(60);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.PermitIssueDate).Column("PermitIssueDate");
            Map(x => x.PermitAcknowledgementReceiveDate).Column("PermitAcknowledgementReceiveDate");
            Map(x => x.BagColour).Column("BagColour").Length(500);
            Map(x => x.FlagColour).Column("FlagColour").Length(500);
            Map(x => x.PermitRevokeIndicator).Column("PermitRevokeIndicator");
            Map(x => x.Remarks).Column("Remarks").Length(60);
            Map(x => x.FrasCharityEventId).Column("FrasCharityEventId").Length(255);
            Map(x => x.FrasResponse).Column("FrasResponse").Length(4000);
            Map(x => x.FrasStatus).Column("FrasStatus").Length(2);

            // Variable for Search
            Map(x => x.OrgRef).Column("OrgRef").Length(8);
            Map(x => x.OrgName).Formula("isnull(EngOrgName,'') + ' ' + isnull(ChiOrgName,'') ");
            Map(x => x.EngOrgName).Column("EngOrgName").Length(255);
            Map(x => x.ChiOrgName).Column("ChiOrgName").Length(255);
            Map(x => x.SubventedIndicator).Column("SubventedIndicator");
            Map(x => x.DisableIndicator).Column("DisableIndicator");
            Map(x => x.ContactPersonName).Formula("isnull(ContactPersonFirstName,'') + ' ' + isnull(ContactPersonLastName,'') ");
            Map(x => x.ContactPersonChiName).Formula("isnull(ContactPersonChiFirstName,'') + isnull(ContactPersonChiLastName,'') ");

            Map(x => x.WithPermit).Column("WithPermit");
        }
    }
}
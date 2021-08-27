using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ComplaintFollowUpActionMap : BaseAuditEntityMap<ComplaintFollowUpAction, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintFollowUpActionId).GeneratedBy.Identity().Column("ComplaintFollowUpActionId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            //References(x => x.DisasterMaster).Column("DisasterMasterId");
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(20);
            Map(x => x.ReportPoliceIndicator).Column("ReportPoliceIndicator");
            Map(x => x.VerbalReportDate).Column("VerbalReportDate");
            Map(x => x.PoliceStation).Column("PoliceStation").Length(100);
            Map(x => x.PoliceOfficerName).Column("PoliceOfficerName").Length(100);
            Map(x => x.RnNum).Column("RnNum").Length(50);
            Map(x => x.RnRemark).Column("RnRemark").Length(4000);
            Map(x => x.RnRemarkHtml).Column("RnRemarkHtml").CustomType("StringClob");
            Map(x => x.WrittenReferralDate).Column("WrittenReferralDate");
            Map(x => x.ReferralPoliceStation).Column("ReferralPoliceStation").Length(100);
            Map(x => x.ActionFileRefEnclosureNum).Column("ActionFileRefEnclosureNum").Length(50);
            Map(x => x.ActionFileRefPartNum).Column("ActionFileRefPartNum").Length(20);
            Map(x => x.PoliceInvestigation).Column("PoliceInvestigation").Length(20);
            Map(x => x.PoliceInvestigationResult).Column("PoliceInvestigationResult").Length(4000);
            Map(x => x.PoliceInvestigationResultHtml).Column("PoliceInvestigationResultHtml").CustomType("StringClob");
            Map(x => x.PoliceReplyDate).Column("PoliceReplyDate");
            Map(x => x.ConvictedPersonName).Column("ConvictedPersonName").Length(100);
            Map(x => x.ConvictedPersonPosition).Column("ConvictedPersonPosition").Length(100);
            Map(x => x.OffenceDetail).Column("OffenceDetail").Length(4000);
            Map(x => x.OffenceDetailHtml).Column("OffenceDetailHtml").CustomType("StringClob");
            Map(x => x.SentenceDetail).Column("SentenceDetail").Length(4000);
            Map(x => x.SentenceDetailHtml).Column("SentenceDetailHtml").CustomType("StringClob");
            Map(x => x.CourtRefNum).Column("CourtRefNum").Length(100);
            Map(x => x.CourtHearingDate).Column("CourtHearingDate").Nullable();
            Map(x => x.PoliceRemark).Column("PoliceRemark").Length(4000);
            Map(x => x.PoliceRemarkHtml).Column("PoliceRemarkHtml").CustomType("StringClob");
            Map(x => x.ReferralPoliceOfficerName).Column("ReferralPoliceOfficerName").Length(100);
            Map(x => x.ReferralPoliceOfficerPosition).Column("ReferralPoliceOfficerPosition").Length(100);
            Map(x => x.FollowUpIndicator).Column("FollowUpIndicator");
            Map(x => x.ContactOrgName).Column("ContactOrgName").Length(300);
            Map(x => x.ContactPersonName).Column("ContactPersonName").Length(100);
            Map(x => x.ContactDate).Column("ContactDate");
            Map(x => x.OrgRemark).Column("OrgRemark").Length(4000);
            Map(x => x.OrgRemarkHtml).Column("OrgRemarkHtml").CustomType("StringClob");
            Map(x => x.FollowUpLetterType).Column("FollowUpLetterType").Length(20);
            Map(x => x.FollowUpLetterReceiver).Column("FollowUpLetterReceiver").Length(100);
            Map(x => x.FollowUpLetterIssueDate).Column("FollowUpLetterIssueDate");
            Map(x => x.FollowUpLetterRemark).Column("FollowUpLetterRemark").Length(4000);
            Map(x => x.FollowUpLetterRemarkHtml).Column("FollowUpLetterRemarkHtml").CustomType("StringClob");
            Map(x => x.FollowUpLetterActionFileRefEnclosureNum).Column("FollowUpLetterActionFileRefEnclosureNum").Length(50);
            Map(x => x.FollowUpLetterActionFileRefPartNum).Column("FollowUpLetterActionFileRefPartNum").Length(20);
            Map(x => x.FollowUpLetterActionFileRefRemark).Column("FollowUpLetterActionFileRefRemark").Length(4000);
            Map(x => x.FollowUpLetterActionFileRefRemarkHtml).Column("FollowUpLetterActionFileRefRemarkHtml").CustomType("StringClob");
            Map(x => x.FollowUpOrgReply).Column("FollowUpOrgReply").Length(4000);
            Map(x => x.FollowUpOrgReplyHtml).Column("FollowUpOrgReplyHtml").CustomType("StringClob");
            Map(x => x.FollowUpOrgReplyDate).Column("FollowUpOrgReplyDate");
            Map(x => x.FollowUpOfficerName).Column("FollowUpOfficerName").Length(100);
            Map(x => x.FollowUpOfficerPosition).Column("FollowUpOfficerPosition").Length(100);
            Map(x => x.OtherFollowUpIndicator).Column("OtherFollowUpIndicator");
            Map(x => x.OtherFollowUpPartyName).Column("OtherFollowUpPartyName").Length(500);
            Map(x => x.OtherFollowUpContactDate).Column("OtherFollowUpContactDate");
            Map(x => x.OtherFollowUpRemark).Column("OtherFollowUpRemark").Length(4000);
            Map(x => x.OtherFollowUpRemarkHtml).Column("OtherFollowUpRemarkHtml").CustomType("StringClob");
            Map(x => x.OtherFollowUpFileRefEnclosureNum).Column("OtherFollowUpFileRefEnclosureNum").Length(50);
            Map(x => x.OtherFollowUpFileRefPartNum).Column("OtherFollowUpFileRefPartNum").Length(20);
            Map(x => x.OtherFollowUpOfficerName).Column("OtherFollowUpOfficerName").Length(100);
            Map(x => x.OtherFollowUpOfficerPosition).Column("OtherFollowUpOfficerPosition").Length(100);
        }
    }
}
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ComplaintPoliceCaseMap : BaseAuditEntityMap<ComplaintPoliceCase, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintPoliceCaseId).GeneratedBy.Identity().Column("ComplaintPoliceCaseId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            Map(x => x.CaseInvestigateRefNum).Column("CaseInvestigateRefNum").Length(50);
            Map(x => x.ReferralDate).Column("ReferralDate");
            Map(x => x.MemoDate).Column("MemoDate");
            Map(x => x.OrgId).Column("OrgId");
            References(x => x.OrgMaster).Column("OrgId").ReadOnly();
            Map(x => x.CorrespondenceEnclosureNum).Column("CorrespondenceEnclosureNum").Length(50);
            Map(x => x.CorrespondencePartNum).Column("CorrespondencePartNum").Length(20);
            Map(x => x.InvestigationResult).Column("InvestigationResult").Length(20);
            Map(x => x.PoliceRefNum).Column("PoliceRefNum").Length(100);
            Map(x => x.CaseNature).Column("CaseNature").Length(4000);
            Map(x => x.CaseNatureHtml).Column("CaseNatureHtml").CustomType("StringClob");
            Map(x => x.ResultDetail).Column("ResultDetail").Length(4000);
            Map(x => x.ResultDetailHtml).Column("ResultDetailHtml").CustomType("StringClob");
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(50);
            Map(x => x.PartNum).Column("PartNum").Length(20);
            Map(x => x.FundRaisingDate).Column("FundRaisingDate");
            Map(x => x.FundRaisingTime).Column("FundRaisingTime").Length(100);
            Map(x => x.FundRaisingLocation).Column("FundRaisingLocation").Length(256);
            Map(x => x.ConvictedPersonName).Column("ConvictedPersonName").Length(100);
            Map(x => x.ConvictedPersonPosition).Column("ConvictedPersonPosition").Length(100);
            Map(x => x.OffenceDetail).Column("OffenceDetail").Length(4000);
            Map(x => x.OffenceDetailHtml).Column("OffenceDetailHtml").CustomType("StringClob");
            Map(x => x.SentenceDetail).Column("SentenceDetail").Length(4000);
            Map(x => x.SentenceDetailHtml).Column("SentenceDetailHtml").CustomType("StringClob");
            Map(x => x.CourtRefNum).Column("CourtRefNum").Length(100);
            Map(x => x.CourtHearingDate).Column("CourtHearingDate");
            Map(x => x.Remark).Column("Remark").Length(4000);
            Map(x => x.RemarkHtml).Column("RemarkHtml").CustomType("StringClob");
        }
    }
}
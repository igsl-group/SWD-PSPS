using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ComplaintTelRecordMap : BaseAuditEntityMap<ComplaintTelRecord, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintTelRecordId).GeneratedBy.Identity().Column("ComplaintTelRecordId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            References(x => x.PspApprovalHistory).Column("PspApprovalHistoryId");
            References(x => x.FdEvent).Column("FdEventId");
            Map(x => x.TelComplaintRef).Column("TelComplaintRef").Not.Nullable().Length(15);
            Map(x => x.ComplaintDate).Column("ComplaintDate");
            Map(x => x.ComplaintTime).Column("ComplaintTime").Length(100);
            Map(x => x.ComplainantName).Column("ComplainantName").Length(100);
            Map(x => x.ComplainantTelNum).Column("ComplainantTelNum").Length(50);
            Map(x => x.OrgName).Column("OrgName").Length(1000);
            Map(x => x.ComplaintContentRemark).Column("ComplaintContentRemark").Length(4000);
            Map(x => x.ComplaintContentRemarkHtml).Column("ComplaintContentRemarkHtml").CustomType("StringClob");
            Map(x => x.OfficerName).Column("OfficerName").Length(256);
            Map(x => x.OfficerPost).Column("OfficerPost").Length(256);
        }
    }
}
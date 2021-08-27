using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ComplaintOtherDepartmentEnquiryMap : BaseAuditEntityMap<ComplaintOtherDepartmentEnquiry, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintOtherDeptEnquiryId).GeneratedBy.Identity().Column("ComplaintOtherDeptEnquiryId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            Map(x => x.RefNum).Column("RefNum").Length(50);
            Map(x => x.ReferralDate).Column("ReferralDate");
            Map(x => x.MemoDate).Column("MemoDate");
            Map(x => x.MemoFromPoliceDate).Column("MemoFromPoliceDate");
            Map(x => x.EnquiryDepartment).Column("EnquiryDepartment").Length(20);
            Map(x => x.OtherEnquiryDepartment).Column("OtherEnquiryDepartment").Length(100);
            Map(x => x.OrgInvolved).Column("OrgInvolved").Length(100);
            Map(x => x.EnquiryContent).Column("EnquiryContent").Length(4000);
            Map(x => x.EnquiryContentHtml).Column("EnquiryContentHtml").CustomType("StringClob");
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(100);
            Map(x => x.Remark).Column("Remark").Length(4000);
            Map(x => x.RemarkHtml).Column("RemarkHtml").CustomType("StringClob");
        }
    }
}
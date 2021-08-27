using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintMasterMap : BaseAuditEntityMap<ComplaintMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintMasterId).GeneratedBy.Identity().Column("ComplaintMasterId");
        }

        protected override void MapEntity()
        {
            References(x => x.PspApprovalHistory).Column("PspApprovalHistoryId");
            References(x => x.FdEvent).Column("FdEventId");
            References(x => x.OrgMaster).Column("OrgId");
            References(x => x.RelatedComplaintMaster).Column("RelatedComplaintMasterId");
            //References(x => x.ComplaintTelRecord).Column("ComplaintMasterId");
            Map(x => x.ComplaintRef).Column("ComplaintRef").Not.Nullable().Length(12);
            Map(x => x.ComplaintRecordType).Column("ComplaintRecordType").Length(20);
            Map(x => x.ComplaintSource).Column("ComplaintSource").Length(20);
            Map(x => x.ComplaintSourceRemark).Column("ComplaintSourceRemark").Length(200);
            Map(x => x.ActivityConcern).Column("ActivityConcern").Length(20);
            Map(x => x.OtherActivityConcern).Column("OtherActivityConcern").Length(100);
            Map(x => x.ComplaintDate).Column("ComplaintDate");
            Map(x => x.FirstComplaintDate).Column("FirstComplaintDate");
            Map(x => x.ReplyDueDate).Column("ReplyDueDate");
            Map(x => x.SwdUnit).Column("SwdUnit").Length(100);
            Map(x => x.LfpsReceiveDate).Column("LfpsReceiveDate");
            Map(x => x.ConcernedOrgName).Column("ConcernedOrgName").Length(1000);
            Map(x => x.ComplainantName).Column("ComplainantName").Length(400);
            Map(x => x.DcLcContent).Column("DcLcContent").Length(4000);
            Map(x => x.DcLcContentHtml).Column("DcLcContentHtml").CustomType("StringClob");
            Map(x => x.NonComplianceNature).Column("NonComplianceNature").Length(100);
            Map(x => x.OtherNonComplianceNature).Column("OtherNonComplianceNature").Length(100);
            Map(x => x.ComplaintPartNum).Column("ComplaintPartNum").Length(20);
            Map(x => x.ComplaintEnclosureNum).Column("ComplaintEnclosureNum").Length(50);
            Map(x => x.ProcessStatus).Column("ProcessStatus").Length(20);
            Map(x => x.ActionFilePartNum).Column("ActionFilePartNum").Length(20);
            Map(x => x.ActionFileEnclosureNum).Column("ActionFileEnclosureNum").Length(50);
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.FundRaisingDate).Column("FundRaisingDate");
            Map(x => x.FundRaisingTime).Column("FundRaisingTime").Length(100);
            Map(x => x.FundRaisingLocation).Column("FundRaisingLocation").Length(256);
            Map(x => x.FundRaiserInvolve).Column("FundRaiserInvolve").Length(3);
            Map(x => x.CollectionMethod).Column("CollectionMethod").Length(20);
            Map(x => x.OtherCollectionMethod).Column("OtherCollectionMethod").Length(100);
            Map(x => x.ComplaintResult).Column("ComplaintResult").Length(20);
            Map(x => x.ComplaintResultRemark).Column("ComplaintResultRemark").Length(4000);
            Map(x => x.ComplaintResultRemarkHtml).Column("ComplaintResultRemarkHtml").CustomType("StringClob");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark").Length(20);
            Map(x => x.OtherWithholdingRemark).Column("OtherWithholdingRemark").Length(4000);
            Map(x => x.OtherWithholdingRemarkHtml).Column("OtherWithholdingRemarkHtml").CustomType("StringClob");
            HasMany(x => x.ComplaintTelRecord).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintAttachment).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintFollowUpAction).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintPoliceCase).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintOtherDepartmentEnquiry).KeyColumn("ComplaintMasterId").Inverse();
        }
    }
}
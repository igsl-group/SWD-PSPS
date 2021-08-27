using Psps.Core.Common;
using Psps.Models.Domain;
using System;

namespace Psps.Data.Mappings
{
    public partial class DisasterStatisticsMap : BaseAuditEntityMap<DisasterStatistics, int>
    {
        protected override void MapId()
        {
            Id(x => x.DisasterStatisticsId).GeneratedBy.Identity().Column("DisasterStatisticsId");
        }

        protected override void MapEntity()
        {
            References(x => x.DisasterMaster).Column("DisasterMasterId");
            Map(x => x.RecordPostId).Column("RecordPostId").Length(20).Not.Nullable();
            Map(x => x.RecordDate).Column("RecordDate").Not.Nullable();
            Map(x => x.PspApplicationProcedurePublicCount).Column("PspApplicationProcedurePublicCount");
            Map(x => x.PspApplicationProcedureOtherCount).Column("PspApplicationProcedureOtherCount");
            Map(x => x.PspScopePublicCount).Column("PspScopePublicCount");
            Map(x => x.PspScopeOtherCount).Column("PspScopeOtherCount");
            Map(x => x.PspApplicationStatusPublicCount).Column("PspApplicationStatusPublicCount");
            Map(x => x.PspApplicationStatusOthersCount).Column("PspApplicationStatusOthersCount");
            Map(x => x.PspPermitConditionCompliancePublicCount).Column("PspPermitConditionCompliancePublicCount");
            Map(x => x.PspPermitConditionComplianceOtherCount).Column("PspPermitConditionComplianceOtherCount");
            Map(x => x.OtherEnquiryPublicCount).Column("OtherEnquiryPublicCount");
            Map(x => x.OtherEnquiryOtherCount).Column("OtherEnquiryOtherCount");
        }
    }
}
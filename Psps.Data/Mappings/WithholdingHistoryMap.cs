using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class WithholdingHistoryMap : BaseEntityMap<WithholdingHistory, int>
    {
        protected override void MapId()
        {
            //Id(x => x.OrgId).GeneratedBy.Assigned().Column("OrgId");
            //Id(x => x.RecordKey).GeneratedBy.Assigned().Column("RecordKey");
            Id(x => x.RowNumber).GeneratedBy.Assigned().Column("RowNumber");
        }

        protected override void MapEntity()
        {
            Map(x => x.RecordKey).Column("RecordKey");
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.WithholdSource).Column("WithholdSource");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.DocSubmission).Column("DocSubmission");
            Map(x => x.OfficialReceiptReceivedDate).Column("OfficialReceiptReceivedDate");
            Map(x => x.NewspaperCuttingReceivedDate).Column("NewspaperCuttingReceivedDate");
            Map(x => x.WithholdingReason).Column("WithholdingReason");
            Map(x => x.DocRemark).Column("DocRemark");
            Map(x => x.Ref).Column("Ref");
        }
    }
}
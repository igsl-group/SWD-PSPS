using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspApprovalHistoryMap : BaseAuditEntityMap<PspApprovalHistory, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspApprovalHistoryId).GeneratedBy.Identity().Column("PspApprovalHistoryId");
        }

        protected override void MapEntity()
        {
            References(x => x.PspMaster).Column("PspMasterId");
            Map(x => x.ApprovalType).Column("ApprovalType").Length(2);
            Map(x => x.ApprovalStatus).Column("ApprovalStatus").Length(2);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.PermitIssueDate).Column("PermitIssueDate");
            //Map(x => x.RejectionLetterDate).Column("RejectionLetterDate");
            //Map(x => x.RepresentationReceiveDate).Column("RepresentationReceiveDate");
            Map(x => x.CancelReason).Column("CancelReason").Length(4000);
            Map(x => x.Remark).Column("Remark").Length(4000);
            Map(x => x.TwoBatchApproachIndicator).Column("TwoBatchApproachIndicator");
            //Map(x => x.PermitRevokeIndicator).Column("PermitRevokeIndicator");
        }
    }
}
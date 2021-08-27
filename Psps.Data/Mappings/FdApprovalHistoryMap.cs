using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdApprovalHistoryMap : BaseAuditEntityMap<FdApprovalHistory, string>
    {
        protected override void MapId()
        {
            Id(x => x.FdYear).GeneratedBy.Assigned().Column("FdYear").Length(5);
        }

        protected override void MapEntity()
        {
            //References(x => x.FdMaster).Column("FdYear");
            Map(x => x.ApproverPostId).Column("ApproverPostId").Length(20);
            Map(x => x.ApproverUserId).Column("ApproverUserId").Length(20);
            Map(x => x.ApprovalDate).Column("ApprovalDate");
            Map(x => x.ApprovalRemark).Column("ApprovalRemark").Length(4000);
        }
    }
}
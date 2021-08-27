using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgEditPspViewMap : BaseEntityMap<OrgEditPspView, int>
    {
        protected override void MapId()
        {
            Id(x => x.RowNum).GeneratedBy.Identity().Column("RowNum");
        }

        protected override void MapEntity()
        {
            Map(x => x.PspMasterId).Column("PspMasterId");
            Map(x => x.PspRef).Column("PspRef");
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.ApplicationDisposalDate).Column("ApplicationDisposalDate");
            Map(x => x.EngFundRaisingPurpose).Column("EngFundRaisingPurpose");
            Map(x => x.ApplicationResult).Column("ApplicationResult");
            Map(x => x.EventPeriodFrom).Column("EventPeriodFrom");
            Map(x => x.EventPeriodTo).Column("EventPeriodTo");
            Map(x => x.TwoBatchApproachIndicator).Column("TwoBatchApproachIndicator");
            Map(x => x.ProcessingOfficerPost).Column("ProcessingOfficerPost");
            Map(x => x.EventApprovedNum).Column("EventApprovedNum");
            Map(x => x.EventHeldNum).Column("EventHeldNum");
            Map(x => x.EventCancelledNum).Column("EventCancelledNum");
            Map(x => x.EventHeldPercent).Column("EventHeldPercent");
            Map(x => x.ArCheckIndicator).Column("ArCheckIndicator");
            Map(x => x.ArCheckIndicatorRaw).Column("ArCheckIndicatorRaw");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.SpecialRemark).Column("SpecialRemark");
            Map(x => x.IsSsaf).Column("IsSsaf");
        }
    }
}
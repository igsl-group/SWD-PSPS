using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdEventMap : BaseAuditEntityMap<FdEvent, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdEventId).GeneratedBy.Identity().Column("FdEventId");
        }

        protected override void MapEntity()
        {
            References(x => x.FdMaster).Column("FdMasterId");
            Map(x => x.FlagDay).Column("FlagDay");
            Map(x => x.FlagTimeFrom).Column("FlagTimeFrom");
            Map(x => x.FlagTimeTo).Column("FlagTimeTo");
            Map(x => x.TWR).Column("TWR").Length(20);
            Map(x => x.TwrDistrict).Column("TwrDistrict").Length(20);
            Map(x => x.CollectionMethod).Column("CollectionMethod").Length(60);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.PermitIssueDate).Column("PermitIssueDate");
            Map(x => x.PermitAcknowledgementReceiveDate).Column("PermitAcknowledgementReceiveDate");
            Map(x => x.BagColour).Column("BagColour").Length(500);
            Map(x => x.FlagColour).Column("FlagColour").Length(500);
            Map(x => x.PermitRevokeIndicator).Column("PermitRevokeIndicator");
            Map(x => x.Remarks).Column("Remarks").Length(60);
            Map(x => x.FrasCharityEventId).Column("FrasCharityEventId").Length(255);
            Map(x => x.FrasResponse).Column("FrasResponse").Length(4000);
            Map(x => x.FrasStatus).Column("FrasStatus").Length(2);
        }
    }
}
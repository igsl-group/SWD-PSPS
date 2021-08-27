using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspEventMap : BaseAuditEntityMap<PspEvent, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspEventId).GeneratedBy.Identity().Column("PspEventId");
        }

        protected override void MapEntity()
        {
            References(x => x.PspMaster).Column("PspMasterId");
            References(x => x.PspApprovalHistory).Column("PspApprovalHistoryId");
            References(x => x.PspCancelHistory).Column("PspCancelHistoryId");
            References(x => x.PspAttachment).Column("PspAttachmentId");
            Map(x => x.EventStatus).Column("EventStatus").Length(2);
            Map(x => x.EventStartDate).Column("EventStartDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.EventStartTime).Column("EventStartTime");
            Map(x => x.EventEndTime).Column("EventEndTime");
            Map(x => x.District).Column("District").Length(20);
            Map(x => x.Location).Column("Location").Length(500);
            Map(x => x.ChiLocation).Column("ChiLocation").Length(500);
            Map(x => x.SimpChiLocation).Column("SimpChiLocation").Length(500);
            Map(x => x.PublicPlaceIndicator).Column("PublicPlaceIndicator");
            Map(x => x.CollectionMethod).Column("CollectionMethod").Length(200);
            Map(x => x.OtherCollectionMethod).Column("OtherCollectionMethod").Length(200);
            Map(x => x.CharitySalesItem).Column("CharitySalesItem").Length(200);
            Map(x => x.Remarks).Column("Remarks").Length(200);
            Map(x => x.ValidationMessage).Column("ValidationMessage").Length(200);
            Map(x => x.ProformaRowNum).Column("ProformaRowNum");
            Map(x => x.FrasCharityEventId).Column("FrasCharityEventId");
            Map(x => x.FrasStatus).Column("FrasStatus").Length(2);
            Map(x => x.FrasResponse).Column("FrasResponse").Length(4000);

            Map(x => x.EventCount).Formula("ISNULL(DATEDIFF(day, EventStartDate, EventEndDate), 0) + 1");
            Map(x => x.Time).Formula("CONVERT(VARCHAR(5),EventStartTime,108) + '-' + CONVERT(VARCHAR(5),EventEndTime,108)");
        }
    }
}
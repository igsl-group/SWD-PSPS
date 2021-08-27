using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgRefGuidePromulgationMap : BaseAuditEntityMap<OrgRefGuidePromulgation, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgRefGuidePromulgationId).GeneratedBy.Identity().Column("OrgRefGuidePromulgationId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.SendDate).Column("SendDate");
            Map(x => x.ReplySlipReceiveDate).Column("ReplySlipReceiveDate");
            Map(x => x.ReplySlipDate).Column("ReplySlipDate");
            Map(x => x.LanguageUsed).Column("LanguageUsed").Length(20);
            Map(x => x.OrgReply).Column("OrgReply").Length(20);
            Map(x => x.PromulgationReason).Column("PromulgationReason").Length(4000);
            Map(x => x.PartNum).Column("PartNum").Length(20);
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(50);
            Map(x => x.ReplySlipPartNum).Column("ReplySlipPartNum").Length(20);
            Map(x => x.ReplySlipEnclosureNum).Column("ReplySlipEnclosureNum").Length(50);
            Map(x => x.Remarks).Column("Remarks").Length(4000);
            Map(x => x.ActivityConcern).Column("ActivityConcern").Length(20);
            Map(x => x.FileRef).Column("FileRef").Length(50);
        }
    }
}
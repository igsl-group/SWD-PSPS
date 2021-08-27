using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class LegalAdviceMasterMap : BaseAuditEntityMap<LegalAdviceMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.LegalAdviceId).GeneratedBy.Identity().Column("LegalAdviceId");
        }

        protected override void MapEntity()
        {
            Map(x => x.RelatedLegalAdviceId).Column("RelatedLegalAdviceId").Length(8);
            Map(x => x.LegalAdviceType).Column("LegalAdviceType").Not.Nullable().Length(20);
            Map(x => x.VenueType).Column("VenueType").Length(20);
            Map(x => x.LegalAdviceCode).Column("LegalAdviceCode").Length(10);
            Map(x => x.LegalAdviceDescription).Column("LegalAdviceDescription").Not.Nullable().Length(1000);
            Map(x => x.PartNum).Column("PartNum").Length(20);
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(50);
            Map(x => x.EffectiveDate).Column("EffectiveDate");
            Map(x => x.RequirePspIndicator).Column("RequirePspIndicator").Length(20);
            Map(x => x.Remarks).Column("Remarks").Length(1000);
        }
    }
}
using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class SuggestionMasterMap : BaseAuditEntityMap<SuggestionMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.SuggestionMasterId).GeneratedBy.Identity().Column("SuggestionMasterId");
        }

        protected override void MapEntity()
        {
            //Map(x => x.SuggestionRefNum).Column("SuggestionRefNum").Not.Nullable().Length(8);
            Map(x => x.SuggestionSource).Column("SuggestionSource").Length(20);
            Map(x => x.SuggestionSourceOther).Column("SuggestionSourceOther").Length(100);
            Map(x => x.SuggestionActivityConcern).Column("SuggestionActivityConcern").Length(20);
            Map(x => x.SuggestionActivityConcernOther).Column("SuggestionActivityConcernOther").Length(100);
            Map(x => x.SuggestionRefNum).Column("SuggestionRefNum").Length(20);
            Map(x => x.SuggestionNature).Column("SuggestionNature").Length(20);
            Map(x => x.SuggestionDate).Column("SuggestionDate");
            Map(x => x.SuggestionSenderName).Column("SuggestionSenderName").Length(100);
            Map(x => x.SuggestionDescription).Column("SuggestionDescription").Length(4000);
            Map(x => x.PartNum).Column("PartNum").Length(20);
            Map(x => x.EnclosureNum).Column("EnclosureNum").Length(50);
            Map(x => x.AcknowledgementSentDate).Column("AcknowledgementSentDate");
            Map(x => x.Remark).Column("Remark").Length(4000);
            HasMany(x => x.SuggestionAttachment).KeyColumn("SuggestionMasterId").Inverse();
        }
    }
}
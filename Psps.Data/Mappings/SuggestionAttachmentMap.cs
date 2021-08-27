using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class SuggestionAttachmentMap : BaseAuditEntityMap<SuggestionAttachment, int>
    {
        protected override void MapId()
        {
            Id(x => x.SuggestionAttachmentId).GeneratedBy.Identity().Column("SuggestionAttachmentId");
        }

        protected override void MapEntity()
        {
            References(x => x.SuggestionMaster).Column("SuggestionMasterId");
            Map(x => x.FileName).Column("FileName").Length(256);
            Map(x => x.FileLocation).Column("FileLocation").Length(400);
            Map(x => x.FileDescription).Column("FileDescription").Length(256);
        }
    }
}
using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspAttachmentMap : BaseAuditEntityMap<PspAttachment, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspAttachmentId).GeneratedBy.Identity().Column("PspAttachmentId");
        }

        protected override void MapEntity()
        {
            References(x => x.PspMaster).Column("PspMasterId");
            Map(x => x.FileLocation).Column("FileLocation").Length(400);
            Map(x => x.FileName).Column("FileName").Length(256);
            Map(x => x.FileDescription).Column("FileDescription").Length(256);
        }
    }
}
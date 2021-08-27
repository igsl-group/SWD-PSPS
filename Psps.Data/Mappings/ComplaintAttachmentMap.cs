using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintAttachmentMap : BaseAuditEntityMap<ComplaintAttachment, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintAttachmentId).GeneratedBy.Identity().Column("ComplaintAttachmentId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            Map(x => x.FileName).Column("FileName").Length(256);
            Map(x => x.FileDescription).Column("FileDescription").Length(256);
            Map(x => x.FileLocation).Column("FileLocation").Length(400);
        }
    }
}
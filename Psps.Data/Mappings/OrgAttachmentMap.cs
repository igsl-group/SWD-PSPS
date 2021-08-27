using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgAttachmentMap : BaseAuditEntityMap<OrgAttachment, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgAttachmentId).GeneratedBy.Identity().Column("OrgAttachmentId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.FileLocation).Column("FileLocation").Length(400);
            Map(x => x.FileName).Column("FileName").Length(256);
            Map(x => x.FileDescription).Column("FileDescription").Length(256);
        }
    }
}
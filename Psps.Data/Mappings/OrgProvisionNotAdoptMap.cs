using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgProvisionNotAdoptMap : BaseAuditEntityMap<OrgProvisionNotAdopt, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgProvisionNotAdoptId).GeneratedBy.Identity().Column("OrgProvisionNotAdoptId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgRefGuidePromulgationId).Column("OrgRefGuidePromulgationId").Not.Nullable().Precision(10);
            Map(x => x.OrgRef).Column("OrgRef").Not.Nullable().Length(8);
            Map(x => x.ProvisionId).Column("ProvisionId").Not.Nullable().Length(20);
        }
    }
}
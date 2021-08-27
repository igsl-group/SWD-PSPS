using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgNameChangeHistoryMap : BaseAuditEntityMap<OrgNameChangeHistory, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgNameChangeId).GeneratedBy.Identity().Column("OrgNameChangeId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.ChangeDate).Column("ChangeDate").Not.Nullable();
            Map(x => x.EngOrgName).Column("EngOrgName").Not.Nullable().Length(255);
            Map(x => x.ChiOrgName).Column("ChiOrgName").Not.Nullable().Length(255);
            Map(x => x.Remarks).Column("Remarks").Length(4000);
        }
    }
}
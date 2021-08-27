using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgFdTabGridViewMap : BaseEntityMap<OrgFdTabGridView, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdMasterId).GeneratedBy.Identity().Column("FdMasterId");
        }

        protected override void MapEntity()
        {
            ReadOnly();

            Map(x => x.OrgId).Not.Nullable();
            Map(x => x.FdRef).Not.Nullable();
            Map(x => x.FdYear);
            Map(x => x.FlagDay);
            Map(x => x.TWR);
            Map(x => x.ApplyForTwr);
            Map(x => x.ApplicationReceiveDate);
            Map(x => x.ApplicationResult);
            Map(x => x.FdGroup);
            Map(x => x.FdGroupDesc);
            Map(x => x.AfsReceiveIndicator);
            Map(x => x.ApplyPledgingMechanismIndicator);
            Map(x => x.NetProceed);
            Map(x => x.PermitNum);
            Map(x => x.PermitIssueDate);
            Map(x => x.FundRaisingPurpose);
            Map(x => x.FdMasterId).Not.Nullable();
        }
    }
}
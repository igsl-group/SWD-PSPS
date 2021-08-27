using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdApproveApplicationListGridViewMap : BaseEntityMap<FdApproveApplicationListGridView, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdMasterId).GeneratedBy.Identity().Column("FdMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.FdMasterId).Column("FdMasterId").Not.Nullable().Precision(10);
            Map(x => x.OrgId).Column("OrgId").Not.Nullable().Precision(10);
            Map(x => x.FdYear).Column("FdYear").Length(5);
            Map(x => x.FdRef).Column("FdRef").Not.Nullable().Length(12);
            Map(x => x.OrgName).Column("OrgName").Not.Nullable().Length(256);
            Map(x => x.FlagDay).Column("FlagDay");
            Map(x => x.TWR).Column("TWR").Length(20);
            Map(x => x.TwrDistrict).Column("TwrDistrict").Length(20);
            Map(x => x.PermitNum).Column("PermitNum").Length(12);
            Map(x => x.ApproveRemarks).Column("ApproveRemarks").Length(60);
            Map(x => x.FrasResponse).Column("FrasResponse");
            Map(x => x.Approve).Column("Approve");
            Map(x => x.FdEventId).Column("FdEventId");
            Map(x => x.RowVersion).Column("RowVersion").Not.Nullable();
            Map(x => x.PermitRevokeIndicator).Column("PermitRevokeIndicator");
        }
    }
}
using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class FdMasterComputeViewMap : BaseEntityMap<FdMasterComputeView, int>
    {
        protected override void MapId()
        {
            Id(x => x.FdMasterId).GeneratedBy.Identity().Column("FdMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.FdMaster1stPrevId).Column("FdMaster1stPrevId");
            Map(x => x.FdMaster2ndPrevId).Column("FdMaster2ndPrevId");
            Map(x => x.ApplicationResultInLastYear).Column("ApplicationResultInLastYear").Length(12);
            Map(x => x.LotGroupInLastYear).Column("LotGroupInLastYear").Length(5);
            Map(x => x.RefLotGroup).Column("RefLotGroup").Length(5);
            References(x => x.FdMaster1stPrev).Column("FdMaster1stPrevId").ReadOnly();
            References(x => x.FdMaster2ndPrev).Column("FdMaster2ndPrevId").ReadOnly();            
        }
    }
}
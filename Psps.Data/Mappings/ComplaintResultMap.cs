using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintResultMap : BaseAuditEntityMap<ComplaintResult, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintResultId).GeneratedBy.Identity().Column("ComplaintResultId");
        }

        protected override void MapEntity()
        {
            References(x => x.ComplaintMaster).Column("ComplaintMasterId");
            Map(x => x.NonComplianceNature).Column("NonComplianceNature").Length(100);
            Map(x => x.OtherNonComplianceNature).Column("OtherNonComplianceNature").Length(100);
            Map(x => x.Result).Column("Result").Length(20);
            Map(x => x.ResultRemark).Column("ResultRemark").Length(4000);
            Map(x => x.ResultRemarkHtml).Column("ResultRemarkHtml").CustomType("StringClob");
        }
    }
}
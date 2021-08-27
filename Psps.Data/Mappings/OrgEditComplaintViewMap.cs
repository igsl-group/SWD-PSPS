using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgEditComplaintViewMap : BaseEntityMap<OrgEditComplaintView, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintMasterId).GeneratedBy.Identity().Column("ComplaintMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.ComplaintRef).Column("ComplaintRef");
            Map(x => x.ComplaintSource).Column("ComplaintSource");
            Map(x => x.ActivityConcern).Column("ActivityConcern");
            Map(x => x.ComplaintDate).Column("ComplaintDate");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.ComplaintRemarks).Column("ComplaintRemarks");
            Map(x => x.FollowUpLetterIssueDate).Column("FollowUpLetterIssueDate");
            Map(x => x.FollowUpLetterType).Column("FollowUpLetterType");
            Map(x => x.LetterIssuedNum).Column("LetterIssuedNum");
        }
    }
}
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Mappings
{
    public partial class PspRecommendEventsViewMap : BaseEntityMap<PspRecommendEventsView, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspMasterId).Column("PspMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OtherEngOrgName).Column("OtherEngOrgName");
            Map(x => x.PspRef).Column("PspRef");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.ProcessingOfficerPost).Column("ProcessingOfficerPost");
            Map(x => x.EventStartDate).Column("EventStartDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.ApprovalType).Column("ApprovalType");
            Map(x => x.TotEventsToBeApproved).Column("TotEventsToBeApproved");
            Map(x => x.RejectionLetterDate).Column("RejectionLetterDate");
            Map(x => x.PermitIssueDate).Column("PermitIssueDate");
            Map(x => x.CancelReason).Column("CancelReason");
            Map(x => x.PspApprovalHistoryId).Column("PspApprovalHistoryId");
            Map(x => x.EngOrgNameSorting).Column("EngOrgNameSorting");
            Map(x => x.ChiOrgName).Column("ChiOrgName");
        }
    }
}
using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class PspSearchViewMap : BaseEntityMap<PspSearchView, int>
    {
        protected override void MapId()
        {
            Id(x => x.PspMasterId).GeneratedBy.Identity().Column("PspMasterId");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");            
            Map(x => x.PspRef).Column("PspRef");
            Map(x => x.SortPspRef).Column("SortPspRef");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
            Map(x => x.BeneficiaryOrg).Column("BeneficiaryOrg");
            Map(x => x.EventStartDate).Column("EventStartDate");
            Map(x => x.EventEndDate).Column("EventEndDate");
            Map(x => x.PermitNum).Column("PermitNum");
            Map(x => x.TotalLocation).Column("TotalLocation");
            Map(x => x.TotEvent).Column("TotEvent");
            Map(x => x.EventApprovedNum).Column("EventApprovedNum");
            Map(x => x.EventHeldNum).Column("EventHeldNum");
            Map(x => x.EventCancelledNum).Column("EventCancelledNum");
            Map(x => x.EventHeldPercent).Column("EventHeldPercent");
            Map(x => x.OverdueIndicator).Column("OverdueIndicator");
            Map(x => x.LateIndicator).Column("LateIndicator");
            Map(x => x.ApplicationResult).Column("ApplicationResult");
            Map(x => x.RejectReason).Column("RejectReason");
            Map(x => x.RejectRemark).Column("RejectRemark");
            Map(x => x.ApplicationDisposalDate).Column("ApplicationDisposalDate");
            Map(x => x.ApplicationCompletionDate).Column("ApplicationCompletionDate");
            Map(x => x.QualityOpinionDetail).Column("QualityOpinionDetail");
            Map(x => x.ProcessingOfficerPost).Column("ProcessingOfficerPost");
            Map(x => x.ContactPersonName).Formula("isnull(ContactPersonFirstName,'') + ' ' + isnull(ContactPersonLastName,'') ");
            Map(x => x.ContactPersonChiName).Formula("isnull(ContactPersonChiFirstName,'') + isnull(ContactPersonChiLastName,'') ");
            Map(x => x.ContactPersonEmailAddress).Column("ContactPersonEmailAddress");
            Map(x => x.ContactPerson).Column("ContactPerson");
            Map(x => x.PreviousPspMasterId).Column("PreviousPspMasterId");
            Map(x => x.PreviousPspRef).Column("PreviousPspRef");
            Map(x => x.NewApplicantIndicator).Column("NewApplicantIndicator");
            Map(x => x.DisasterMasterId).Column("DisasterMasterId");
            Map(x => x.ReSubmit).Column("ReSubmit");
            Map(x => x.ReEvents).Column("ReEvents");
            Map(x => x.ApplicationDate).Column("ApplicationDate");
            Map(x => x.SpecialRemark).Column("SpecialRemark");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.IsSsaf).Column("IsSsaf");

            HasMany(x => x.PspApprovalHistory).KeyColumn("PspMasterId").Inverse();
            HasMany(x => x.PspAttachment).KeyColumn("PspMasterId").Inverse();
            HasMany(x => x.PspEvent).KeyColumn("PspMasterId").Inverse();
        }
    }
}
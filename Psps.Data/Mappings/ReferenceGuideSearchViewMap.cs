using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class ReferenceGuideSearchViewMap : BaseEntityMap<ReferenceGuideSearchView, int>
    {
        public ReferenceGuideSearchViewMap()
            : base()
        {
            ReadOnly();
        }

        protected override void MapId()
        {
            Id(x => x.RowNumber).GeneratedBy.Assigned().Column("RowNumber");
        }

        protected override void MapEntity()
        {
            References(x => x.OrgMaster).Column("OrgId");
            Map(x => x.OrgRefGuidePromulgationId).Column("OrgRefGuidePromulgationId");
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.OrgRef).Column("OrgRef");
            Map(x => x.DisableIndicator).Column("DisableIndicator");
            Map(x => x.EngOrgName).Column("EngOrgName");
            Map(x => x.ChiOrgName).Column("ChiOrgName");
            Map(x => x.EngOrgNameSorting).Column("EngOrgNameSorting");
            Map(x => x.SubventedIndicator).Column("SubventedIndicator");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.AddressProofIndicator).Column("AddressProofIndicator");
            Map(x => x.AnnualReport1Indicator).Column("AnnualReport1Indicator");
            Map(x => x.Section88StartDate).Column("Section88StartDate");
            Map(x => x.RegistrationType1).Column("RegistrationType1");
            Map(x => x.RegistrationOtherName1).Column("RegistrationOtherName1");
            Map(x => x.RegistrationDate1).Column("RegistrationDate1");
            Map(x => x.RegistrationType2).Column("RegistrationType2");
            Map(x => x.RegistrationOtherName2).Column("RegistrationOtherName2");
            Map(x => x.RegistrationDate2).Column("RegistrationDate2");
            Map(x => x.AppliedPSPBefore).Column("AppliedPSPBefore");
            Map(x => x.AppliedSSAFBefore).Column("AppliedSSAFBefore");
            Map(x => x.AppliedFDBefore).Column("AppliedFDBefore");
            Map(x => x.AppliedFDBeforeYears).Column("AppliedFDBeforeYears");
            Map(x => x.PSPIssuedBefore).Column("PSPIssuedBefore");
            Map(x => x.SSAFIssuedBefore).Column("SSAFIssuedBefore");
            Map(x => x.FDPermitIssuedBefore).Column("FDPermitIssuedBefore");
            Map(x => x.FDPermitIssuedBeforeYears).Column("FDPermitIssuedBeforeYears");
            Map(x => x.OrgReply).Column("OrgReply");
            Map(x => x.SendDate).Column("SendDate");
            Map(x => x.ReplySlipReceiveDate).Column("ReplySlipReceiveDate");
            Map(x => x.EngMailingAddress1).Column("EngMailingAddress1");
            Map(x => x.EngMailingAddress2).Column("EngMailingAddress2");
            Map(x => x.EngMailingAddress3).Column("EngMailingAddress3");
            Map(x => x.EngMailingAddress4).Column("EngMailingAddress4");
            Map(x => x.EngMailingAddress5).Column("EngMailingAddress5");            
            Map(x => x.ChiMailingAddress1).Column("ChiMailingAddress1");
            Map(x => x.ChiMailingAddress2).Column("ChiMailingAddress2");
            Map(x => x.ChiMailingAddress3).Column("ChiMailingAddress3");
            Map(x => x.ChiMailingAddress4).Column("ChiMailingAddress4");
            Map(x => x.ChiMailingAddress5).Column("ChiMailingAddress5");
            Map(x => x.ContactPerson).Column("ContactPerson");
            Map(x => x.EmailAddress).Column("EmailAddress");
            Map(x => x.PartNum).Column("PartNum");
            Map(x => x.EnclosureNum).Column("EnclosureNum");
            Map(x => x.ReplySlipPartNum).Column("ReplySlipPartNum");
            Map(x => x.ReplySlipEnclosureNum).Column("ReplySlipEnclosureNum");

            Map(x => x.ActivityConcern).Column("ActivityConcern");
            Map(x => x.ActivityConcernDesc).Column("ActivityConcernDesc"); 
            Map(x => x.FileRef).Column("FileRef");
            Map(x => x.OrgProvisionNotAdopts).Column("OrgProvisionNotAdopts");
            Map(x => x.PromulgationReason).Column("PromulgationReason");
            Map(x => x.Remarks).Column("Remarks");
            Map(x => x.Approved).Column("Approved");

            HasMany(x => x.PspMasters).PropertyRef("OrgId").KeyColumn("OrgId").Inverse();
            HasMany(x => x.FdMasters).PropertyRef("OrgId").KeyColumn("OrgId").Inverse();
            //HasMany(x => x.PspMasters).KeyColumn("OrgId").Inverse();
            //HasMany(x => x.FdMasters).KeyColumn("OrgId").Inverse();
        }
    }
}
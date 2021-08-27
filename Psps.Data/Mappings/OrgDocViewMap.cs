using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgDocViewMap : BaseEntityMap<OrgDocView, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgId).GeneratedBy.Identity().Column("OrgId");
        }

        protected override void MapEntity()
        {
            Map(x => x.DocumentDate).Column("DocumentDate").Not.Nullable();
            Map(x => x.OrgRef).Column("OrgRef").Not.Nullable();
            Map(x => x.DisableIndicator).Column("DisableIndicator").Not.Nullable();
            Map(x => x.EngOrgName).Column("EngOrgName").Not.Nullable();
            Map(x => x.ChiOrgName).Column("ChiOrgName").Not.Nullable();
            Map(x => x.SimpChiOrgName).Column("SimpChiOrgName");
            Map(x => x.EngOrgNameSorting).Column("EngOrgNameSorting").Not.Nullable();
            Map(x => x.OtherEngOrgName).Column("OtherEngOrgName");
            Map(x => x.OtherChiOrgName).Column("OtherChiOrgName");
            Map(x => x.OtherSimpChiOrgName).Column("OtherSimpChiOrgName");
            Map(x => x.EngRegisteredAddress1).Column("EngRegisteredAddress1");
            Map(x => x.EngRegisteredAddress2).Column("EngRegisteredAddress2");
            Map(x => x.EngRegisteredAddress3).Column("EngRegisteredAddress3");
            Map(x => x.EngRegisteredAddress4).Column("EngRegisteredAddress4");
            Map(x => x.EngRegisteredAddress5).Column("EngRegisteredAddress5");
            Map(x => x.EngRegisteredAddressFull).Column("EngRegisteredAddressFull");
            Map(x => x.ChiRegisteredAddress1).Column("ChiRegisteredAddress1");
            Map(x => x.ChiRegisteredAddress2).Column("ChiRegisteredAddress2");
            Map(x => x.ChiRegisteredAddress3).Column("ChiRegisteredAddress3");
            Map(x => x.ChiRegisteredAddress4).Column("ChiRegisteredAddress4");
            Map(x => x.ChiRegisteredAddress5).Column("ChiRegisteredAddress5");
            Map(x => x.ChiRegisteredAddressFull).Column("ChiRegisteredAddressFull");
            Map(x => x.EngMailingAddress1).Column("EngMailingAddress1");
            Map(x => x.EngMailingAddress2).Column("EngMailingAddress2");
            Map(x => x.EngMailingAddress3).Column("EngMailingAddress3");
            Map(x => x.EngMailingAddress4).Column("EngMailingAddress4");
            Map(x => x.EngMailingAddress5).Column("EngMailingAddress5");
            Map(x => x.EngMailingAddressFull).Column("EngMailingAddressFull");
            Map(x => x.ChiMailingAddress1).Column("ChiMailingAddress1");
            Map(x => x.ChiMailingAddress2).Column("ChiMailingAddress2");
            Map(x => x.ChiMailingAddress3).Column("ChiMailingAddress3");
            Map(x => x.ChiMailingAddress4).Column("ChiMailingAddress4");
            Map(x => x.ChiMailingAddress5).Column("ChiMailingAddress5");
            Map(x => x.ChiMailingAddressFull).Column("ChiMailingAddressFull");
            Map(x => x.URL).Column("URL");
            Map(x => x.TelNum).Column("TelNum");
            Map(x => x.FaxNum).Column("FaxNum");
            Map(x => x.EmailAddress).Column("EmailAddress");
            Map(x => x.ApplicantFirstName).Column("ApplicantFirstName");
            Map(x => x.ApplicantLastName).Column("ApplicantLastName");
            Map(x => x.ApplicantEngSalute).Column("ApplicantEngSalute");
            Map(x => x.ContactPersonName).Column("ContactPersonName");
            Map(x => x.ApplicantChiLastName).Column("ApplicantChiLastName");
            Map(x => x.ApplicantChiFirstName).Column("ApplicantChiFirstName");
            Map(x => x.ApplicantChiSalute).Column("ApplicantChiSalute");
            Map(x => x.ContactPersonChineseName).Column("ContactPersonChineseName");
            Map(x => x.ApplicantPosition).Column("ApplicantPosition");
            Map(x => x.ApplicantTelNum).Column("ApplicantTelNum");
            Map(x => x.PresidentName).Column("PresidentName");
            Map(x => x.SecretaryName).Column("SecretaryName");
            Map(x => x.TreasurerName).Column("TreasurerName");
            Map(x => x.OrgObjective).Column("OrgObjective");
            Map(x => x.SubventedIndicator).Column("SubventedIndicator");
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.Section88StartDate).Column("Section88StartDate");
            Map(x => x.RegistrationType1).Column("RegistrationType1");
            Map(x => x.RegistrationOtherName1).Column("RegistrationOtherName1");
            Map(x => x.RegistrationDate1).Column("RegistrationDate1");
            Map(x => x.RegistrationType2).Column("RegistrationType2");
            Map(x => x.RegistrationOtherName2).Column("RegistrationOtherName2");
            Map(x => x.RegistrationDate2).Column("RegistrationDate2");
            Map(x => x.AddressProofIndicator).Column("AddressProofIndicator");
            Map(x => x.AddressProofAttachmentId).Column("AddressProofAttachmentId");
            Map(x => x.AddressProofDate).Column("AddressProofDate");
            Map(x => x.MaaConstitutionIndicator).Column("MaaConstitutionIndicator");
            Map(x => x.MaaConstitutionAttachmentId).Column("MaaConstitutionAttachmentId");
            Map(x => x.AnnualReport1Indicator).Column("AnnualReport1Indicator");
            Map(x => x.AnnualReport1BeginDate).Column("AnnualReport1BeginDate");
            Map(x => x.AnnualReport1EndDate).Column("AnnualReport1EndDate");
            Map(x => x.AnnualReport1AttachmentId).Column("AnnualReport1AttachmentId");
            Map(x => x.AnnualReport2Indicator).Column("AnnualReport2Indicator");
            Map(x => x.AnnualReport2BeginDate).Column("AnnualReport2BeginDate");
            Map(x => x.AnnualReport2EndDate).Column("AnnualReport2EndDate");
            Map(x => x.AnnualReport2AttachmentId).Column("AnnualReport2AttachmentId");
            Map(x => x.AnnualReport3Indicator).Column("AnnualReport3Indicator");
            Map(x => x.AnnualReport3BeginDate).Column("AnnualReport3BeginDate");
            Map(x => x.AnnualReport3EndDate).Column("AnnualReport3EndDate");
            Map(x => x.AnnualReport3AttachmentId).Column("AnnualReport3AttachmentId");
            Map(x => x.Afs1Indicator).Column("Afs1Indicator");
            Map(x => x.Afs1YearEnd).Column("Afs1YearEnd");
            Map(x => x.Afs1AttachmentId).Column("Afs1AttachmentId");
            Map(x => x.Afs2Indicator).Column("Afs2Indicator");
            Map(x => x.Afs2YearEnd).Column("Afs2YearEnd");
            Map(x => x.Afs2AttachmentId).Column("Afs2AttachmentId");
            Map(x => x.Afs3Indicator).Column("Afs3Indicator");
            Map(x => x.Afs3YearEnd).Column("Afs3YearEnd");
            Map(x => x.Afs3AttachmentId).Column("Afs3AttachmentId");
            Map(x => x.QualifiedOpinionIndicator).Column("QualifiedOpinionIndicator");
            Map(x => x.QualifiedOpinionRemark).Column("QualifiedOpinionRemark");
            Map(x => x.OtherSupportDocIndicator).Column("OtherSupportDocIndicator");
            Map(x => x.OtherSupportDocRemark).Column("OtherSupportDocRemark");
            Map(x => x.OtherSupportDocAttachmentId).Column("OtherSupportDocAttachmentId");
            Map(x => x.OverallRemark).Column("OverallRemark");
            Map(x => x.RefGuideReplySlipReceiveDate).Column("RefGuideReplySlipReceiveDate");
            Map(x => x.RefGuidePartNum).Column("RefGuidePartNum");
            Map(x => x.RefGuideEnclosureNum).Column("RefGuideEnclosureNum");
            Map(x => x.RefGuideReplyPartNum).Column("RefGuideReplyPartNum");
            Map(x => x.RefGuideReplyEnclosureNum).Column("RefGuideReplyEnclosureNum");            
            Map(x => x.EngSFCName).Column("EngSFCName");
            Map(x => x.ChiSFCName).Column("ChiSFCName");
            Map(x => x.SFCTel).Column("SFCTel");
            Map(x => x.SFCEmail).Column("SFCEmail");
            Map(x => x.EngEOIILF5Name).Column("EngEOIILF5Name");
            Map(x => x.ChiEOIILF5Name).Column("ChiEOIILF5Name");
            Map(x => x.EOIILF5Tel).Column("EOIILF5Tel");
            Map(x => x.EOIILF5Email).Column("EOIILF5Email");
            Map(x => x.EngCEOLFName).Column("EngCEOLFName");
            Map(x => x.ChiCEOLFName).Column("ChiCEOLFName");
        }
    }
}
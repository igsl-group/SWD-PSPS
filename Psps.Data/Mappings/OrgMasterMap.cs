using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class OrgMasterMap : BaseAuditEntityMap<OrgMaster, int>
    {
        protected override void MapId()
        {
            Id(x => x.OrgId).GeneratedBy.Identity().Column("OrgId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgRef).Column("OrgRef").Not.Nullable().Length(8);
            Map(x => x.DisableIndicator).Column("DisableIndicator").Not.Nullable();
            Map(x => x.EngOrgName).Column("EngOrgName").Not.Nullable().Length(255);
            Map(x => x.ChiOrgName).Column("ChiOrgName").Not.Nullable().Length(255);
            Map(x => x.SimpChiOrgName).Column("SimpChiOrgName").Length(255);
            Map(x => x.EngOrgNameSorting).Column("EngOrgNameSorting").Not.Nullable().Length(256);
            Map(x => x.OtherEngOrgName).Column("OtherEngOrgName").Length(256);
            Map(x => x.OtherChiOrgName).Column("OtherChiOrgName").Length(100);
            Map(x => x.OtherSimpChiOrgName).Column("OtherSimpChiOrgName").Length(100);
            Map(x => x.EngRegisteredAddress1).Column("EngRegisteredAddress1").Length(400);
            Map(x => x.EngRegisteredAddress2).Column("EngRegisteredAddress2").Length(200);
            Map(x => x.EngRegisteredAddress3).Column("EngRegisteredAddress3").Length(200);
            Map(x => x.EngRegisteredAddress4).Column("EngRegisteredAddress4").Length(200);
            Map(x => x.EngRegisteredAddress5).Column("EngRegisteredAddress5").Length(200);
            Map(x => x.ChiRegisteredAddress1).Column("ChiRegisteredAddress1").Length(100);
            Map(x => x.ChiRegisteredAddress2).Column("ChiRegisteredAddress2").Length(100);
            Map(x => x.ChiRegisteredAddress3).Column("ChiRegisteredAddress3").Length(100);
            Map(x => x.ChiRegisteredAddress4).Column("ChiRegisteredAddress4").Length(100);
            Map(x => x.ChiRegisteredAddress5).Column("ChiRegisteredAddress5").Length(100);
            Map(x => x.EngMailingAddress1).Column("EngMailingAddress1").Length(200);
            Map(x => x.EngMailingAddress2).Column("EngMailingAddress2").Length(200);
            Map(x => x.EngMailingAddress3).Column("EngMailingAddress3").Length(200);
            Map(x => x.EngMailingAddress4).Column("EngMailingAddress4").Length(200);
            Map(x => x.EngMailingAddress5).Column("EngMailingAddress5").Length(200);
            Map(x => x.ChiMailingAddress1).Column("ChiMailingAddress1").Length(100);
            Map(x => x.ChiMailingAddress2).Column("ChiMailingAddress2").Length(100);
            Map(x => x.ChiMailingAddress3).Column("ChiMailingAddress3").Length(100);
            Map(x => x.ChiMailingAddress4).Column("ChiMailingAddress4").Length(100);
            Map(x => x.ChiMailingAddress5).Column("ChiMailingAddress5").Length(100);
            Map(x => x.URL).Column("URL").Length(100);
            Map(x => x.TelNum).Column("TelNum").Length(50);
            Map(x => x.FaxNum).Column("FaxNum").Length(50);
            Map(x => x.EmailAddress).Column("EmailAddress").Length(200);
            Map(x => x.ApplicantSalute).Column("ApplicantSalute").Length(20);
            Map(x => x.ApplicantFirstName).Column("ApplicantFirstName").Length(100);
            Map(x => x.ApplicantLastName).Column("ApplicantLastName").Length(50);
            Map(x => x.ApplicantChiFirstName).Column("ApplicantChiFirstName").Length(5);
            Map(x => x.ApplicantChiLastName).Column("ApplicantChiLastName").Length(10);
            Map(x => x.ApplicantPosition).Column("ApplicantPosition").Length(100);
            Map(x => x.ApplicantTelNum).Column("ApplicantTelNum").Length(50);
            Map(x => x.PresidentName).Column("PresidentName").Length(100);
            Map(x => x.SecretaryName).Column("SecretaryName").Length(100);
            Map(x => x.TreasurerName).Column("TreasurerName").Length(100);
            Map(x => x.OrgObjective).Column("OrgObjective").Length(4000);
            Map(x => x.SubventedIndicator).Column("SubventedIndicator").Not.Nullable();
            Map(x => x.Section88Indicator).Column("Section88Indicator");
            Map(x => x.Section88StartDate).Column("Section88StartDate");
            Map(x => x.RegistrationType1).Column("RegistrationType1").Length(20);
            Map(x => x.RegistrationOtherName1).Column("RegistrationOtherName1").Length(100);
            Map(x => x.RegistrationDate1).Column("RegistrationDate1");
            Map(x => x.RegistrationType2).Column("RegistrationType2").Length(20);
            Map(x => x.RegistrationOtherName2).Column("RegistrationOtherName2").Length(100);
            Map(x => x.RegistrationDate2).Column("RegistrationDate2");
            //Map(x => x.BrNum1).Column("BrNum1").Length(8);
            //Map(x => x.BrNum2).Column("BrNum2").Length(3);
            //Map(x => x.CrNum).Column("CrNum").Length(20);
            Map(x => x.AddressProofIndicator).Column("AddressProofIndicator");
            Map(x => x.AddressProofDate).Column("AddressProofDate");
            Map(x => x.AddressProofAttachmentId).Column("AddressProofAttachmentId");
            Map(x => x.MaaConstitutionIndicator).Column("MaaConstitutionIndicator");
            Map(x => x.MaaConstitutionAttachmentId).Column("MaaConstitutionAttachmentId").Precision(10);
            Map(x => x.QualifiedOpinionIndicator).Column("QualifiedOpinionIndicator");
            Map(x => x.QualifiedOpinionRemark).Column("QualifiedOpinionRemark").Length(4000);
            Map(x => x.OtherSupportDocIndicator).Column("OtherSupportDocIndicator");
            Map(x => x.OtherSupportDocRemark).Column("OtherSupportDocRemark").Length(4000);
            Map(x => x.OtherSupportDocAttachmentId).Column("OtherSupportDocAttachmentId").Precision(10);
            Map(x => x.OverallRemark).Column("OverallRemark").Length(4000);
            Map(x => x.FrasOrganisationId).Column("FrasOrganisationId");
            Map(x => x.OrgNameEngChi).Formula("EngOrgName + CHAR(10) + CHAR(13) + ChiOrgName").ReadOnly();

            HasMany(x => x.ComplaintMaster).KeyColumn("OrgId").Inverse();
            HasMany(x => x.FdMaster).KeyColumn("OrgId").Inverse();
            HasMany(x => x.OrgAttachment).KeyColumn("OrgId").Inverse();
            HasMany(x => x.OrgNameChangeHistory).KeyColumn("OrgId").Inverse();
            //HasOne(x => x.OrgRefGuidePromulgation).ForeignKey("OrgId");
            HasMany(x => x.OrgRefGuidePromulgations).KeyColumn("OrgId").Inverse();
            //HasMany(x => x.BeneficiaryPspMasters).KeyColumn("BeneficiaryOrgId").Inverse();
            HasMany(x => x.PspMasters).KeyColumn("OrgId").Inverse();
            HasMany(x => x.ReferenceGuideSearchViews).KeyColumn("OrgId").Inverse();
        }
    }
}
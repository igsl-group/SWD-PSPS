using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgDocView : BaseEntity<int>
    {
        public virtual DateTime DocumentDate { get; set; }

        public virtual int OrgId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual bool DisableIndicator { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual string SimpChiOrgName { get; set; }

        public virtual string EngOrgNameSorting { get; set; }

        public virtual string OtherEngOrgName { get; set; }

        public virtual string OtherChiOrgName { get; set; }

        public virtual string OtherSimpChiOrgName { get; set; }

        public virtual string EngRegisteredAddress1 { get; set; }

        public virtual string EngRegisteredAddress2 { get; set; }

        public virtual string EngRegisteredAddress3 { get; set; }

        public virtual string EngRegisteredAddress4 { get; set; }

        public virtual string EngRegisteredAddress5 { get; set; }

        public virtual string EngRegisteredAddressFull { get; set; }

        public virtual string ChiRegisteredAddress1 { get; set; }

        public virtual string ChiRegisteredAddress2 { get; set; }

        public virtual string ChiRegisteredAddress3 { get; set; }

        public virtual string ChiRegisteredAddress4 { get; set; }

        public virtual string ChiRegisteredAddress5 { get; set; }

        public virtual string ChiRegisteredAddressFull { get; set; }

        public virtual string EngMailingAddress1 { get; set; }

        public virtual string EngMailingAddress2 { get; set; }

        public virtual string EngMailingAddress3 { get; set; }

        public virtual string EngMailingAddress4 { get; set; }

        public virtual string EngMailingAddress5 { get; set; }

        public virtual string EngMailingAddressFull { get; set; }

        public virtual string ChiMailingAddress1 { get; set; }

        public virtual string ChiMailingAddress2 { get; set; }

        public virtual string ChiMailingAddress3 { get; set; }

        public virtual string ChiMailingAddress4 { get; set; }

        public virtual string ChiMailingAddress5 { get; set; }

        public virtual string ChiMailingAddressFull { get; set; }

        public virtual string URL { get; set; }

        public virtual string TelNum { get; set; }

        public virtual string FaxNum { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string ApplicantFirstName { get; set; }

        public virtual string ApplicantLastName { get; set; }

        public virtual string ApplicantEngSalute { get; set; }

        public virtual string ContactPersonName { get; set; }

        public virtual string ApplicantChiLastName { get; set; }

        public virtual string ApplicantChiFirstName { get; set; }

        public virtual string ApplicantChiSalute { get; set; }

        public virtual string ContactPersonChineseName { get; set; }

        public virtual string ApplicantPosition { get; set; }

        public virtual string ApplicantTelNum { get; set; }

        public virtual string PresidentName { get; set; }

        public virtual string SecretaryName { get; set; }

        public virtual string TreasurerName { get; set; }

        public virtual string OrgObjective { get; set; }

        public virtual bool? SubventedIndicator { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual DateTime? Section88StartDate { get; set; }

        public virtual string RegistrationType1 { get; set; }

        public virtual string RegistrationOtherName1 { get; set; }

        public virtual DateTime? RegistrationDate1 { get; set; }

        public virtual string RegistrationType2 { get; set; }

        public virtual string RegistrationOtherName2 { get; set; }

        public virtual DateTime? RegistrationDate2 { get; set; }

        public virtual bool? AddressProofIndicator { get; set; }

        public virtual int? AddressProofAttachmentId { get; set; }

        public virtual DateTime? AddressProofDate { get; set; }

        public virtual bool? MaaConstitutionIndicator { get; set; }

        public virtual int? MaaConstitutionAttachmentId { get; set; }

        public virtual bool? AnnualReport1Indicator { get; set; }

        public virtual DateTime? AnnualReport1BeginDate { get; set; }

        public virtual DateTime? AnnualReport1EndDate { get; set; }

        public virtual int? AnnualReport1AttachmentId { get; set; }

        public virtual bool? AnnualReport2Indicator { get; set; }

        public virtual DateTime? AnnualReport2BeginDate { get; set; }

        public virtual DateTime? AnnualReport2EndDate { get; set; }

        public virtual int? AnnualReport2AttachmentId { get; set; }

        public virtual bool? AnnualReport3Indicator { get; set; }

        public virtual DateTime? AnnualReport3BeginDate { get; set; }

        public virtual DateTime? AnnualReport3EndDate { get; set; }

        public virtual int? AnnualReport3AttachmentId { get; set; }

        public virtual bool? Afs1Indicator { get; set; }

        public virtual DateTime? Afs1YearEnd { get; set; }

        public virtual int? Afs1AttachmentId { get; set; }

        public virtual bool? Afs2Indicator { get; set; }

        public virtual DateTime? Afs2YearEnd { get; set; }

        public virtual int? Afs2AttachmentId { get; set; }

        public virtual bool? Afs3Indicator { get; set; }

        public virtual DateTime? Afs3YearEnd { get; set; }

        public virtual int? Afs3AttachmentId { get; set; }

        public virtual bool? QualifiedOpinionIndicator { get; set; }

        public virtual string QualifiedOpinionRemark { get; set; }

        public virtual bool? OtherSupportDocIndicator { get; set; }

        public virtual string OtherSupportDocRemark { get; set; }

        public virtual int? OtherSupportDocAttachmentId { get; set; }

        public virtual string OverallRemark { get; set; }

        public virtual DateTime? RefGuideReplySlipReceiveDate { get; set; }

        public virtual string RefGuidePartNum { get; set; }

        public virtual string RefGuideEnclosureNum { get; set; }

        public virtual string RefGuideReplyPartNum { get; set; }

        public virtual string RefGuideReplyEnclosureNum { get; set; }

        public virtual string EngSFCName { get; set; }

        public virtual string ChiSFCName { get; set; }

        public virtual string SFCTel { get; set; }

        public virtual string SFCEmail { get; set; }
        
        public virtual string EngEOIILF5Name { get; set; }

        public virtual string ChiEOIILF5Name { get; set; }

        public virtual string EOIILF5Tel { get; set; }

        public virtual string EOIILF5Email { get; set; }

        public virtual string EngCEOLFName { get; set; }

        public virtual string ChiCEOLFName { get; set; }

        public override int Id
        {
            get
            {
                return OrgId;
            }
            set
            {
                OrgId = value;
            }
        }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Domain
{
    public partial class OrgMasterSearchView : BaseEntity<int>
    {
        public OrgMasterSearchView()
        {
            FdMaster = new List<FdMaster>();
            PspMasters = new List<PspMaster>();
            ComplaintMaster = new List<ComplaintMaster>();
            OrgNameChangeHistory = new List<OrgNameChangeHistory>();
        }

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

        public virtual string ChiRegisteredAddress1 { get; set; }

        public virtual string ChiRegisteredAddress2 { get; set; }

        public virtual string ChiRegisteredAddress3 { get; set; }

        public virtual string ChiRegisteredAddress4 { get; set; }

        public virtual string ChiRegisteredAddress5 { get; set; }

        public virtual string EngMailingAddress1 { get; set; }

        public virtual string EngMailingAddress2 { get; set; }

        public virtual string EngMailingAddress3 { get; set; }

        public virtual string EngMailingAddress4 { get; set; }

        public virtual string EngMailingAddress5 { get; set; }

        public virtual string ChiMailingAddress1 { get; set; }

        public virtual string ChiMailingAddress2 { get; set; }

        public virtual string ChiMailingAddress3 { get; set; }

        public virtual string ChiMailingAddress4 { get; set; }

        public virtual string ChiMailingAddress5 { get; set; }

        public virtual string URL { get; set; }

        public virtual string TelNum { get; set; }

        public virtual string FaxNum { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string ApplicantSalute { get; set; }

        public virtual string ApplicantFirstName { get; set; }

        public virtual string ApplicantLastName { get; set; }

        public virtual string ApplicantChiFirstName { get; set; }

        public virtual string ApplicantChiLastName { get; set; }

        public virtual string ApplicantPosition { get; set; }

        public virtual string ApplicantTelNum { get; set; }

        public virtual string PresidentName { get; set; }

        public virtual string SecretaryName { get; set; }

        public virtual string TreasurerName { get; set; }

        public virtual string OrgObjective { get; set; }

        public virtual bool SubventedIndicator { get; set; }

        public virtual bool? Section88Indicator { get; set; }

        public virtual DateTime? Section88StartDate { get; set; }

        public virtual string RegistrationType1 { get; set; }

        public virtual string RegistrationOtherName1 { get; set; }

        public virtual DateTime? RegistrationDate1 { get; set; }

        public virtual string RegistrationType2 { get; set; }

        public virtual string RegistrationOtherName2 { get; set; }

        public virtual DateTime? RegistrationDate2 { get; set; }

        public virtual bool? AddressProofIndicator { get; set; }

        public virtual DateTime? AddressProofDate { get; set; }

        public virtual int? AddressProofAttachmentId { get; set; }

        public virtual bool? MaaConstitutionIndicator { get; set; }

        public virtual int? MaaConstitutionAttachmentId { get; set; }

        public virtual bool? QualifiedOpinionIndicator { get; set; }

        public virtual string QualifiedOpinionRemark { get; set; }

        public virtual bool? OtherSupportDocIndicator { get; set; }

        public virtual string OtherSupportDocRemark { get; set; }

        public virtual int? OtherSupportDocAttachmentId { get; set; }

        public virtual string OverallRemark { get; set; }

        public virtual DateTime? FdApplicationReceiveDate { get; set; }

        public virtual DateTime? PspApplicationReceiveDate { get; set; }

        public virtual bool AppliedPSPBefore { get; set; }

        public virtual bool AppliedFDBefore { get; set; }

        public virtual bool AppliedSSAFBefore { get; set; }        

        public virtual bool ReceivedComplaintEnquiryBefore { get; set; }

        public virtual int PSPIssuedNum { get; set; }

        public virtual int FDPermitIssuedNum { get; set; }

        public virtual int SSAFPermitIssuedNum { get; set; }

        public virtual int ComplaintReceivedNum { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual bool WithholdingInd { get; set; }

        public virtual IList<PspMaster> PspMasters { get; set; }

        public virtual IList<FdMaster> FdMaster { get; set; }

        public virtual IList<ComplaintMaster> ComplaintMaster { get; set; }

        public virtual IList<OrgNameChangeHistory> OrgNameChangeHistory { get; set; }

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
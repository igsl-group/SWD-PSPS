using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ReferenceGuideSearchView : BaseEntity<int>
    {
        public ReferenceGuideSearchView()
        {
            OrgMaster = new OrgMaster();
            PspMasters = new List<PspMaster>();
            FdMasters = new List<FdMaster>();
        }

        public virtual int RowNumber { get; set; }

        public virtual int OrgRefGuidePromulgationId { get; set; }

        public virtual int OrgId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual bool DisableIndicator { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual string EngOrgNameSorting { get; set; }

        public virtual bool SubventedIndicator { get; set; }

        public virtual bool Section88Indicator { get; set; }

        public virtual bool AddressProofIndicator { get; set; }

        public virtual bool AnnualReport1Indicator { get; set; }

        public virtual DateTime? Section88StartDate { get; set; }

        public virtual string RegistrationType1 { get; set; }

        public virtual string RegistrationOtherName1 { get; set; }

        public virtual DateTime? RegistrationDate1 { get; set; }

        public virtual string RegistrationType2 { get; set; }

        public virtual string RegistrationOtherName2 { get; set; }

        public virtual DateTime? RegistrationDate2 { get; set; }

        public virtual bool AppliedPSPBefore { get; set; }

        public virtual bool AppliedSSAFBefore { get; set; }

        public virtual bool AppliedFDBefore { get; set; }

        public virtual string AppliedFDBeforeYears { get; set; }

        public virtual bool PSPIssuedBefore { get; set; }

        public virtual bool SSAFIssuedBefore { get; set; }

        public virtual bool FDPermitIssuedBefore { get; set; }

        public virtual string FDPermitIssuedBeforeYears { get; set; }

        public virtual string OrgReply { get; set; }

        public virtual DateTime? SendDate { get; set; }

        public virtual DateTime? ReplySlipReceiveDate { get; set; }

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

        public virtual string ContactPerson { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string PartNum { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual string ReplySlipPartNum { get; set; }

        public virtual string ReplySlipEnclosureNum { get; set; }
        
        public virtual string ActivityConcern { get; set; }

        public virtual string ActivityConcernDesc { get; set; }

        public virtual string FileRef { get; set; }

        public virtual string OrgProvisionNotAdopts { get; set; }

        public virtual string PromulgationReason { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string Approved { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual IList<PspMaster> PspMasters { get; set; }

        public virtual IList<FdMaster> FdMasters { get; set; }

        public override int Id
        {
            get
            {
                return OrgRefGuidePromulgationId;
            }
            set
            {
                OrgRefGuidePromulgationId = value;
            }
        }
    }
}
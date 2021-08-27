using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Models.Domain;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Organisation
{
    [Validator(typeof(OrganisationViewModelValidator))]
    public partial class OrganisationViewModel : BaseViewModel
    {
        #region Search

        public bool isFirstSearch { get; set; }

        public string PrePage { get; set; }

        public string PreRecordId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_OrganisationSearchTitle")]
        public string OrganisationSearch { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_OrganisationReference")]
        public string OrganisationReference { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "EngOrganisationName")]
        public string OrganisationName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_OrganisationName")]
        public string SearchOrganisationName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_TelephoneNo")]
        public string TelephoneNo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_OrganisationStatus")]
        public string OrganisationStatusId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_OrganisationStatus")]
        public IDictionary<string, string> OrganisationStatus { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Subvented")]
        public bool Subvented { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Subvented")]
        public string SubventedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Subvented")]
        public IDictionary<string, string> Subventeds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_NameofContactPerson")]
        public string NameofContactPerson { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_PrincipalActivities")]
        public string PrincipalActivities { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Section")]
        public string SectionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Section")]
        public IDictionary<string, string> Sections { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_RegistrationTitle")]
        public string RegistrationTitleId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_RegistrationTitle")]
        public IDictionary<string, string> RegistrationTitles { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_Registration")]
        public string Registration { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReferenceGuide")]
        public string ReferenceGuide { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReferenceGuide")]
        public string[] OrgReply { get; set; }

        public IDictionary<string, string> OrgReplys { get; set; }

        public string ReferenceGuideType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedPSPBefore")]
        public string AppliedPSPBeforeId { get; set; }
        
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedSSAFBefore")]
        public string AppliedSSAFBeforeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedPSPBefore")]
        public IDictionary<string, string> AppliedPSPBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedSSAFBefore")]
        public IDictionary<string, string> AppliedSSAFBefores { get; set; }

        public DateTime? FromPspApplicationDate { get; set; }

        public DateTime? ToPspApplicationDate { get; set; }

        public DateTime? FromSSAFApplicationDate { get; set; }

        public DateTime? ToSSAFApplicationDate { get; set; }

        public DateTime? FromPspPermitIssueDate { get; set; }

        public DateTime? ToPspPermitIssueDate { get; set; }

        public DateTime? FromSSAFPermitIssueDate { get; set; }

        public DateTime? ToSSAFPermitIssueDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_PSPIssuedBefore")]
        public string PSPIssuedBeforeId { get; set; }

        public IDictionary<string, string> PSPIssuedBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_SSAFIssuedBefore")]
        public string SSAFIssuedBeforeId { get; set; }

        public IDictionary<string, string> SSAFIssuedBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_FDIssuedBefore")]
        public string FDIssuedBeforeId { get; set; }

        public IDictionary<string, string> FDIssuedBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedFDBefore")]
        public string AppliedFDBeforeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_AppliedFDBefore")]
        public IDictionary<string, string> AppliedFDBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_FdYear")]
        public string FdYear { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_FdYear")]
        public List<string> AppliedFDBeforeFdYear { get; set; }
        
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_FdYear")]
        public List<string> FdIssuedBeforeFdYear { get; set; }

        public IDictionary<string, string> FdYears { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReceivedComplaintBefore")]
        public string ReceivedComplaintBeforeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReceivedComplaintBefore")]
        public IDictionary<string, string> ReceivedComplaintBefores { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReceivedEnquiryBefore")]
        public string ReceivedEnquiryBeforeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_ReceivedEnquiryBefore")]
        public IDictionary<string, string> ReceivedEnquiryBefores { get; set; }

        public bool withholdingIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_WithHoldInd")]
        public string WithholdingInd { get; set; }

        public IDictionary<string, string> WithholdingInds { get; set; }

        public IDictionary<string, string> FollowUpLetterTypes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Search_FlagYear")]
        public string FlagYear { get; set; }

        public IDictionary<string, string> FlagYears { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Disabled")]
        public bool OrganisationDisabled { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiName")]
        public string OrganisationChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_SimpliChiName")]
        public string OrganisationSimpliChiName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherSimpChiOrgName")]
        public string OtherSimpChiOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OldName")]
        public string OrganisationOldName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_SortingName")]
        public string EngOrgNameSorting { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherEngName")]
        public string OtherEngOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherChiName")]
        public string OtherChiOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngRegisteredAddress")]
        public string EngRegisteredAddress1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngRegisteredAddress")]
        public string EngRegisteredAddress2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngRegisteredAddress")]
        public string EngRegisteredAddress3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngRegisteredAddress")]
        public string EngRegisteredAddress4 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngRegisteredAddress")]
        public string EngRegisteredAddress5 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiRegisteredAddress")]
        public string ChiRegisteredAddress1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiRegisteredAddress")]
        public string ChiRegisteredAddress2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiRegisteredAddress")]
        public string ChiRegisteredAddress3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiRegisteredAddress")]
        public string ChiRegisteredAddress4 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiRegisteredAddress")]
        public string ChiRegisteredAddress5 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngMailingAddress")]
        public string EngMailingAddress1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngMailingAddress")]
        public string EngMailingAddress2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngMailingAddress")]
        public string EngMailingAddress3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngMailingAddress")]
        public string EngMailingAddress4 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EngMailingAddress")]
        public string EngMailingAddress5 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiMailingAddress")]
        public string ChiMailingAddress1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiMailingAddress")]
        public string ChiMailingAddress2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiMailingAddress")]
        public string ChiMailingAddress3 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiMailingAddress")]
        public string ChiMailingAddress4 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiMailingAddress")]
        public string ChiMailingAddress5 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChangeDate")]
        public DateTime OrganisationChangeDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ChiAddress")]
        public string ChiAddress { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Website")]
        public string OrganisationWebsite { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_TelNum")]
        public string TelNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_FaxNo")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantSalute")]
        public string ApplicantSaluteId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantSalute")]
        public IDictionary<string, string> ApplicantSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantFirstName")]
        public string ApplicantFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantLastName")]
        public string ApplicantLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantChiSalute")]
        public string ApplicantChiSaluteId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantChiSalute")]
        public IDictionary<string, string> ApplicantChiSalutes { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantChiSalute")]
        public string ApplicantChiFirstName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantChiSalute")]
        public string ApplicantChiLastName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantPosition")]
        public string ApplicantPosition { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ApplicantTelNum")]
        public string ApplicantTelNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_President")]
        public string President { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Secretary")]
        public string Secretary { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Treasurer")]
        public string Treasurer { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Objectives")]
        public string Objectives { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Section88")]
        public bool Section88 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Section88Date")]
        public DateTime? Section88Date { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_RegistrationType1")]
        public string RegistrationType1 { get; set; }

        public string RegistrationOtherName1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_RegistrationDate")]
        public DateTime? RegistrationDate1 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_RegistrationType2")]
        public string RegistrationType2 { get; set; }

        public string RegistrationOtherName2 { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_RegistrationDate")]
        public DateTime? RegistrationDate2 { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_BrNum")]
        //public string BrNum1 { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_BrNum")]
        //public string BrNum2 { get; set; }

        //[Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_CrNum")]
        //public string CrNum { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_AddressProof")]
        public bool AddressProofIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_AddressProofDate")]
        public DateTime? AddressProofDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_AddressProofFile")]
        public HttpPostedFileBase AddressProofAttachmentId { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_MaaConstitutionIndicator")]
        public bool MaaConstitutionIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_MaaConstitutionAttachmentId")]
        public HttpPostedFileBase MaaConstitutionAttachmentId { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Afs1YearEnd")]
        public bool QualifiedOpinionIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Afs1YearEnd")]
        public string QualifiedOpinionRemark { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherSupportDoc")]
        public bool OtherSupportDocIndicator { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherSupportDoc")]
        public string OtherSupportDocRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OverallRemark")]
        public string OverallRemark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_OtherSupportDoc")]
        public HttpPostedFileBase OtherSupportDocAttachmentId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_SendDate")]
        public DateTime? SendDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReceiveDate")]
        public DateTime? ReplySlipReceiveDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReplySlipDate")]
        public DateTime? ReplySlipDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_LanguageUsed")]
        public string LanguageUsedId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_LanguageUsed")]
        public IDictionary<string, string> LanguageUseds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReplyFrom")]
        public string ReplyFromId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReplyFrom")]
        public IDictionary<string, string> ReplyFroms { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_PromulgationReason")]
        public string PromulgationReason { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Provision")]
        public string ProvisionId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_PartNum")]
        public string PartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EnclosureNum")]
        public string EnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReplySlipPartNum")]
        public string ReplySlipPartNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_EnclosureNum")]
        public string ReplySlipEnclosureNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_Remarks")]
        public string Remarks { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReceiveDate")]
        public DateTime? ReplySlipReceiveDateStart { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_ReceiveDate")]
        public DateTime? ReplySlipReceiveDateEnd { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_SendDate")]
        public DateTime? SendDateStart { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Organisation_SendDate")]
        public DateTime? SendDateEnd { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentDocument")]
        public HttpPostedFileBase AttachmentDocument { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "AttachmentRemark")]
        public string AttachmentRemark { get; set; }

        public string AttachmentId { get; set; }

        public string OrgMasterId { get; set; }

        public byte[] RowVersion { get; set; }

        #endregion Search

        #region Reference Guide

        public string OrgRefGuidePromulgationId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReferenceGuide_ProvisionsNotBeAdopteds")]
        public string[] ProvisionsNotBeAdopted { get; set; }

        public IDictionary<string, string> ProvisionsNotBeAdopteds { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

        public string OrgRefGuideFileNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ReferenceGuide_ReferenceGuideActivityConcern")]
        public string ReferenceGuideActivityConcern { get; set; }

        public string FileRef { get; set; }

        #endregion Reference Guide

        #region searchoptions

        public IDictionary<string, string> ProcessStatus { get; set; }

        public IDictionary<string, string> ComplaintResults { get; set; }

        public IDictionary<string, string> ComplaintSources { get; set; }

        public IDictionary<string, string> ActivityConcerns { get; set; }

        public IDictionary<string, string> FdApplicationResults { get; set; }

        public IDictionary<string, string> FdGroupings { get; set; }

        public IDictionary<string, string> PSPApplicationResults { get; set; }

        public IDictionary<string, string> TWRs { get; set; }

        public IDictionary<string, string> CheckIndicator { get; set; }

        #endregion searchoptions

        public string WithholdingSection { get; set; }

        public string WithholdingBeginDate { get; set; }

        public string WithholdingEndDate { get; set; }

        public string PspRef { get; set; }

        public string PspContactPersonName { get; set; }

        public string PspContactPersonEmailAddress { get; set; }

        public string FdRef { get; set; }

        public string FdContactPersonName { get; set; }

        public string FdContactPersonEmailAddress { get; set; }

        public bool isProcessingOfficer { get; set; }
    }
}
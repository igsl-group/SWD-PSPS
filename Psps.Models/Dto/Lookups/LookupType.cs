using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Psps.Core.Helper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Psps.Models.Dto.Lookups
{
    /// <summary>
    /// Represents the lookup type enumeration
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LookupType
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_BenchmarkRFD")]
        [EnumMember(Value = "BenchmarkRFD")]
        BenchmarkRFD,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_BenchmarkTWFD")]
        [EnumMember(Value = "BenchmarkTWFD")]
        BenchmarkTWFD,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_CaseCloseReason")]
        [EnumMember(Value = "CaseCloseReason")]
        CaseCloseReason,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_CheckIndicator")]
        [EnumMember(Value = "CheckIndicator")]
        CheckIndicator,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintActivityConcern")]
        [EnumMember(Value = "ComplaintActivityConcern")]
        ComplaintActivityConcern,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintInvestigationResult")]
        [EnumMember(Value = "ComplaintInvestigationResult")]
        ComplaintInvestigationResult,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintNonComplianceNature")]
        [EnumMember(Value = "ComplaintNonComplianceNature")]
        ComplaintNonComplianceNature,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintProcessStatus")]
        [EnumMember(Value = "ComplaintProcessStatus")]
        ComplaintProcessStatus,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintRecordType")]
        [EnumMember(Value = "ComplaintRecordType")]
        ComplaintRecordType,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintResult")]
        [EnumMember(Value = "ComplaintResult")]
        ComplaintResult,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintSource")]
        [EnumMember(Value = "ComplaintSource")]
        ComplaintSource,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintWithholdingRemarks")]
        [EnumMember(Value = "ComplaintWithholdingRemarks")]
        ComplaintWithholdingRemarks,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ComplaintCollectionMethod")]
        [EnumMember(Value = "ComplaintCollectionMethod")]
        ComplaintCollectionMethod,

        // Should be Hidden
        [EnumMember(Value = "Department")]
        Department,

        // Should be Hidden
        [EnumMember(Value = "FrasDistrict")]
        FrasDistrict,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdApplicationResult")]
        [EnumMember(Value = "FdApplicationResult")]
        FdApplicationResult,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdCollectionMethod")]
        [EnumMember(Value = "FdCollectionMethod")]
        FdCollectionMethod,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdDocSubmitted")]
        [EnumMember(Value = "FdDocSubmitted")]
        FdDocSubmitted,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdEventRemark")]
        [EnumMember(Value = "FdEventRemark")]
        FdEventRemark,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdGrouping")]
        [EnumMember(Value = "FdGrouping")]
        FdGrouping,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FdLotResult")]
        [EnumMember(Value = "FdLotResult")]
        FdLotResult,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FollowUpLetterType")]
        [EnumMember(Value = "FollowUpLetterType")]
        FollowUpLetterType,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_FundUsed")]
        [EnumMember(Value = "FundUsed")]
        FundUsed,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_LanguageUsed")]
        [EnumMember(Value = "LanguageUsed")]
        LanguageUsed,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_LegalAdviceType")]
        [EnumMember(Value = "LegalAdviceType")]
        LegalAdviceType,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_NoSolicitationDate")]
        [EnumMember(Value = "NoSolicitationDate")]
        NoSolicitationDate,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_OrgRegistrationType")]
        [EnumMember(Value = "OrgRegistrationType")]
        OrgRegistrationType,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_OrgReply")]
        [EnumMember(Value = "OrgReply")]
        OrgReply,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_OrgStatus")]
        [EnumMember(Value = "OrgStatus")]
        OrgStatus,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspBroughtForward")]
        [EnumMember(Value = "PspBroughtForward")]
        PspBroughtForward,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PSPApplicationResult")]
        [EnumMember(Value = "PSPApplicationResult")]
        PSPApplicationResult,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspCollectionMethod")]
        [EnumMember(Value = "PspCollectionMethod")]
        PspCollectionMethod,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspDocSubmitted")]
        [EnumMember(Value = "PspDocSubmitted")]
        PspDocSubmitted,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspEventRemark")]
        [EnumMember(Value = "PspEventRemark")]
        PspEventRemark,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PSPNotRequireReason")]
        [EnumMember(Value = "PSPNotRequireReason")]
        PSPNotRequireReason,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PSPRejectReason")]
        [EnumMember(Value = "PSPRejectReason")]
        PSPRejectReason,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PSPRequiredIndicator")]
        [EnumMember(Value = "PSPRequiredIndicator")]
        PSPRequiredIndicator,

        // Should be Hidden
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspSpecialRemark")]
        [EnumMember(Value = "PspSpecialRemark")]
        PspSpecialRemark,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ReferenceGuideProvision")]
        [EnumMember(Value = "ReferenceGuideProvision")]
        ReferenceGuideProvision,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_ReferenceGuideActivityConcern")]
        [EnumMember(Value = "ReferenceGuideActivityConcern")]
        ReferenceGuideActivityConcern,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_Salute")]
        [EnumMember(Value = "Salute")]
        Salute,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_SuggestionActivityConcern")]
        [EnumMember(Value = "SuggestionActivityConcern")]
        SuggestionActivityConcern,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_SuggestionNature")]
        [EnumMember(Value = "SuggestionNature")]
        SuggestionNature,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_SuggestionSource")]
        [EnumMember(Value = "SuggestionSource")]
        SuggestionSource,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_TWR")]
        [EnumMember(Value = "TWR")]
        TWR,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_TWRDistrict")]
        [EnumMember(Value = "TWRDistrict")]
        TWRDistrict,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_PspRegion")]
        [EnumMember(Value = "PspRegion")]
        PspRegion,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_VenueType")]
        [EnumMember(Value = "VenueType")]
        VenueType,

        // Should be Hidden
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_YesNo")]
        [EnumMember(Value = "YesNo")]
        YesNo,

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "LookupType_SsafBroughtForward")]
        [EnumMember(Value = "SsafBroughtForward")]
        SsafBroughtForward,
    }
}
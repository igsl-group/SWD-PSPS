using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class Activity
    {
        public Activity()
        {
            Schedule = new List<Schedule>();
            Remarks = new List<string>();
        }

        /// <summary>
        /// Unique ID representing the record
        /// </summary>
        public string CharityEventId { get; set; }

        /// <summary>
        /// Unique ID representing the activity type
        /// </summary>
        public long ActivityId { get; set; }

        /// <summary>
        /// Type of activity (English)
        /// </summary>
        public string ActivityNameEnglish { get; set; }

        /// <summary>
        /// Type of activity (Traditional Chinese)
        /// </summary>
        public string ActivityNameTChinese { get; set; }

        /// <summary>
        /// Type of activity (Simplified Chinese)
        /// </summary>
        public string ActivityNameSChinese { get; set; }

        /// <summary>
        /// Unique ID representing the organisation
        /// </summary>
        public long OrganisationId { get; set; }

        /// <summary>
        /// Name of the organisation (English)
        /// </summary>
        public string OrganisationNameEnglish { get; set; }

        /// <summary>
        /// Name of the organisation (Traditional Chinese)
        /// </summary>
        public string OrganisationNameTChinese { get; set; }

        /// <summary>
        /// Name of the organisation (Simplified Chinese)
        /// </summary>
        public string OrganisationNameSChinese { get; set; }

        /// <summary>
        /// Unique ID representing the district
        /// </summary>
        public long DistrictId { get; set; }

        /// <summary>
        /// District (English)
        /// </summary>
        public string DistrictNameEnglish { get; set; }

        /// <summary>
        /// District (Traditional Chinese)
        /// </summary>
        public string DistrictNameTChinese { get; set; }

        /// <summary>
        /// District in Simplified Chinese
        /// </summary>
        public string DistrictNameSChinese { get; set; }

        /// <summary>
        /// Location in English
        /// </summary>
        public string LocationNameEnglish { get; set; }

        /// <summary>
        /// Location in Traditional Chinese
        /// </summary>
        public string LocationNameTChinese { get; set; }

        /// <summary>
        /// Location in Simplified Chinese
        /// </summary>
        public string LocationNameSChinese { get; set; }

        /// <summary>
        /// Permit type in English
        /// </summary>
        public string LicencePermitTypeEnglish { get; set; }

        /// <summary>
        /// Permit type in Traditional Chinese
        /// </summary>
        public string LicencePermitTypeTChinese { get; set; }

        /// <summary>
        /// Permit type in Simplified Chinese
        /// </summary>
        public string LicencePermitTypeSChinese { get; set; }

        /// <summary>
        /// Enquiry contact information
        /// </summary>
        public string EnquiryContact { get; set; }

        /// <summary>
        /// Internal supplementary information in English
        /// </summary>
        public string InternalSupplementaryInfoEnglish { get; set; }

        /// <summary>
        /// Internal supplementary information in Traditional Chinese
        /// </summary>
        public string InternalSupplementaryInfoTChinese { get; set; }

        /// <summary>
        /// Internal supplementary information in Simplified Chinese
        /// </summary>
        public string InternalSupplementaryInfoSChinese { get; set; }

        /// <summary>
        /// External supplementary information in English
        /// </summary>
        public string ExternalSupplementaryInfoEnglish { get; set; }

        /// <summary>
        /// External supplementary information in Traditional Chinese
        /// </summary>
        public string ExternalSupplementaryInfoTChinese { get; set; }

        /// <summary>
        /// External supplementary information in Simplified Chinese
        /// </summary>
        public string ExternalSupplementaryInfoSChinese { get; set; }

        /// <summary>
        /// Permit number
        /// </summary>
        public string PermitNumber { get; set; }

        /// <summary>
        /// Workflow status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Flag day conflict warning message
        /// </summary>
        public string FlagWarn { get; set; }

        /// <summary>
        /// Remarks, no legend
        /// </summary>
        public List<string> Remarks { get; set; }

        /// <summary>
        /// Event schedule
        /// </summary>
        /// <remarks></remarks>
        public List<Schedule> Schedule { get; set; }
    }
}
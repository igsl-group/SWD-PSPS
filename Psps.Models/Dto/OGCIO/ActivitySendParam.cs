using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class ActivitySendParam
    {
        public ActivitySendParam()
        {
            Schedule = new List<Schedule>();
            GovHKRemarkList = new List<string>();
        }

        /// <summary>
        /// [Required] Unique ID representing the record
        /// </summary>
        /// <remarks>You may have some additional information about this class.</remarks>
        public string CharityEventId { get; set; }

        /// <summary>
        /// [Required] Unique ID representing the district
        /// </summary>
        /// <remarks>Numeric</remarks>
        public long DistrictId { get; set; }

        /// <summary>
        /// [Required] Unique ID representing the activity type
        /// </summary>
        /// <remarks>Numeric</remarks>
        public long ActivityId { get; set; }

        /// <summary>
        /// [Required] Unique ID representing the organisation
        /// </summary>
        /// <remarks>Numeric</remarks>
        public long OrganisationId { get; set; }

        /// <summary>
        /// [Required] Charitable Activity. 1 if yes and 0 if not
        /// </summary>
        /// <remarks>0 or 1</remarks>
        public int Charitable { get; set; }

        /// <summary>
        /// Location in English
        /// </summary>
        /// <remarks>Alpha numeric value up to 500 characters</remarks>
        public string LocationNameEnglish { get; set; }

        /// <summary>
        /// Location in Traditional Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 500 characters</remarks>
        public string LocationNameTChinese { get; set; }

        /// <summary>
        /// Location in Simplified Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 500 characters</remarks>
        public string LocationNameSChinese { get; set; }

        /// <summary>
        /// Permit number
        /// </summary>
        /// <remarks>Alpha numeric value up to 100 characters</remarks>
        public string PermitNumber { get; set; }

        /// <summary>
        /// Enquiry contact information
        /// </summary>
        /// <remarks>Alpha numeric value up to 255 characters</remarks>
        public string EnquiryContact { get; set; }

        /// <summary>
        /// Internal supplementary information in English
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string InternalSuppInfoEnglish { get; set; }

        /// <summary>
        /// Internal supplementary information in Traditional Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string InternalSuppInfoTChinese { get; set; }

        /// <summary>
        /// Internal supplementary information in Simplified Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string InternalSuppInfoSChinese { get; set; }

        /// <summary>
        /// User who input this activity into the system
        /// </summary>
        /// <remarks>Alpha numeric value up to 50 characters</remarks>
        public string InputBy { get; set; }

        /// <summary>
        /// Approver who approved this activity
        /// </summary>
        /// <remarks>Alpha numeric value up to 255 characters</remarks>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        /// <remarks>yyyy-MM-dd format</remarks>
        public string ApprovedOn { get; set; }

        /// <summary>
        /// External supplementary information in English
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string ExternalSuppInfoEnglish { get; set; }

        /// <summary>
        /// External supplementary information in Traditional Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string ExternalSuppInfoTChinese { get; set; }

        /// <summary>
        /// External supplementary information in Simplified Chinese
        /// </summary>
        /// <remarks>Alpha numeric value up to 1000 characters</remarks>
        public string ExternalSuppInfoSChinese { get; set; }

        /// <summary>
        /// A list of remark, no legend
        /// </summary>
        /// <remarks>A list of alpha numeric value up to 50 characters each</remarks>
        public List<string> GovHKRemarkList { get; set; }

        /// <summary>
        /// [Required] Event schedule
        /// </summary>
        /// <remarks></remarks>
        public List<Schedule> Schedule { get; set; }
    }
}
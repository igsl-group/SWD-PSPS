using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psps.Models.Dto.Complaint
{
    public partial class ComplaintMasterSearchDto : BaseDto
    {
        public int ComplaintMasterId { get; set; }

        public string OrgRef { get; set; }

        public string EngChiOrgName { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public bool DisableIndicator { get; set; }

        public bool SubventedIndicator { get; set; }

        public string RegistrationType1 { get; set; }

        public string RegistrationType2 { get; set; }

        public string RegistrationOtherName1 { get; set; }

        public string RegistrationOtherName2 { get; set; }

        public string ComplaintRef { get; set; }

        public string ComplaintRecordType { get; set; }

        public string ComplaintSource { get; set; }

        public string ActivityConcern { get; set; }

        public string OtherActivityConcern { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public DateTime? FirstComplaintDate { get; set; }

        public DateTime? ReplyDueDate { get; set; }

        public string SwdUnit { get; set; }

        public DateTime? LfpsReceiveDate { get; set; }

        public string ComplainantName { get; set; }

        public string DcLcContent { get; set; }

        public string NonComplianceNature { get; set; }

        public string OtherNonComplianceNature { get; set; }

        public string ComplaintEnclosureNum { get; set; }

        public string ProcessStatus { get; set; }

        public string ActionFileEnclosureNum { get; set; }

        public string FundRaisingLocation { get; set; }

        public string ComplaintResult { get; set; }

        public string WithholdingRemark { get; set; }
        //cal
        public int TelRecordNum { get; set; }

        public int FollowUpActionRecordNum { get; set; }

        public int PoliceCaseNum { get; set; }

        public int OtherDepartmentEnquiryNum { get; set; }

        public string ComplaintResultRemark { get; set; }

        public string OtherWithholdingRemark { get; set; }



    }
}
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psps.Models.Dto.Complaint
{
    public partial class ComplaintMasterDto : BaseEntity<int>
    {
        public virtual int ComplaintMasterId { get; set; }

        public virtual PspApprovalHistory PspApprovalHistory { get; set; }

        public virtual FdApprovalHistory FdApprovalHistory { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual ComplaintMaster RelatedComplaintMaster { get; set; }

        public virtual ComplaintTelRecord ComplaintTelRecord { get; set; }

        public virtual string ComplaintRef { get; set; }

        public virtual string ComplaintRecordType { get; set; }

        public virtual string ComplaintSource { get; set; }

        public virtual string ComplaintSourceRemark { get; set; }

        public virtual string ActivityConcern { get; set; }

        public virtual string OtherActivityConcern { get; set; }

        public virtual DateTime ComplaintDate { get; set; }

        public virtual DateTime FirstComplaintDate { get; set; }

        public virtual DateTime ReplyDueDate { get; set; }

        public virtual string SwdUnit { get; set; }

        public virtual DateTime LfpsReceiveDate { get; set; }

        public virtual string ConcernedOrgName { get; set; }

        public virtual string ComplainantName { get; set; }

        public virtual bool GovernmentHotlineIndicator { get; set; }

        public virtual string DcLcContent { get; set; }

        public virtual string NonComplianceNature1 { get; set; }

        public virtual string OtherNonComplianceNature1 { get; set; }

        public virtual string NonComplianceNature2 { get; set; }

        public virtual string OtherNonComplianceNature2 { get; set; }

        public virtual string NonComplianceNature3 { get; set; }

        public virtual string OtherNonComplianceNature3 { get; set; }

        public virtual string ComplaintEnclosureNum { get; set; }

        public virtual string ProcessStatus { get; set; }

        public virtual string ActionFileEnclosureNum { get; set; }

        public virtual string ComplaintRemarks { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintMasterId;
            }
            set
            {
                ComplaintMasterId = value;
            }
        }
    }
}
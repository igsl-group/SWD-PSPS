using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintTelRecord : BaseAuditEntity<int>
    {
        public virtual int ComplaintTelRecordId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string TelComplaintRef { get; set; }

        public virtual DateTime ComplaintDate { get; set; }

        public virtual string ComplaintTime { get; set; }

        public virtual string ComplainantName { get; set; }

        public virtual string ComplainantTelNum { get; set; }

        public virtual string OrgName { get; set; }

        public virtual PspApprovalHistory PspApprovalHistory { get; set; }

        public virtual FdEvent FdEvent { get; set; }

        public virtual string ComplaintContentRemark { get; set; }

        public virtual string ComplaintContentRemarkHtml { get; set; }

        public virtual string OfficerName { get; set; }

        public virtual string OfficerPost { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintTelRecordId;
            }
            set
            {
                ComplaintTelRecordId = value;
            }
        }
    }
}

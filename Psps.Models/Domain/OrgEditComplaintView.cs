using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgEditComplaintView : BaseEntity<int>
    {
        public virtual int ComplaintMasterId { get; set; }

        public virtual int OrgId { get; set; }

        public virtual string ComplaintRef { get; set; }

        public virtual string ComplaintSource { get; set; }

        public virtual string ActivityConcern { get; set; }

        public virtual DateTime? ComplaintDate { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual string ComplaintRemarks { get; set; }

        public virtual DateTime? FollowUpLetterIssueDate { get; set; }

        public virtual string FollowUpLetterType { get; set; }

        public virtual string LetterIssuedNum { get; set; }
        
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
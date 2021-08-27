using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Complaint
{
    public partial class ComplaintFdMasterDto : BaseEntity<int>
    {
        public int ComplaintMasterId { get; set; }

        public int OrgId { get; set; }

        public int FdMasterId { get; set; }

        public string ComplaintRef { get; set; }

        public string ComplaintSource { get; set; }

        public string ActivityConcern { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public string PermitNum { get; set; }

        public string ComplaintRemarks { get; set; }

        public DateTime? FollowUpLetterIssueDate { get; set; }

        public string FollowUpLetterType { get; set; }

        public string LetterIssuedNum { get; set; }

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
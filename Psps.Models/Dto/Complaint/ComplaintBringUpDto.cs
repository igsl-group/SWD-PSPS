using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Complaint
{
    public partial class ComplaintBringUpDto : BaseEntity<string>
    {
        public string OrgRef { get; set; }

        public string EngChiOrgName { get; set; }

        public string ComplaintRef { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public string ComplaintSource { get; set; }

        public string PermitConcern { get; set; }

        public string ActionFileEnclosureNum { get; set; }

        public byte[] RowVersion { get; set; }

        public override string Id
        {
            get
            {
                return OrgRef;
            }
            set
            {
                OrgRef = value;
            }
        }
    }
}
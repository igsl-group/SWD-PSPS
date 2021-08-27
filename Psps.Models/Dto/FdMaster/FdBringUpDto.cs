using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdMaster
{
    public partial class FdBringUpDto : BaseDto
    {
        //public int? DisasterStatisticsId { get; set; }

        public string FdRef { get; set; }

        public string FdYear { get; set; }

        public DateTime? FlagDayDate { get; set; }

        public string FlagDayType { get; set; }

        public DateTime? ApplicationReceiveDate { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApplicationResult { get; set; }

        public string PermitNum { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public byte[] RowVersion { get; set; }

        public string Id
        {
            get
            {
                return FdRef;
            }
            set
            {
                FdRef = value;
            }
        }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.PspMaster
{
    public partial class PspMasterDto : BaseEntity<string>
    {
        //public int? DisasterStatisticsId { get; set; }

        public string PspRef { get; set; }

        public string EngOrgName { get; set; }

        public DateTime ApplicationReceiveDate { get; set; }

        public DateTime PspEventDate { get; set; }

        public string ProcessingOfficerPost { get; set; }

        public string SpecialRemark { get; set; }

        public byte[] RowVersion { get; set; }

        public override string Id
        {
            get
            {
                return PspRef;
            }
            set
            {
                PspRef = value;
            }
        }
    }
}
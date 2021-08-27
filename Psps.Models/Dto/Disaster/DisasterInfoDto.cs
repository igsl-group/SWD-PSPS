using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Disaster
{
    public partial class DisasterInfoDto : BaseEntity<int>
    {
        //public int? DisasterStatisticsId { get; set; }

        public int DisasterMasterId { get; set; }

        public string DisasterName { get; set; }

        public string ChiDisasterName { get; set; }

        public DateTime? DisasterDate { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal PspApplicationProcedurePublicCount { get; set; }

        public decimal PspApplicationProcedureOtherCount { get; set; }

        public decimal PspScopePublicCount { get; set; }

        public decimal PspScopeOtherCount { get; set; }

        public decimal PspApplicationStatusPublicCount { get; set; }

        public decimal PspApplicationStatusOthersCount { get; set; }

        public decimal PspPermitConditionCompliancePublicCount { get; set; }

        public decimal PspPermitConditionComplianceOtherCount { get; set; }

        public decimal OtherEnquiryPublicCount { get; set; }

        public decimal OtherEnquiryOtherCount { get; set; }

        public decimal SubTotal { get; set; }

        public byte[] RowVersion { get; set; }

        public override int Id
        {
            get
            {
                return DisasterMasterId;
            }
            set
            {
                DisasterMasterId = value;
            }
        }
    }
}
using Psps.Core.Models;
using System;

namespace Psps.Models.Domain
{
    public partial class DisasterStatistics : BaseAuditEntity<int>
    {
        public virtual int DisasterStatisticsId { get; set; }

        public virtual DisasterMaster DisasterMaster { get; set; }

        public virtual string RecordPostId { get; set; }

        public virtual DateTime RecordDate { get; set; }

        public virtual decimal? PspApplicationProcedurePublicCount { get; set; }

        public virtual decimal? PspApplicationProcedureOtherCount { get; set; }

        public virtual decimal? PspScopePublicCount { get; set; }

        public virtual decimal? PspScopeOtherCount { get; set; }

        public virtual decimal? PspApplicationStatusPublicCount { get; set; }

        public virtual decimal? PspApplicationStatusOthersCount { get; set; }

        public virtual decimal? PspPermitConditionCompliancePublicCount { get; set; }

        public virtual decimal? PspPermitConditionComplianceOtherCount { get; set; }

        public virtual decimal? OtherEnquiryPublicCount { get; set; }

        public virtual decimal? OtherEnquiryOtherCount { get; set; }

        public override int Id
        {
            get
            {
                return DisasterStatisticsId;
            }
            set
            {
                DisasterStatisticsId = value;
            }
        }
    }
}
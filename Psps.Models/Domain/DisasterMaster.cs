using Psps.Core.Models;
using System;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class DisasterMaster : BaseAuditEntity<int>
    {
        public DisasterMaster()
        {
            DisasterStatistics = new List<DisasterStatistics>();
        }

        public virtual int DisasterMasterId { get; set; }

        public virtual string DisasterName { get; set; }

        public virtual string ChiDisasterName { get; set; }

        public virtual DateTime? DisasterDate { get; set; }

        public virtual DateTime BeginDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual IList<DisasterStatistics> DisasterStatistics { get; set; }

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
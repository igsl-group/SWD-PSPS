using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Domain
{
    public partial class PspEventsToProforma : BaseEntity<string>
    {
        public virtual PspMaster PspMaster { get; set; }

        public virtual string EventStartYear { get; set; }

        public virtual string EventStartMonth { get; set; }

        public virtual string EventDays { get; set; }

        public virtual string EventStartTime { get; set; }

        public virtual string EventEndTime { get; set; }

        public virtual string Location { get; set; }

        public virtual string ChiLocation { get; set; }

        public virtual string District { get; set; }

        public virtual string CollectionMethod { get; set; }

        public override string Id
        {
            get
            {
                return PspMaster.PspMasterId.ToString() + EventStartYear + EventStartMonth + EventDays + EventStartTime + EventEndTime + District + Location + ChiLocation;
            }

            set
            {
                Id = value;
            }
        }
    }
}
using Psps.Core.Models;
using System;

namespace Psps.Models.Domain
{
    public partial class ActivityLog : BaseEntity<int>
    {
        public virtual int LogId { get; set; }

        public virtual string RecordKey { get; set; }

        public virtual string Activity { get; set; }

        public virtual string Action { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime ActionedOn { get; set; }

        public virtual User User { get; set; }

        public virtual Post Post { get; set; }

        public override int Id
        {
            get
            {
                return LogId;
            }
            set
            {
                LogId = value;
            }
        }
    }
}
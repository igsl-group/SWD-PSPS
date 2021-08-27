using Psps.Core.Models;
using System;

namespace Psps.Models.Domain
{
    public partial class Acting : BaseAuditEntity<int>
    {
        public virtual int ActingId { get; set; }

        public virtual User User { get; set; }

        public virtual Post Post { get; set; }

        public virtual Post PostToBeActed { get; set; }

        public virtual DateTime EffectiveFrom { get; set; }

        public virtual DateTime EffectiveTo { get; set; }

        public override int Id
        {
            get
            {
                return ActingId;
            }
            set
            {
                ActingId = value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.Models
{
    [Serializable]
    public abstract class BaseAuditEntity<TPk> : BaseEntity<TPk>, IEntity<TPk>, IAuditable
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string CreatedById { get; set; }

        public virtual string CreatedByPost { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual string UpdatedById { get; set; }

        public virtual string UpdatedByPost { get; set; }

        public virtual DateTime? UpdatedOn { get; set; }

        public virtual byte[] RowVersion { get; set; }
    }
}
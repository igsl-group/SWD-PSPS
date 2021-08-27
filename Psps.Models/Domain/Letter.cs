using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class Letter : BaseAuditEntity<int>
    {
        public virtual int LetterId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Path { get; set; }

        public virtual string Version { get; set; }

        public virtual bool IsActive { get; set; }

        public override int Id
        {
            get
            {
                return LetterId;
            }
            set
            {
                LetterId = value;
            }
        }
    }
}
using NHibernate.Envers;
using System;
using System.Collections.Generic;

namespace Lfpis.Models.Domain
{
    public class RevChange
    {
        public virtual int RevChangeId { get; set; }

        public virtual RevInfo RevInfo { get; set; }

        public virtual RevisionType RevisionType { get; set; }

        public virtual string EntityId { get; set; }

        public virtual string EntityName { get; set; }
    }
}
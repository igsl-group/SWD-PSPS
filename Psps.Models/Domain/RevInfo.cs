using NHibernate.Envers;
using System;
using System.Collections.Generic;

namespace Lfpis.Models.Domain
{
    public class RevInfo
    {
        public RevInfo()
        {
            RevisionChanges = new List<RevChange>();
        }

        public virtual int RevInfoId { get; set; }

        public virtual DateTime RevisionedOn { get; set; }

        public virtual string RevisionedById { get; set; }

        public virtual string RevisionedByPost { get; set; }

        public virtual IList<RevChange> RevisionChanges { get; set; }

        public override bool Equals(object obj)
        {
            var casted = obj as RevInfo;
            if (casted == null)
                return false;
            return (RevInfoId == casted.RevInfoId &&
                    RevisionChanges.Equals(casted.RevisionChanges) &&
                    RevisionedOn.Equals(casted.RevisionedOn) &&
                    RevisionedById.Equals(casted.RevisionedById) &&
                    RevisionedByPost.Equals(casted.RevisionedByPost)
                    );
        }

        public override int GetHashCode()
        {
            return RevInfoId.GetHashCode() ^ RevisionChanges.GetHashCode() ^ RevisionedOn.GetHashCode() ^ RevisionedById.GetHashCode() ^ RevisionedByPost.GetHashCode();
        }
    }
}
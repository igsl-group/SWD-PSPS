using Psps.Core.Models;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class Rank : BaseAuditEntity<string>
    {
        public Rank()
        {
            Posts = new List<Post>();
        }

        public virtual string RankId { get; set; }

        public virtual int RankLevel { get; set; }

        public virtual IList<Post> Posts { get; set; }

        public override string Id
        {
            get
            {
                return RankId;
            }
            set
            {
                RankId = value;
            }
        }
    }
}
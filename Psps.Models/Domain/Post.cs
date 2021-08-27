using Psps.Core.Models;
using System.Collections.Generic;

namespace Psps.Models.Domain
{
    public partial class Post : BaseAuditEntity<string>
    {
        public Post()
        {
            Supervisees = new List<Post>();
            Roles = new List<Role>();
            ActedBy = new List<Acting>();
            ActedOn = new List<Acting>();
            PostsInRole = new List<PostsInRoles>();
        }

        public virtual string PostId { get; set; }

        public virtual Rank Rank { get; set; }

        public virtual User Owner { get; set; }

        public virtual Post Supervisor { get; set; }

        public virtual IList<Post> Supervisees { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public virtual IList<Acting> ActedBy { get; set; }

        public virtual IList<Acting> ActedOn { get; set; }

        public virtual IList<PostsInRoles> PostsInRole { get; set; }

        public override string Id
        {
            get
            {
                return PostId;
            }
            set
            {
                PostId = value;
            }
        }
    }
}
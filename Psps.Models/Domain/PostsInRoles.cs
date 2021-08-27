using Psps.Core.Models;

namespace Psps.Models.Domain
{
    public partial class PostsInRoles : BaseAuditEntity<int>
    {
        public PostsInRoles()
        {
        }

        public virtual int PostsInRolesId { get; set; }

        public virtual string RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual string PostId { get; set; }

        public virtual Post Post { get; set; }

        public override int Id
        {
            get
            {
                return PostsInRolesId;
            }
            set
            {
                PostsInRolesId = value;
            }
        }
    }
}
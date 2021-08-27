using Psps.Core.Models;
using System;

namespace Psps.Models.Domain
{
    public partial class User : BaseAuditEntity<string>
    {
        public User()
        {
        }

        public virtual string UserId { get; set; }

        public virtual string Password { get; set; }

        public virtual string PrevPassword1 { get; set; }

        public virtual string PrevPassword2 { get; set; }

        public virtual string PrevPassword3 { get; set; }

        public virtual string PrevPassword4 { get; set; }

        public virtual DateTime? PasswordChangedDate { get; set; }

        public virtual string EngUserName { get; set; }

        public virtual string ChiUserName { get; set; }

        public virtual string TelephoneNumber { get; set; }

        public virtual string Email { get; set; }

        public virtual bool IsSystemAdministrator { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual Post Post { get; set; }

        public override string Id
        {
            get
            {
                return UserId;
            }
            set
            {
                UserId = value;
            }
        }
    }
}
using System;

namespace Psps.Core.Models
{
    public interface IUserInfo
    {
        string Name { get; set; }

        string UserId { get; set; }

        string PostId { get; set; }

        DateTime? PasswordChangedDate { get; set; }

        string OriginalPostIdIfActed { get; set; }

        bool IsSysAdmin { get; set; }
    }
}
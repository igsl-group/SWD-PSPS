using System.Security.Principal;

namespace Psps.Core.Models
{
    public interface IPspsUser : IIdentity
    {
        new string Name { get; set; }

        string UserId { get; }

        string PostId { get; set; }

        string OriginalPostIdIfActed { get; set; }

        bool IsSysAdmin { get; set; }

        IUserInfo GetUserInfo();
    }
}
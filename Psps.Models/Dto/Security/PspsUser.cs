using Psps.Core.Helper;
using Psps.Core.Models;
using System;
using System.Security.Principal;

namespace Psps.Models.Dto.Security
{
    [Serializable]
    public class PspsUser : IIdentity, IPspsUser
    {
        public PspsUser()
        {
        }

        public PspsUser(string name, string userId, string postId, string originalPostIdIfActed, bool isSysAdmin)
        {
            this.Name = name;
            this.UserId = userId;
            this.PostId = postId;
            this.OriginalPostIdIfActed = originalPostIdIfActed;
            this.IsSysAdmin = IsSysAdmin;
        }

        public PspsUser(string name, IUserInfo userInfo)
            : this(name, userInfo.UserId, userInfo.PostId, userInfo.OriginalPostIdIfActed, userInfo.IsSysAdmin)
        {
            Ensure.Argument.NotNull(userInfo, "userInfo");
        }

        public PspsUser(string name, string ticketUserData)
            : this(name, UserInfo.FromString(ticketUserData))
        {
            Ensure.Argument.NotNull(ticketUserData, "ticketUserData");
        }

        public string Name { get; set; }

        public string UserId { get; private set; }

        public string PostId { get; set; }

        public string OriginalPostIdIfActed { get; set; }

        public string AuthenticationType { get { return "PspsForms"; } }

        public bool IsAuthenticated { get { return true; } }

        public bool IsSysAdmin { get; set; }

        public IUserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Name = this.Name,
                UserId = this.UserId,
                PostId = this.PostId,
                OriginalPostIdIfActed = this.OriginalPostIdIfActed,
                IsSysAdmin = this.IsSysAdmin
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Accounts
{
    public class UserInfoDto : BaseDto
    {
        public string UserId { get; set; }

        public string PostId { get; set; }

        public string EngUserName { get; set; }

        public string ChiUserName { get; set; }

        public string TelephoneNumber { get; set; }

        public string Email { get; set; }

        public bool IsSystemAdministrator { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
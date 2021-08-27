using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Security
{
    public class RoleInfoDto :BaseDto
    {
        public string RoleId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByPost { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByPost { get; set; }
        public byte[] RowVersion { get; set; }
        
    }
}

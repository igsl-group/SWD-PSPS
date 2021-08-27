using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Security
{
    public class FunctionsInRolesDto
    {
        public string FunctionId { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public int FunctionsInRolesId { get; set; }

        public string RoleId { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
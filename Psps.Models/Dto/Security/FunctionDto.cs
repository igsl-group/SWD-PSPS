using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Security
{
    public class FunctionDto
    {
        public string FunctionId { get; set; }

        public string Module { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public int FunctionsInRolesId { get; set; }

        public string RoleId { get; set; }
    }
}
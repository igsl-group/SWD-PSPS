using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Organisation
{
    public partial class WithHoldingDate : BaseDto
    {
        /// <summary>
        /// grid properties
        /// </summary>

        public string WithholdingBeginDate { get; set; }

        public string WithholdingEndDate { get; set; }
    }
}
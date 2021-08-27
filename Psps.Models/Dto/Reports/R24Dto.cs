using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R24Dto
    {
        public DateTime? FlagDay { get; set; }

        public String Region { get; set; }

        public String EngOrgNameSorting { get; set; }

        public string ChiOrgName { get; set; }

        public decimal NetProceed { get; set; }

        public Boolean ApplyPledgingMechanismIndicator { get; set; }

        public String Benchmark { get; set; }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R3BreakdownDto : BaseReportDto
    {
        public int Yr { get; set; }

        public string Mon { get; set; }

        public int Level { get; set; }

        public int Telephone { get; set; }

        public int Written { get; set; }

        public int From1823 { get; set; }

        public int Mass { get; set; }

        public int DC { get; set; }

        public int LegC { get; set; }

        public int Other { get; set; }

        public int FromPolice { get; set; }

        public int UnClass { get; set; }

        public int Police { get; set; }
    }
}
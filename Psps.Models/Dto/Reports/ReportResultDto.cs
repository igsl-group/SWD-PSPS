using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class ReportResultDto
    {
        public string FileName { get; set; }

        public Stream ReportStream { get; set; }
    }
}
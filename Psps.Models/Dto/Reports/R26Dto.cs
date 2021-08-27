using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R26Dto : BaseReportDto
    {
        public string AdviceCode { get; set; }

        public string VenueCode { get; set; }

        public string LegalAdviceCodeType { get; set; }

        public string LegalAdviceCodeSubType { get; set; }

        public string LegalAdviceCodeCode { get; set; }

        public string LegalAdviceDescription { get; set; }

        public string EnclosureNum { get; set; }

        public string PartNum { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string PspRequire { get; set; }

        public string Remarks { get; set; }
    }
}
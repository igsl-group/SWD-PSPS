using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.LegalAdvice
{
    public class LegalAdviceMasterDto
    {
        public string LegalAdviceAndVenueType { get; set; }
        public string LegalAdviceCode { get; set; }
        public string LegalAdviceDescription { get; set; }
        public string PartNum { get; set; }
        public string EnclosureNum { get; set; }
        public string EffectiveDate { get; set; }
        public string RequirePspIndicator { get; set; }
        public string Remarks { get; set; }
    }
}

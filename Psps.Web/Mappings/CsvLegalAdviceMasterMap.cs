using CsvHelper.Configuration;
using Psps.Models.Dto.LegalAdvice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Mappings
{
    public class CsvLegalAdviceMasterMap : CsvClassMap<LegalAdviceMasterDto>
    {
        public CsvLegalAdviceMasterMap()
        {
            Map(m => m.LegalAdviceAndVenueType).Name("Legal Advice Type");
            Map(m => m.LegalAdviceCode).Name("Legal Advice Code");
            Map(m => m.LegalAdviceDescription).Name("Legal Advice Description");
            Map(m => m.PartNum).Name("Part No.");
            Map(m => m.EnclosureNum).Name("Encl. No.");
            Map(m => m.EffectiveDate).Name("Date");
            Map(m => m.RequirePspIndicator).Name("PSP Required");
            Map(m => m.Remarks).Name("Remarks");
            
        }
    }
}
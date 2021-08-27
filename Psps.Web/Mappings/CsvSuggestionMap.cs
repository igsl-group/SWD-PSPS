using CsvHelper.Configuration;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Mappings
{
    public class CsvSuggestionMap : CsvClassMap<SuggestionMaster>
    {
        public CsvSuggestionMap()
        {
            Map(m => m.SuggestionMasterId).Name("SuggestionMasterId");
            Map(m => m.SuggestionRefNum).Name("SuggestionRefNum");
            Map(m => m.SuggestionSource).Name("SuggestionSource");
            Map(m => m.SuggestionSourceOther).Name("SuggestionSourceOther");
            Map(m => m.SuggestionActivityConcern).Name("SuggestionActivityConcern");
            Map(m => m.SuggestionActivityConcernOther).Name("SuggestionActivityConcernOther");
            Map(m => m.SuggestionNature).Name("SuggestionNature");
            Map(m => m.SuggestionDate).Name("SuggestionDate");
            Map(m => m.SuggestionSenderName).Name("SuggestionSenderName");
            Map(m => m.SuggestionDescription).Name("SuggestionDescription");
            Map(m => m.PartNum).Name("PartNum");
            Map(m => m.EnclosureNum).Name("EnclosureNum");
            Map(m => m.AcknowledgementSentDate).Name("AcknowledgementSentDate");
            Map(m => m.Remark).Name("Remark");
        
        }
    }
}
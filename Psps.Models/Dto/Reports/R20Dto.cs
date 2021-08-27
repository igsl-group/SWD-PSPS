using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public partial class R20Dto : BaseDto
    {
        public int SuggestionMasterId { get; set; }

        public string SuggestionNature { get; set; }

        //public DateTime? SuggestionDate { get; set; }

        public string StrSuggestionDate { get; set; }

        public string SuggestionSenderName { get; set; }

        public string SuggestionDescription { get; set; }

        public string SuggestionRefNum { get; set; }

        //public DateTime? AcknowledgementSentDate { get; set; }

        public string StrAcknowledgementSentDate { get; set; }
    }
}
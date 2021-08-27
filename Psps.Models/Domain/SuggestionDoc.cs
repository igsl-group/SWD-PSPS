using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class SuggestionDoc : BaseAuditEntity<int>
    {
        public virtual int SuggestionDocId { get; set; }

        public virtual string DocNum { get; set; }

        public virtual string DocName { get; set; }

        public virtual string VersionNum { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual bool DocStatus { get; set; }

        public override int Id
        {
            get
            {
                return SuggestionDocId;
            }
            set
            {
                SuggestionDocId = value;
            }
        }
    }
}
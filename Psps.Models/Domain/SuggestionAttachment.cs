using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class SuggestionAttachment : BaseAuditEntity<int>
    {
        public virtual int SuggestionAttachmentId { get; set; }

        public virtual SuggestionMaster SuggestionMaster { get; set; }

        public virtual string SuggestionMasterId { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FileDescription { get; set; }

        public override int Id
        {
            get
            {
                return SuggestionAttachmentId;
            }
            set
            {
                SuggestionAttachmentId = value;
            }
        }
    }
}
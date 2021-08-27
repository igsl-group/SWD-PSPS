using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class SuggestionMaster : BaseAuditEntity<int>
    {
        public SuggestionMaster()
        {
            SuggestionAttachment = new List<SuggestionAttachment>();
        }

        public virtual int SuggestionMasterId { get; set; }

        public virtual string SuggestionRefNum { get; set; }

        public virtual string SuggestionSource { get; set; }

        public virtual string SuggestionSourceOther { get; set; }

        public virtual string SuggestionActivityConcern { get; set; }

        public virtual string SuggestionActivityConcernOther { get; set; }

        public virtual string SuggestionNature { get; set; }

        public virtual DateTime? SuggestionDate { get; set; }

        public virtual string SuggestionSenderName { get; set; }

        public virtual string SuggestionDescription { get; set; }

        public virtual string PartNum { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual DateTime? AcknowledgementSentDate { get; set; }

        public virtual string Remark { get; set; }

        public virtual IList<SuggestionAttachment> SuggestionAttachment { get; set; }

        public override int Id
        {
            get
            {
                return SuggestionMasterId;
            }
            set
            {
                SuggestionMasterId = value;
            }
        }
    }
}
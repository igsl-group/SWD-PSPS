using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class LegalAdviceMaster : BaseAuditEntity<int>
    {
        public virtual int LegalAdviceId { get; set; }

        public virtual int RelatedLegalAdviceId { get; set; }

        public virtual string LegalAdviceType { get; set; }

        public virtual string VenueType { get; set; }

        public virtual string LegalAdviceCode { get; set; }

        public virtual string LegalAdviceDescription { get; set; }

        public virtual string PartNum { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual DateTime? EffectiveDate { get; set; }

        public virtual string RequirePspIndicator { get; set; }

        public virtual string Remarks { get; set; }

        public override int Id
        {
            get
            {
                return LegalAdviceId;
            }
            set
            {
                LegalAdviceId = value;
            }
        }
    }
}
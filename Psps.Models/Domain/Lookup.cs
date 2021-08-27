using Psps.Core.Models;

namespace Psps.Models.Domain
{
    public partial class Lookup : BaseAuditEntity<int>
    {
        public Lookup()
        {
        }

        public virtual int LookupId { get; set; }

        public virtual string Type { get; set; }

        public virtual string Code { get; set; }

        public virtual string EngDescription { get; set; }

        public virtual string ChiDescription { get; set; }

        public virtual int DisplayOrder { get; set; }

        public virtual bool IsActive { get; set; }

        public override int Id
        {
            get
            {
                return LookupId;
            }
            set
            {
                LookupId = value;
            }
        }
    }
}
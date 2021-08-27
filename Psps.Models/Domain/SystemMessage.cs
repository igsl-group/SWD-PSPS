using Psps.Core.Models;

namespace Psps.Models.Domain
{
    public partial class SystemMessage : BaseAuditEntity<int>
    {
        public SystemMessage()
        {
        }

        public virtual int SystemMessageId { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string Value { get; set; }

        public override int Id
        {
            get
            {
                return SystemMessageId;
            }
            set
            {
                SystemMessageId = value;
            }
        }
    }
}
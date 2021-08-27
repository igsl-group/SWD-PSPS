using Psps.Core.Models;

namespace Psps.Models.Domain
{
    public partial class SystemParameter : BaseAuditEntity<int>
    {
        public SystemParameter()
        {
        }

        public virtual int SystemParameterId { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string Value { get; set; }

        public override int Id
        {
            get
            {
                return SystemParameterId;
            }
            set
            {
                SystemParameterId = value;
            }
        }
    }
}
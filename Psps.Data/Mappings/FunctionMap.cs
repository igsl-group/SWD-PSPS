using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class FunctionMap : BaseAuditEntityMap<Function, string>
    {
        protected override void MapId()
        {
            Id(x => x.FunctionId).GeneratedBy.Assigned().Column("FunctionId").Length(20);
        }

        protected override void MapEntity()
        {
            Map(x => x.Module).Column("Module").Length(30);
            Map(x => x.Description).Column("Description").Length(500);
            HasManyToMany(x => x.Roles).Table("FunctionsInRoles").ApplyChildFilter<DeletedFilter>();
        }
    }
}
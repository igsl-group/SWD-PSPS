using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class DocumentLibraryMap : BaseAuditEntityMap<DocumentLibrary, int>
    {
        protected override void MapId()
        {
            Id(x => x.DocumentLibraryId).GeneratedBy.Identity().Column("DocumentLibraryId");
        }

        protected override void MapEntity()
        {
            References(x => x.Parent).Column("ParentId");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(100);
            Map(x => x.Path).Column("Path").Not.Nullable().Length(400);
            HasMany(x => x.Documents).KeyColumn("DocumentLibraryId").Inverse();
            HasMany(x => x.DocumentLibraries).KeyColumn("ParentId").Where(x => x.Parent.DocumentLibraryId == x.DocumentLibraryId).Inverse();
        }
    }
}
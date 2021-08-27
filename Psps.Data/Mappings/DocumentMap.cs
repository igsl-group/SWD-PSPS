using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class DocumentMap : BaseAuditEntityMap<Document, int>
    {
        protected override void MapId()
        {
            Id(x => x.DocumentId).GeneratedBy.Identity().Column("DocumentId");
        }

        protected override void MapEntity()
        {
            References(x => x.DocumentLibrary).Column("DocumentLibraryId");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(200);
            Map(x => x.FileName).Column("FileName").Not.Nullable().Length(200);
            Map(x => x.Path).Column("Path").Not.Nullable().Length(400);
            Map(x => x.Remark).Column("Remark").Length(1000);
            Map(x => x.UploadedOn).Column("UploadedOn").Not.Nullable();
            Map(x => x.UploadedById).Column("UploadedById").Not.Nullable().Length(20);
            Map(x => x.UploadedByPost).Column("UploadedByPost").Not.Nullable().Length(20);
        }
    }
}
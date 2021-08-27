using Psps.Core.Models;
using System;

namespace Psps.Models.Domain
{
    public partial class Document : BaseAuditEntity<int>
    {
        public virtual int DocumentId { get; set; }

        public virtual DocumentLibrary DocumentLibrary { get; set; }

        public virtual string Name { get; set; }

        public virtual string FileName { get; set; }

        public virtual string Path { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime UploadedOn { get; set; }

        public virtual string UploadedById { get; set; }

        public virtual string UploadedByPost { get; set; }

        public override int Id
        {
            get
            {
                return DocumentId;
            }
            set
            {
                DocumentId = value;
            }
        }
    }
}
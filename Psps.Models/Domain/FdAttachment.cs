using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdAttachment : BaseAuditEntity<int>
    {
        public virtual int FdAttachmentId { get; set; }

        public virtual FdMaster FdMaster { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FileDescription { get; set; }

        public virtual string FileLocation { get; set; }

        public override int Id
        {
            get
            {
                return FdAttachmentId;
            }
            set
            {
                FdAttachmentId = value;
            }
        }
    }
}
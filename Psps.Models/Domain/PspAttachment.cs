using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspAttachment : BaseAuditEntity<int>
    {
        public PspAttachment()
        {
        }

        public virtual int PspAttachmentId { get; set; }

        public virtual PspMaster PspMaster { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FileDescription { get; set; }

        public override int Id
        {
            get
            {
                return PspAttachmentId;
            }
            set
            {
                PspAttachmentId = value;
            }
        }
    }
}
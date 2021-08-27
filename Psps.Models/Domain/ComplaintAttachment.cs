using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintAttachment : BaseAuditEntity<int>
    {
        public virtual int ComplaintAttachmentId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FileDescription { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintAttachmentId;
            }
            set
            {
                ComplaintAttachmentId = value;
            }
        }
    }
}
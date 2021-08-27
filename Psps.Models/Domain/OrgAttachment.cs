using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgAttachment : BaseAuditEntity<int>
    {
        public virtual int OrgAttachmentId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FileDescription { get; set; }

        public override int Id
        {
            get
            {
                return OrgAttachmentId;
            }
            set
            {
                OrgAttachmentId = value;
            }
        }
    }
}
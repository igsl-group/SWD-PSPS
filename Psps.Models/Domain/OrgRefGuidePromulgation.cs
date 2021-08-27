using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgRefGuidePromulgation : BaseAuditEntity<int>
    {
        public virtual int OrgRefGuidePromulgationId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual DateTime? SendDate { get; set; }

        public virtual DateTime? ReplySlipReceiveDate { get; set; }

        public virtual DateTime? ReplySlipDate { get; set; }

        public virtual string LanguageUsed { get; set; }

        public virtual string OrgReply { get; set; }

        public virtual string PromulgationReason { get; set; }

        public virtual string PartNum { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string ReplySlipPartNum { get; set; }

        public virtual string ReplySlipEnclosureNum { get; set; }

        public virtual string ActivityConcern { get; set; }

        public virtual string FileRef { get; set; }

        public override int Id
        {
            get
            {
                return OrgRefGuidePromulgationId;
            }
            set
            {
                OrgRefGuidePromulgationId = value;
            }
        }
    }
}
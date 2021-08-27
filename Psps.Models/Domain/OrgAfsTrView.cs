using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Domain
{
    public partial class OrgAfsTrView : BaseEntity<string>
    {
        public virtual int OrgId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual string PermitType { get; set; }

        public virtual string FileRef { get; set; }

        public virtual DateTime? AfsRecordStartDate { get; set; }

        public virtual DateTime? AfsRecordEndDate { get; set; }

        public virtual string AfsRecordDetails { get; set; }

        public virtual DateTime? TrackRecordStartDate { get; set; }

        public virtual DateTime? TrackRecordEndDate { get; set; }

        public virtual string TrackRecordDetails { get; set; }

        public virtual int RecordKey { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public override string Id
        {
            get
            {
                return FileRef;
            }
            set
            {
                FileRef = value;
            }
        }
    }
}
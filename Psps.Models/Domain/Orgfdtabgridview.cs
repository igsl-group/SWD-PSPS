using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class OrgFdTabGridView : BaseEntity<int>
    {
        public virtual int OrgId { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string FdYear { get; set; }

        public virtual DateTime? FlagDay { get; set; }

        public virtual string TWR { get; set; }

        public virtual string ApplyForTwr { get; set; }

        public virtual DateTime? ApplicationReceiveDate { get; set; }

        public virtual string ApplicationResult { get; set; }

        public virtual string FdGroup { get; set; }

        public virtual string FdGroupDesc { get; set; }

        public virtual bool? ApplyPledgingMechanismIndicator { get; set; }

        public virtual bool? AfsReceiveIndicator { get; set; }

        public virtual decimal? NetProceed { get; set; }

        public virtual string PermitNum { get; set; }

        public virtual DateTime? PermitIssueDate { get; set; }

        public virtual string FundRaisingPurpose { get; set; }

        public virtual int FdMasterId { get; set; }

        public override int Id
        {
            get
            {
                return FdMasterId;
            }
            set
            {
                FdMasterId = value;
            }
        }
    }
}
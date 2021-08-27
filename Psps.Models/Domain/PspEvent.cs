using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PspEvent : BaseAuditEntity<int>
    {
        public virtual int PspEventId { get; set; }

        public virtual PspMaster PspMaster { get; set; }

        public virtual PspApprovalHistory PspApprovalHistory { get; set; }

        public virtual PspApprovalHistory PspCancelHistory { get; set; }

        public virtual PspAttachment PspAttachment { get; set; }

        public virtual int? PspCancelHistoryId { get; set; }

        public virtual string EventStatus { get; set; }

        public virtual DateTime? EventStartDate { get; set; }

        public virtual DateTime? EventEndDate { get; set; }

        public virtual DateTime? EventStartTime { get; set; }

        public virtual DateTime? EventEndTime { get; set; }

        public virtual string District { get; set; }

        public virtual string Location { get; set; }

        public virtual string ChiLocation { get; set; }

        public virtual string SimpChiLocation { get; set; }

        public virtual bool? PublicPlaceIndicator { get; set; }

        public virtual string CollectionMethod { get; set; }

        public virtual string OtherCollectionMethod { get; set; }

        public virtual string CharitySalesItem { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string ValidationMessage { get; set; }

        public virtual decimal? ProformaRowNum { get; set; }

        public virtual int EventCount { get; set; }

        public virtual string Time { get; set; }

        public virtual string FrasCharityEventId { get; set; }

        public virtual string FrasStatus { get; set; }

        public virtual string FrasResponse { get; set; }

        public override int Id
        {
            get
            {
                return PspEventId;
            }
            set
            {
                PspEventId = value;
            }
        }
    }
}
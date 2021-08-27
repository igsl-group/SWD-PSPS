using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class FdMasterComputeView : BaseEntity<int>
    {

        public virtual int FdMasterId { get; set; }

        public virtual int? FdMaster1stPrevId { get; set; }

        public virtual int? FdMaster2ndPrevId { get; set; }

        public virtual string ApplicationResultInLastYear { get; set; }

        public virtual string LotGroupInLastYear { get; set; }

        public virtual string RefLotGroup { get; set; }        

        public virtual FdMaster FdMaster1stPrev { get; set; }

        public virtual FdMaster FdMaster2ndPrev { get; set; }

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
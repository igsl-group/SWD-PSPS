using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class PublicHoliday : BaseAuditEntity<int>
    {
        public virtual int HolidayId { get; set; }

        public virtual int HolidayYear { get; set; }

        public virtual DateTime HolidayDate { get; set; }

        public override int Id
        {
            get
            {
                return HolidayId;
            }
            set
            {
                HolidayId = value;
            }
        }
    }
}
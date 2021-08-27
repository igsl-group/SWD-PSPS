using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Domain
{
    public class FdList : BaseAuditEntity<int>
    {
        public virtual int FlagDayListId { get; set; }

        public virtual string FlagDayYear { get; set; }

        public virtual string FlagDayType { get; set; }

        public virtual DateTime? FlagDayDate { get; set; }

        public override int Id
        {
            get
            {
                return FlagDayListId;
            }
            set
            {
                FlagDayListId = value;
            }
        }
    }
}
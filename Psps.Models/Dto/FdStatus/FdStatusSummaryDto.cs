using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdStatus
{
    public partial class FdStatusSummary : BaseEntity<string>
    {   
        public virtual string FdYear { get; set; }
        
        public virtual int OrgTWR { get; set; }
        
        public virtual int OrgRFD { get; set; }
        
        public virtual int ApplyTWR { get; set; }
        
        public virtual int ApplyRFD { get; set; }
        
        public virtual int EligibleTWR_A { get; set; }
        
        public virtual int EligibleTWR_B { get; set; }
        
        public virtual int EligibleRFD_A { get; set; }
        
        public virtual int EligibleRFD_B { get; set; }
        
        public virtual int IneligibleTWR { get; set; }

        public virtual int IneligibleRFD { get; set; }

        public virtual int LateTWR { get; set; }

        public virtual int LateRFD { get; set; } 

        public override string Id
        {
            get
            {
                return FdYear;
            }
            set
            {
                FdYear = value;
            }
        }
    }
}
using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdStatus
{
    public partial class FdStatusDto : BaseEntity<string>
    {
        //public int? DisasterStatisticsId { get; set; }

        public string Type { get; set; }

        public string Year { get; set; }

        public int DataInputStage { get; set; }

        public int ReadyForApproval { get; set; }

        public int Approved { get; set; }

        public string MultiBatchApp { get; set; }

        public byte[] RowVersion { get; set; }

        public override string Id
        {
            get
            {
                return Year;
            }
            set
            {
                Year = value;
            }
        }
    }
}
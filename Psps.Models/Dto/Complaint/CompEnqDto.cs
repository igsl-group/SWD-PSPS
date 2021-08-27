using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Complaint
{
    public partial class CompEnqDto : BaseEntity<string>
    {
        public string Year { get; set; }

        public string Code { get; set; }

        public int Count { get; set; }

        public IEnumerable<string> EngDescriptionList { get; set; }

        public IDictionary<string, int> CountList { get; set; }

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
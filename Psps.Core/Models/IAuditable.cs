using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.Models
{
    public interface IAuditable
    {
        bool IsDeleted { get; set; }

        string CreatedById { get; set; }

        string CreatedByPost { get; set; }

        DateTime CreatedOn { get; set; }

        string UpdatedById { get; set; }

        string UpdatedByPost { get; set; }

        DateTime? UpdatedOn { get; set; }

        byte[] RowVersion { get; set; }
    }
}
using Psps.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.JqGrid.Models
{
    public enum GroupOp
    {
        [EnumMember(Value = "AND")]
        AND,

        [EnumMember(Value = "OR")]
        OR
    }
}
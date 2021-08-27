using Psps.Core.Helper;
using System.Runtime.Serialization;

namespace Psps.Core.JqGrid.Models
{
    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [EnumMember(Value = "eq")]
        Equal,

        [EnumMember(Value = "ne")]
        NotEqual,

        [EnumMember(Value = "lt")]
        LessThan,

        [EnumMember(Value = "le")]
        LessThanOrEqual,

        [EnumMember(Value = "gt")]
        GreaterThan,

        [EnumMember(Value = "ge")]
        GreaterThanOrEqual,

        [EnumMember(Value = "bw")]
        BeginsWith,

        [EnumMember(Value = "bn")]
        NotBeginWith,

        [EnumMember(Value = "ew")]
        EndsWith,

        [EnumMember(Value = "en")]
        NotEndWith,

        [EnumMember(Value = "cn")]
        Contains,

        [EnumMember(Value = "nc")]
        NotContain,

        [EnumMember(Value = "nu")]
        IsNull
    }
}
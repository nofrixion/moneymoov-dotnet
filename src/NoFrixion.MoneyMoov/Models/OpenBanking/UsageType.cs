
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public enum UsageType
{
    [EnumMember(Value = "PERSONAL")]
    PERSONAL = 1,

    [EnumMember(Value = "BUSINESS")]
    BUSINESS = 2,

    [EnumMember(Value = "OTHER")]
    OTHER = 3,

    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 4
}

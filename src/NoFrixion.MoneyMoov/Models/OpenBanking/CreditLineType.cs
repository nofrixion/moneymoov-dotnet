
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[JsonConverter(typeof(StringEnumConverter))]
public enum CreditLineType
{
    [EnumMember(Value = "AVAILABLE")]
    AVAILABLE = 1,

    [EnumMember(Value = "CREDIT")]
    CREDIT = 2,

    [EnumMember(Value = "EMERGENCY")]
    EMERGENCY = 3,

    [EnumMember(Value = "PRE_AGREED")]
    PREAGREED = 4,

    [EnumMember(Value = "TEMPORARY")]
    TEMPORARY = 5,

    [EnumMember(Value = "OTHER")]
    OTHER = 6,

    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 7
}

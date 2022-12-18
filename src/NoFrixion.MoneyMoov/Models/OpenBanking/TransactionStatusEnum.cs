
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[JsonConverter(typeof(StringEnumConverter))]
public enum TransactionStatusEnum
{
    [EnumMember(Value = "BOOKED")]
    BOOKED = 1,

    [EnumMember(Value = "PENDING")]
    PENDING = 2
}

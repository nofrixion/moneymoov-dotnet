
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionStatusEnum
{
    [EnumMember(Value = "BOOKED")]
    BOOKED = 1,

    [EnumMember(Value = "PENDING")]
    PENDING = 2
}

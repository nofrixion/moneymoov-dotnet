
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public enum TransactionStatusEnum
{
    [EnumMember(Value = "BOOKED")]
    BOOKED = 1,

    [EnumMember(Value = "PENDING")]
    PENDING = 2
}

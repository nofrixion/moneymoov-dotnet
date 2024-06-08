
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// Specifies the type of the stated account balance.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountBalanceType
{
    [EnumMember(Value = "CLOSING_AVAILABLE")]
    CLOSINGAVAILABLE = 1,

    [EnumMember(Value = "CLOSING_BOOKED")]
    CLOSINGBOOKED = 2,

    [EnumMember(Value = "CLOSING_CLEARED")]
    CLOSINGCLEARED = 3,

    [EnumMember(Value = "EXPECTED")]
    EXPECTED = 4,

    [EnumMember(Value = "FORWARD_AVAILABLE")]
    FORWARDAVAILABLE = 5,

    [EnumMember(Value = "INFORMATION")]
    INFORMATION = 6,

    [EnumMember(Value = "INTERIM_AVAILABLE")]
    INTERIMAVAILABLE = 7,

    [EnumMember(Value = "INTERIM_BOOKED")]
    INTERIMBOOKED = 8,

    [EnumMember(Value = "INTERIM_CLEARED")]
    INTERIMCLEARED = 9,

    [EnumMember(Value = "OPENING_AVAILABLE")]
    OPENINGAVAILABLE = 10,

    [EnumMember(Value = "OPENING_BOOKED")]
    OPENINGBOOKED = 11,

    [EnumMember(Value = "OPENING_CLEARED")]
    OPENINGCLEARED = 12,

    [EnumMember(Value = "PREVIOUSLY_CLOSED_BOOKED")]
    PREVIOUSLYCLOSEDBOOKED = 13,

    [EnumMember(Value = "AUTHORISED")]
    AUTHORISED = 14,

    [EnumMember(Value = "OTHER")]
    OTHER = 15,

    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 16
}


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AddressTypeEnum
{
    [EnumMember(Value = "BUSINESS")]
    BUSINESS = 1,

    [EnumMember(Value = "CORRESPONDENCE")]
    CORRESPONDENCE = 2,

    [EnumMember(Value = "DELIVERY_TO")]
    DELIVERYTO = 3,

    [EnumMember(Value = "MAIL_TO")]
    MAILTO = 4,

    [EnumMember(Value = "PO_BOX")]
    POBOX = 5,

    [EnumMember(Value = "POSTAL")]
    POSTAL = 6,

    [EnumMember(Value = "RESIDENTIAL")]
    RESIDENTIAL = 7,

    [EnumMember(Value = "STATEMENT")]
    STATEMENT = 8,

    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 9
}

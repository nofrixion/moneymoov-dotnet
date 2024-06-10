
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public enum AccountIdentificationType
{
    [EnumMember(Value = "SORT_CODE")]
    SORTCODE = 1,

    [EnumMember(Value = "ACCOUNT_NUMBER")]
    ACCOUNTNUMBER = 2,

    [EnumMember(Value = "IBAN")]
    IBAN = 3,

    [EnumMember(Value = "BBAN")]
    BBAN = 4,

    [EnumMember(Value = "BIC")]
    BIC = 5,

    [EnumMember(Value = "PAN")]
    PAN = 6,

    [EnumMember(Value = "MASKED_PAN")]
    MASKEDPAN = 7,

    [EnumMember(Value = "MSISDN")]
    MSISDN = 8,

    [EnumMember(Value = "BSB")]
    BSB = 9,

    [EnumMember(Value = "NCC")]
    NCC = 10,

    [EnumMember(Value = "ABA")]
    ABA = 11,

    [EnumMember(Value = "ABA_WIRE")]
    ABAWIRE = 12,

    [EnumMember(Value = "ABA_ACH")]
    ABAACH = 13,

    [EnumMember(Value = "EMAIL")]
    EMAIL = 14,

    [EnumMember(Value = "ROLL_NUMBER")]
    ROLLNUMBER = 15,

    [EnumMember(Value = "BLZ")]
    BLZ = 16,

    [EnumMember(Value = "IFS")]
    IFS = 17,

    [EnumMember(Value = "CLABE")]
    CLABE = 18,

    [EnumMember(Value = "CTN")]
    CTN = 19,

    [EnumMember(Value = "BRANCH_CODE")]
    BRANCHCODE = 20
}

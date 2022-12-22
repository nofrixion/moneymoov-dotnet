
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public enum AccountType
{
    [EnumMember(Value = "CASH_TRADING")]
    CASHTRADING = 1,

    [EnumMember(Value = "CASH_INCOME")]
    CASHINCOME = 2,

    [EnumMember(Value = "CASH_PAYMENT")]
    CASHPAYMENT = 3,

    [EnumMember(Value = "CHARGE_CARD")]
    CHARGECARD = 4,

    [EnumMember(Value = "CHARGES")]
    CHARGES = 5,

    [EnumMember(Value = "COMMISSION")]
    COMMISSION = 6,

    [EnumMember(Value = "CREDIT_CARD")]
    CREDITCARD = 7,

    [EnumMember(Value = "CURRENT")]
    CURRENT = 8,

    [EnumMember(Value = "E_MONEY")]
    EMONEY = 9,

    [EnumMember(Value = "LIMITED_LIQUIDITY_SAVINGS_ACCOUNT")]
    LIMITEDLIQUIDITYSAVINGSACCOUNT = 10,

    [EnumMember(Value = "LOAN")]
    LOAN = 11,

    [EnumMember(Value = "MARGINAL_LENDING")]
    MARGINALLENDING = 12,

    [EnumMember(Value = "MONEY_MARKET")]
    MONEYMARKET = 13,

    [EnumMember(Value = "MORTGAGE")]
    MORTGAGE = 14,

    [EnumMember(Value = "NON_RESIDENT_EXTERNAL")]
    NONRESIDENTEXTERNAL = 15,

    [EnumMember(Value = "OTHER")]
    OTHER = 16,

    [EnumMember(Value = "OVERDRAFT")]
    OVERDRAFT = 17,

    [EnumMember(Value = "OVERNIGHT_DEPOSIT")]
    OVERNIGHTDEPOSIT = 18,

    [EnumMember(Value = "PREPAID_CARD")]
    PREPAIDCARD = 19,

    [EnumMember(Value = "SALARY")]
    SALARY = 20,

    [EnumMember(Value = "SAVINGS")]
    SAVINGS = 21,

    [EnumMember(Value = "SETTLEMENT")]
    SETTLEMENT = 22,

    [EnumMember(Value = "TAX")]
    TAX = 23,

    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 24
}

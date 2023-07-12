// -----------------------------------------------------------------------------
// Filename: CurrencyTypeEnum.cs
// 
// Description: List of the currencies supported by NoFrixion.
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 26 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 03 Dec 2021  Aaron Clauson   Renamed from Currency to CurrencyTypeEnum.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov;

public enum CurrencyTypeEnum
{
    [EnumMember(Value = "NONE")]
    None = 0,

    /// <summary>
    /// Great Britain Pounds (Fiat).
    /// </summary>
    [EnumMember(Value = "GBP")]
    GBP = 1,

    /// <summary>
    /// Euro (Fiat).
    /// </summary>
    [EnumMember(Value = "EUR")]
    EUR = 2,

    // Start non-fiat currencies from 1000 to avoid conflicting with supplier mappings.

    /// <summary>
    /// Bitcoin.
    /// </summary>
    [EnumMember(Value = "BTC")]
    BTC = 1001,

    /// <summary>
    /// Testnet Bitcoin.
    /// </summary>
    [EnumMember(Value = "TBTC")]
    TBTC = 1002
}
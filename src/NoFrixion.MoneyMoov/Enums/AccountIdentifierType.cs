// -----------------------------------------------------------------------------
//  Filename: AccountIdentifierType.cs
// 
//  Description: MerchantAccount identifier type:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Defines Type
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AccountIdentifierType
{
    /// <summary>
    /// Enum SCAN for value: SCAN
    /// </summary>
    SCAN = 1,

    /// <summary>
    /// Enum IBAN for value: IBAN
    /// </summary>
    IBAN = 2,

    /// <summary>
    /// Enum DD for value: DD
    /// </summary>
    DD = 3,

    /// <summary>
    /// Enum INTL for value: INTL
    /// </summary>
    INTL = 4
}
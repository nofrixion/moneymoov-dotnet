// -----------------------------------------------------------------------------
//  Filename: AccountStatus.cs
// 
//  Description: Enum for the MerchantAccount class:
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

[JsonConverter(typeof(StringEnumConverter))]
public enum AccountStatus
{
    ACTIVE = 1,

    INACTIVE = 2,

    BLOCKED = 3,

    CLOSED = 4,

    CLIENTBLOCKED = 5
}
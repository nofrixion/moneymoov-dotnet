// -----------------------------------------------------------------------------
//  Filename: WalletsEnum.cs
// 
//  Description: Enum for wallets
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  29 05 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WalletsEnum
{
    ApplePay,
    
    GooglePay
}
// -----------------------------------------------------------------------------
//  Filename: TimeFrequencyEnum.cs
// 
//  Description: Enum that specified frequency of data to be returned.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  20 10 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TimeFrequencyEnum
{
    None,
    Daily,
}
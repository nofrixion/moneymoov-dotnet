// -----------------------------------------------------------------------------
//  Filename: StatementGenerationStatusEnum.cs
// 
//  Description: Enum for the status of a transaction statement generation.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  09 05 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatementGenerationStatusEnum
{
    Unknown,
    Generating,
    Ready
}
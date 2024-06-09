// -----------------------------------------------------------------------------
//  Filename: TransactionStatementFormat.cs
// 
//  Description: Enum for the format of a transaction statement.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  03 05 2024  Axel Granillo   Created, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionStatementFormat
{
    Pdf,
    Csv
}
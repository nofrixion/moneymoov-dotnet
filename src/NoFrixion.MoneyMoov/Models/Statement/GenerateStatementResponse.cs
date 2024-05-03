// -----------------------------------------------------------------------------
//  Filename: GenerateStatementResponse.cs
// 
//  Description: Response model when generating a transaction statement.
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

namespace NoFrixion.MoneyMoov.Models;

public class GenerateStatementResponse
{
    public string Status { get; set; } = "";
    public string StatementUrl { get; set; } = "";
}
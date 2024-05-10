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

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class GenerateStatementResponse
{
    public StatementGenerationStatusEnum Status { get; set; } = StatementGenerationStatusEnum.Unknown;
    public string StatementUrl { get; set; } = "";
}
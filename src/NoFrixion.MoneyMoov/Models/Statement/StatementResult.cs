//-----------------------------------------------------------------------------
// Filename: StatementResult.cs
// 
// Description: Model to represent the result of a NoFrixion statement generation.
// Statements are typically transaction records for a single payment account, i.e.
// analagous to a bank statement.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 May 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class StatementResult
{
    public required byte[] Content { get; set; }
    public required string ContentType { get; set; }
    public required string FileName { get; set; }
}
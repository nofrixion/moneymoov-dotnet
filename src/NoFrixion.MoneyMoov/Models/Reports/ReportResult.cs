//-----------------------------------------------------------------------------
// Filename: ReportResult.cs
// 
// Description: Model to represent the result of a NoFrixion report execution.
// Reports are things such  as an account transaction statement or SWIFT report.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 15 Jan 2024  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class ReportResult
{
    public Guid MerchantID { get; set; }
    public ReportTypesEnum ReportType { get; set; }
    public string ReportName { get; set; } = string.Empty;
    public DateTimeOffset LastCompletedAt { get; set; }
    public int StatementNumber { get; set; }
    public string Contents { get; set; } = string.Empty;
}
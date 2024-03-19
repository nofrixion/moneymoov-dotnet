//-----------------------------------------------------------------------------
// Filename: Report.cs
// 
// Description: Model to represent a NoFrixion report. Reports are things such 
// as an account transaction statement or SWIFT report.
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

public class Report : IWebhookPayload
{
    public Guid ID { get; set; }
    public Guid? MerchantID { get; set; }
    public Guid? CreatedByUserID { get; set; }
    public ReportTypesEnum ReportType { get; set; }
    public string ReportName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset Inserted { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public string? CronExpression { get; set; }
    public ReportStatusTypesEnum Status { get; set; }
    public DateTimeOffset LastCompletedAt { get; set; }
    public int StatementNumber { get; set; }
    public bool IsDisabled { get; set; }
    public string? Error { get; set; }
}
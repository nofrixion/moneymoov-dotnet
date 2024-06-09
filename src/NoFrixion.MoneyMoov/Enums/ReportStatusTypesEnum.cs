//-----------------------------------------------------------------------------
// Filename:ReportStatusTypesEnum.cs
// 
// Description: List of the statuses a report can be in.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 19 Dec 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportStatusTypesEnum
{
    None = 0,

    /// <summary>
    /// The report was successfully run.
    /// </summary>
    Completed = 1,

    /// <summary>
    /// The report is currently running.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// The report is currently running.
    /// </summary>
    Error = 3,
}
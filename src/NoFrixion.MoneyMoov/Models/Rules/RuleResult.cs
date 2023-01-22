//-----------------------------------------------------------------------------
// Filename: RulesResult.cs
// 
// Description: A model that represents the status fields produced by a MoneyMoov
// Rule execution.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 22 Jan 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class RuleResult
{
    public Guid RuleID { get; set; }

    public RuleResultsEnum Result { get; set; }

    public DateTimeOffset ExecutedAt { get; set; }

    public string? Message { get; set; }
}
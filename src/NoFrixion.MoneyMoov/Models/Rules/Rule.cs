//-----------------------------------------------------------------------------
// Filename: Rule.cs
// 
// Description: The model that represents a MoneyMoov Rule.
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

public class Rule
{
    public Guid ID { get; set; }
    public Guid AccountID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RuleStatusEnum Status { get; set; }
    public bool TriggerOnPayIn { get; set; }
    public bool TriggerOnPayOut { get; set; }
    public string? TriggerCronExpression { get; }
    public DateTimeOffset? StartAt { get; }
    public DateTimeOffset? EndAt { get; }
    public List<RuleAction> RuleActions { get; set; } = new List<RuleAction>();
    public string? ApproveUrl { get; set; }
    public string? ApproveHash { get; set; }
    public string? OnExecutedWebHookUrl { get; set; }
    public DateTimeOffset Inserted { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}
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
    public Guid UserID { get; set; }
    public Guid? ApproverID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDisabled { get; set; }
    public RuleStatusEnum Status { get; set; }
    public bool TriggerOnPayIn { get; set; }
    public bool TriggerOnPayOut { get; set; }
    public string TriggerCronExpression { get; set; } = string.Empty;
    public DateTimeOffset? StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
    public SweepAction SweepAction { get; set; } = SweepAction.Empty;

    /// <summary>
    /// If set this property holds the URL an approver needs to visit in order to
    /// complete a strong authentication check in order to approve the rule.
    /// </summary>
    public string? ApproveUrl { get; set; }

    public string? OnExecutedWebHookUrl { get; set; }
    public DateTimeOffset Inserted { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// The approval hash is used when approving the rule and to detect when critical
    /// fields change.
    /// </summary>
    public string GeApprovalHash()
    {
        string input = ID.ToString() + SweepAction.GetDestinationApprovalHash();
        return HashHelper.CreateHash(input);
    }
}
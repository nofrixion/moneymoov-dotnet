//-----------------------------------------------------------------------------
// Filename: RuleCreate.cs
// 
// Description: The model use to create a new MoneyMoov Rule.
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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class RuleCreate
{
    /// <summary>
    /// The ID of the account the rule will apply to.
    /// </summary>
    public Guid AccountID { get; set; }

    /// <summary>
    /// A name to succinctly describe the rule.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Arbitrary description for the rule.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request with the rule details when the rule status changes to 
    /// approved. The webhook payload will contain the full Rule object.
    /// </summary>
    public string? OnApprovedWebHookUrl { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request when a rule execution attempt fails. The webhook 
    /// payload will contain a NoFrixionPorblem object.
    /// </summary>
    public string? OnExecutionErrorWebHookUrl { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request when a rule execution attempt succeeds. The webhook 
    /// payload will contain a ?.
    /// </summary>
    public string? OnExecutionSuccessWebHookUrl { get; set; }

    /// <summary>
    /// If set to true the rule will be disabled from executing.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Set to true if the rule execution should be triggered when the account 
    /// receives a pay in (credit).
    /// </summary>
    public bool TriggerOnPayIn { get; set; }

    /// <summary>
    /// Set to true if the rule execution should be triggered when the account 
    /// makes a pay out (debit).
    /// </summary>
    public bool TriggerOnPayOut { get; set; }

    /// <summary>
    /// If the rule should be executed on a recurring schedule this is the expression
    /// that sets the schedule. The expression uses a CRON format.
    /// </summary>
    public string? TriggerCronExpression { get; set; }

    /// <summary>
    /// Optional start time for rule executions. If this value is set the rule will not
    /// be triggered until the start time has been reached.
    /// </summary>
    public DateTimeOffset? StartAt { get; set; }

    /// <summary>
    /// Optional end time for rule executions. If this value is set the rule will not
    /// be triggered after the end time has been reached.
    /// </summary>
    public DateTimeOffset? EndAt { get; set; }

    /// <summary>
    /// The sweep action parameters for the rule. Any changes to the sweep rule parameters
    /// will require an administrator to authorise.
    /// </summary>
    [Required]
    public SweepAction? SweepAction { get; set; }

    /// <summary>
    /// Places all the rule create model's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary of string key value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>
        {
            { nameof(AccountID), AccountID.ToString() },
            { nameof(Name), Name},
            { nameof(Description), Description ?? string.Empty},
            { nameof(OnApprovedWebHookUrl), OnApprovedWebHookUrl ?? string.Empty },
            { nameof(OnExecutionErrorWebHookUrl), OnExecutionErrorWebHookUrl ?? string.Empty },
            { nameof(OnExecutionSuccessWebHookUrl), OnExecutionSuccessWebHookUrl ?? string.Empty },
            { nameof(IsDisabled), IsDisabled.ToString() },
            { nameof(TriggerOnPayIn), TriggerOnPayIn.ToString() },
            { nameof(TriggerOnPayOut), TriggerOnPayOut.ToString() },
            { nameof(TriggerCronExpression), TriggerCronExpression ?? string.Empty },
            { nameof(StartAt), StartAt != null ? StartAt.Value.ToString() : string.Empty },
            { nameof(EndAt), EndAt != null ? EndAt.Value.ToString() : string.Empty }
        };

        if (SweepAction != null && !SweepAction.IsEmpty())
        {
            dict = dict.Concat(SweepAction.ToDictionary($"{nameof(SweepAction)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }
}
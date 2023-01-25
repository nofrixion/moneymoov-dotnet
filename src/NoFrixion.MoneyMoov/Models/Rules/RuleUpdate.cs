//-----------------------------------------------------------------------------
// Filename: RuleUpdate.cs
// 
// Description: The model use to update a MoneyMoov Rule.
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

public class RuleUpdate
{
    /// <summary>
    /// A name to succinctly describe the rule.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Arbitrary description for the rule.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A webhook URL to invoke when a rule exection completes.
    /// </summary>
    public string? OnExecutedWebHookUrl { get; set; }

    /// <summary>
    /// If set to false the rule will be disabled from executing.
    /// </summary>
    public bool? IsEnabled { get; set; }

    /// <summary>
    /// If set to true the rule will be schedule for an immediate execution.
    /// </summary>
    public bool? DoImmediateExecution { get; set; }

    /// <summary>
    /// Set to true if the rule execution should be triggered when the account 
    /// receives a pay in (credit).
    /// </summary>
    public bool? TriggerOnPayIn { get; set; }

    /// <summary>
    /// Set to true if the rule execution should be triggered when the account 
    /// makes a pay out (debit).
    /// </summary>
    public bool? TriggerOnPayOut { get; set; }

    /// <summary>
    /// If the rule should be executed on a recurring schedule this is the expression
    /// that sets the schedule. The expression uses a CRON format.
    /// </summary>
    public string? TriggerCronExpression { get; }

    /// <summary>
    /// Optional start time for rule executions. If this value is set the rule will not
    /// be triggered until the start time has been reached.
    /// </summary>
    public DateTimeOffset? StartAt { get; }

    /// <summary>
    /// Optional end time for rule executions. If this value is set the rule will not
    /// be triggered after the end time has been reached.
    /// </summary>
    public DateTimeOffset? EndAt { get; }

    /// <summary>
    /// The sweep action parameters for the rule. Any changes to the sweep rule parameters
    /// will require an administrator to authorise.
    /// </summary>
    public SweepAction? SweepAction { get; set; }
}
﻿//-----------------------------------------------------------------------------
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
    /// A webhook URL to invoke when a rule exection completes.
    /// </summary>
    public string? OnExecutedWebHookUrl { get; set; }

    /// <summary>
    /// If set to false the rule will be disabled from executing.
    /// </summary>
    public bool IsEnabled { get; set; }

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
    /// A list of actions that the execution of the rule will invoke. All changes to the rule actions
    /// require an administrator to authorise.
    /// </summary>
    [Required]
    public string RuleActionsJson { get; set; } = string.Empty;

    /// <summary>
    /// Places all the rule create model's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary ot string key value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(AccountID), AccountID.ToString() },
            { nameof(Name), Name},
            { nameof(Description), Description ?? string.Empty},
            { nameof(OnExecutedWebHookUrl), OnExecutedWebHookUrl ?? string.Empty },
            { nameof(IsEnabled), IsEnabled.ToString() },
            { nameof(TriggerOnPayIn), TriggerOnPayIn.ToString() },
            { nameof(TriggerOnPayOut), TriggerOnPayOut.ToString() },
            { nameof(TriggerCronExpression), TriggerCronExpression ?? string.Empty },
            { nameof(StartAt), StartAt != null ? StartAt.Value.ToString() : string.Empty },
            { nameof(EndAt), EndAt != null ? EndAt.Value.ToString() : string.Empty },
            { nameof(RuleActionsJson), RuleActionsJson ?? string.Empty},
        };
    }
}
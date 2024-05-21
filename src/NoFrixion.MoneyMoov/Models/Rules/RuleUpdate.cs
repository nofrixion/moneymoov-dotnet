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
    /// If set to false the rule will be disabled from executing.
    /// </summary>
    public bool? IsDisabled { get; set; }

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
    public SweepAction? SweepAction { get; set; }

    /// <summary>
    /// If set this secret will be used to sign Web Hook requests.
    /// </summary>
    public string? WebHookSecret { get; set; }

    /// <summary>
    /// Places all the rule update model's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary of string key value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (Name != null) dict.Add(nameof(Name), Name);
        if (Description != null) dict.Add(nameof(Description), Description);
        if (OnApprovedWebHookUrl != null) dict.Add(nameof(OnApprovedWebHookUrl), OnApprovedWebHookUrl);
        if (OnExecutionErrorWebHookUrl != null) dict.Add(nameof(OnExecutionErrorWebHookUrl), OnExecutionErrorWebHookUrl);
        if (OnExecutionSuccessWebHookUrl != null) dict.Add(nameof(OnExecutionSuccessWebHookUrl), OnExecutionSuccessWebHookUrl);
        if (IsDisabled != null) dict.Add(nameof(IsDisabled), IsDisabled.Value.ToString());
        if (TriggerOnPayIn != null) dict.Add(nameof(TriggerOnPayIn), TriggerOnPayIn.Value.ToString());
        if (TriggerOnPayOut != null) dict.Add(nameof(TriggerOnPayOut), TriggerOnPayOut.Value.ToString());
        if (TriggerCronExpression != null) dict.Add(nameof(TriggerCronExpression), TriggerCronExpression);
        if (StartAt != null) dict.Add(nameof(StartAt), StartAt.Value.ToString());
        if (EndAt != null) dict.Add(nameof(EndAt), EndAt.Value.ToString());
        if (WebHookSecret != null) dict.Add(nameof(WebHookSecret), WebHookSecret);

        if (SweepAction != null && !SweepAction.IsEmpty())
        {
            dict = dict.Concat(SweepAction.ToDictionary($"{nameof(SweepAction)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }
}
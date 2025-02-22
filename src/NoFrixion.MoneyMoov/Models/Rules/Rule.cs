﻿//-----------------------------------------------------------------------------
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

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Approve;
using Quartz;
using static System.String;

namespace NoFrixion.MoneyMoov.Models;

public class Rule : IValidatableObject, IWebhookPayload
{
    /// <summary>
    /// The minimum interval between rule executions in minutes.
    /// </summary>
    public const int MINIMUM_CRON_INTERVAL_MINUTES = 1;

    public Guid ID { get; set; }
    public Guid AccountID { get; set; }

    /// <summary>
    /// The ID of the merchant that owns the account.
    /// </summary>
    public Guid MerchantID { get; set; }

    public Guid? UserID { get; set; }
    public Guid? ApproverID { get; set; }
    public string Name { get; set; } = Empty;
    public string? Description { get; set; }
    public bool IsDisabled { get; set; }
    public RuleStatusEnum Status { get; set; }
    public bool TriggerOnPayIn { get; set; }

    [Obsolete("Payout triggers are no longer supported.")]
    public bool TriggerOnPayOut { get; set; }

    public string TriggerCronExpression { get; set; } = Empty;

    public string TimeZoneId { get; set; } = Empty;
    public DateTimeOffset? StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
    public SweepAction SweepAction { get; set; } = SweepAction.Empty;

    /// <summary>
    /// If set this property holds the URL an approver needs to visit in order to
    /// complete a strong authentication check in order to approve the rule.
    /// </summary>
    public string? ApproveUrl { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request with the rule details when the rule status changes to 
    /// approved. The webhook payload will contain the full Rule object.
    /// </summary>
    public string? OnApprovedWebHookUrl { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request when a rule execution attempt fails. The webhook 
    /// payload will contain a Problem object.
    /// </summary>
    public string? OnExecutionErrorWebHookUrl { get; set; }

    /// <summary>
    /// Optional URL to receive an HTTP request when a rule execution attempt succeeds. The webhook 
    /// payload will contain a list of any payouts that were submitted for the rule execution.
    /// </summary>
    public string? OnExecutionSuccessWebHookUrl { get; set; }

    /// <summary>
    /// If set this secret will be used to sign Web Hook requests.
    /// </summary>
    public string? WebHookSecret { get; set; }

    public DateTimeOffset Inserted { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    public DateTimeOffset? LastExecutedAt { get; set; }

    /// <summary>
    /// The most recent transaction date when the rule was last run.
    /// </summary>
    public DateTimeOffset LastRunAtTransactionDate { get; set; }

    public User? CreatedBy { get; set; } = null!;

    [Obsolete("Refer to Authorisations instead.")]
    public User? AuthorisedBy { get; set; }

    /// <summary>
    /// A list of the users who have successfully authorised the latest version of the rule and when.
    /// </summary>
    public List<Authorisation>? Authorisations { get; set; }

    public PaymentAccount? Account { get; set; } = null!;

    /// <summary>
    /// The number of authorisers required for this rule.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the rule.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }

    /// <summary>
    /// True if the rule can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the current user has authorised.
    /// </summary>
    public bool HasCurrentUserAuthorised { get; set; }

    public required string Nonce { get; set; }

    /// <summary>
    /// A list of authentication types allowed to authorise the payout.
    /// </summary>
    public List<AuthenticationTypesEnum>? AuthenticationMethods { get; set; }

    /// <summary>
    /// The approval hash is used when approving the rule and to detect when critical
    /// fields change.
    /// </summary>
    public string GetApprovalHash()
    {
        string input = ID.ToString() + SweepAction.GetDestinationApprovalHash() + Nonce;
        return HashHelper.CreateHash(input);
    }

    private bool IsCronExpressionScheduledMoreThanXMinutes(string cronExpression, int minutes)
    {
        var interval = TimeSpan.FromMinutes(minutes);

        // Parse the cron expression
        var cron = new CronExpression(cronExpression);

        // Get the next 2 scheduled dates/times for the cron expression
        var now = DateTime.Now;
        var next1 = cron.GetNextValidTimeAfter(now);
        var next2 = cron.GetNextValidTimeAfter(next1 ?? now);

        // If the interval between the next 2 scheduled dates is less than X minutes,
        // the cron expression will run more frequently than every X minutes
        return (next2 - next1) >= interval;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!TriggerOnPayIn && string.IsNullOrEmpty(TriggerCronExpression))
        {
            yield return new ValidationResult($"A CRON expression must be set when a payin trigger is not set.",
                               new string[] { nameof(TriggerCronExpression) });
        }

        if (TriggerOnPayIn && !string.IsNullOrEmpty(TriggerCronExpression))
        {
            yield return new ValidationResult($"A CRON expression cannot be set when a payin trigger is set.",
                new string[] { nameof(TriggerCronExpression) });
        }

        if (!IsNullOrEmpty(TriggerCronExpression))
        {
            if (!CronExpression.IsValidExpression(TriggerCronExpression))
            {
                yield return new ValidationResult($"Invalid TriggerCronExpression. Please refer to https://www.quartz-scheduler.net/documentation/quartz-3.x/how-tos/crontrigger.html#examples for valid examples.",
                    new string[] { nameof(TriggerCronExpression) });
            }
            else
            {
                if (!IsCronExpressionScheduledMoreThanXMinutes(TriggerCronExpression, MINIMUM_CRON_INTERVAL_MINUTES))
                {
                    yield return new ValidationResult($"Invalid TriggerCronExpression. The minimum interval between rule executions is {MINIMUM_CRON_INTERVAL_MINUTES} minutes.",
                        new string[] { nameof(TriggerCronExpression) });
                }
            }
        }

        if (SweepAction != null)
        {
            foreach (var err in SweepAction.Validate(validationContext))
            {
                yield return err;
            }
        }
    }

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem("The Rule had one or more validation errors.", validationResults);
    }
}

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

using System.ComponentModel.DataAnnotations;
using Quartz;
using static System.String;

namespace NoFrixion.MoneyMoov.Models;

public class Rule : IValidatableObject
{
    public Guid ID { get; set; }
    public Guid AccountID { get; set; }
    public Guid? UserID { get; set; }
    public Guid? ApproverID { get; set; }
    public string Name { get; set; } = Empty;
    public string? Description { get; set; }
    public bool IsDisabled { get; set; }
    public RuleStatusEnum Status { get; set; }
    public bool TriggerOnPayIn { get; set; }
    public bool TriggerOnPayOut { get; set; }
    public string TriggerCronExpression { get; set; } = Empty;
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
    /// The most recent transaction date when the rule was last run.
    /// </summary>
    public DateTimeOffset LastRunAtTransactionDate { get; set; }

    /// <summary>
    /// The approval hash is used when approving the rule and to detect when critical
    /// fields change.
    /// </summary>
    public string GeApprovalHash()
    {
        string input = ID.ToString() + SweepAction.GetDestinationApprovalHash();
        return HashHelper.CreateHash(input);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ((TriggerOnPayIn || TriggerOnPayOut) && !IsNullOrEmpty(TriggerCronExpression))
        {
            yield return new ValidationResult($"A payin and/or payout trigger cannot be set at the same time as a CRON expression trigger.",
                new string[] { nameof(TriggerCronExpression) });
        }

        if (!IsNullOrEmpty(TriggerCronExpression))
        {
            if (!CronExpression.IsValidExpression(TriggerCronExpression))
            {
                yield return new ValidationResult($"Invalid TriggerCronExpression. Please refer to https://www.quartz-scheduler.net/documentation/quartz-3.x/how-tos/crontrigger.html#examples for valid examples.",
                    new string[] { nameof(TriggerCronExpression) });
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
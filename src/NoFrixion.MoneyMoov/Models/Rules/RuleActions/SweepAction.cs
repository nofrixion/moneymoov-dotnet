//-----------------------------------------------------------------------------
// Filename: SweepAction.cs
// 
// Description: A Rule Action that sweeps all the available funds in an account
// to a different account.
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

public class SweepAction : IValidatableObject
{
    private static SweepAction _empty = new SweepAction { _isEmpty = true };
    public static SweepAction Empty
    {
        get
        {
            return _empty;
        }
    }

    private bool _isEmpty = false;

    public int Priority { get; set; }

    public RuleActionsEnum ActionType { get; } = RuleActionsEnum.Sweep;

    public List<SweepDestination> Destinations { get; set; } = new List<SweepDestination>();

    /// <summary>
    /// The amount to leave in the account once the sweep has been processed.
    /// A value of zero means sweep all funds.
    /// </summary>
    public decimal AmountToLeave { get; set; }

    /// <summary>
    /// The minimum amount that must be available in order for the sweep to be run.
    /// For example, setting to 1000 means the rule will not execute if the funds
    /// available are less than 1000.
    /// </summary>
    public decimal MinimumAmountToRunAt { get; set; }

    /// <summary>
    /// The pattern to use for the Your Reference value when creating payouts based on the rule.
    /// </summary>
    public string? PayoutYourReference { get; set; }

    /// <summary>
    /// The pattern to use for the Their Reference value when creating payouts based on the rule.
    /// </summary>
    public string? PayoutTheirReference { get; set; }

    /// <summary>
    /// The pattern to use for the Description value when creating payouts based on the rule.
    /// </summary>
    public string? PayoutDescription { get; set; }

    public bool IsEmpty() => _isEmpty;

    public Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = new Dictionary<string, string>
        {
            { keyPrefix + nameof(Priority), Priority.ToString() },
            { keyPrefix + nameof(ActionType), ActionType.ToString() },
            { keyPrefix + nameof(AmountToLeave), AmountToLeave.ToString() },
            { keyPrefix + nameof(MinimumAmountToRunAt), MinimumAmountToRunAt.ToString() },
            { keyPrefix + nameof(PayoutYourReference), PayoutYourReference ?? string.Empty },
            { keyPrefix + nameof(PayoutTheirReference), PayoutTheirReference ?? string.Empty },
            { keyPrefix + nameof(PayoutDescription), PayoutDescription ?? string.Empty },
        };

        for (int i = 0; i < Destinations.Count(); i++)
        {
            var destination = Destinations[i];

            dict = dict.Concat(destination.ToDictionary(keyPrefix + nameof(Destinations) + $"[{i}]."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }

    /// <summary>
    /// The sweep action approval is only required if the destinations change. The approval hash
    /// does not need to take into account non-destination related fields.
    /// </summary>
    public string GetDestinationApprovalHash()
    {
        string input = string.Empty;

        foreach (var destination in Destinations)
        {
            input += destination.GetApprovalHash();
        }

        return input != string.Empty ? HashHelper.CreateHash(input) : string.Empty;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Destinations.Sum(x => x.SweepPercentage) > 100)
        {
            yield return new ValidationResult($"The sum of the percentages on the sweep destinations cannot exceed 100.",
                new string[] { nameof(Destinations) });
        }

        foreach (var dest in Destinations)
        {
            foreach (var err in dest.Validate(validationContext))
            {
                yield return err;
            }
        }
    }
}
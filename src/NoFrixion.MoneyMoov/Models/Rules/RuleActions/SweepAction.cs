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
    public static readonly SweepAction Empty = new SweepAction { _isEmpty = true };
    
    private bool _isEmpty = false;

    public int Priority { get; set; }

    public RuleActionsEnum ActionType { get; set; }

    public List<SweepDestination> Destinations { get; set; } = new List<SweepDestination>();

    public decimal AmountToLeave { get; set; }

    public bool IsEmpty() => _isEmpty;

    public Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = new Dictionary<string, string>
        {
            { keyPrefix + nameof(Priority), Priority.ToString() },
            { keyPrefix + nameof(ActionType), ActionType.ToString() },
            { keyPrefix + nameof(AmountToLeave), AmountToLeave.ToString() }
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

        foreach(var destination in Destinations)
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
    }
}
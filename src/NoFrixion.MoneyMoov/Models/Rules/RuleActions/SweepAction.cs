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

namespace NoFrixion.MoneyMoov.Models;

public class SweepAction 
{
    public static readonly SweepAction Empty = new SweepAction { _isEmpty = true };
    private bool _isEmpty;

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
}
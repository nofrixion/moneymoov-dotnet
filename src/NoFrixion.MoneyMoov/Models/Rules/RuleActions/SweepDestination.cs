//-----------------------------------------------------------------------------
// Filename: SweepDestination.cs
// 
// Description: A destination for a sweep rule that can sweep a percentage,
// or absolute amount, from an account to the destination account.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 Jan 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class SweepDestination : Counterparty
{
    /// <summary>
    /// The percentage of the total funds in the account when the rule is exectued
    /// that should be swept to the destination.
    /// </summary>
    public decimal SweepPercentage { get; set; }

    /// <summary>
    /// The amount that should be swept to the destination. Ignored if a sweep poercentage is set.
    /// </summary>
    public decimal SweepAmount { get; set; }

    public override Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = base.ToDictionary(keyPrefix);

        dict.Add(keyPrefix + nameof(SweepPercentage), SweepPercentage.ToString());
        dict.Add(keyPrefix + nameof(SweepAmount), SweepAmount.ToString());

        return dict;
    }

    public override string GetApprovalHash()
    {
        string input = 
            Math.Round(SweepPercentage, 2).ToString() +
            Math.Round(SweepAmount, 2).ToString() +
            base.GetApprovalHash();
        return HashHelper.CreateHash(input);
    }
}
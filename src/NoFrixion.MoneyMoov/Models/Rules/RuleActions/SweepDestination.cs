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

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.PayeeVerification;

namespace NoFrixion.MoneyMoov.Models;

public class SweepDestination : Counterparty, IValidatableObject
{
    /// <summary>
    /// The percentage of the total funds in the account when the rule is executed
    /// that should be swept to the destination.
    /// </summary>
    [Range(0, 100.00)]
    public decimal SweepPercentage { get; set; }

    /// <summary>
    /// The amount that should be swept to the destination. Ignored if a sweep percentage is set.
    /// </summary>
    public decimal SweepAmount { get; set; }

    /// <summary>
    /// The priority of the destination. Lower numbers represent higher priorities, e.g. 0 is the highest priority. 
    /// Relevant when there are multiple destinations in a sweep. The higher priority destinations will be paid first 
    /// and also attributed any remaining balance in the case of an uneven split.
    /// </summary>
    [Range(0, 1000)]
    public int Priority { get; set; }

    /// <summary>
    /// If set to true, it indicates that the destiation is disabled and should not be used.
    /// </summary>
    public bool IsDisabled { get; set; }

    public override Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = base.ToDictionary(keyPrefix);

        dict.Add(keyPrefix + nameof(SweepPercentage), SweepPercentage.ToString());
        dict.Add(keyPrefix + nameof(SweepAmount), SweepAmount.ToString());
        dict.Add(keyPrefix + nameof(Priority), Priority.ToString());

        return dict;
    }

    public override string GetApprovalHash()
    {
        string input =
            Math.Round(SweepPercentage, 2).ToString("0.##") +
            Math.Round(SweepAmount, 2).ToString("0.##") +
            Priority.ToString() +
            base.GetApprovalHash();
        return HashHelper.CreateHash(input);
    }
    
    public string GetBaseApprovalHash()
    {
        return base.GetApprovalHash();
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach(var err in base.Validate(validationContext))
        {
            yield return err;
        }

        if(SweepAmount > 0 && SweepPercentage > 0)
        {
            yield return new ValidationResult($"A sweep destination can only have one of {nameof(SweepAmount)} or {nameof(SweepPercentage)} greater than zero, not both.", 
                new string[] { nameof(SweepAmount), nameof(SweepPercentage) });
        }

        if (SweepAmount == 0 && SweepPercentage == 0)
        {
            yield return new ValidationResult($"A sweep destination must have one of {nameof(SweepAmount)} or {nameof(SweepPercentage)} greater than zero.",
                new string[] { nameof(SweepAmount), nameof(SweepPercentage) });
        }

        if (Identifier?.Currency == CurrencyTypeEnum.None)
        {
            yield return new ValidationResult($"The destination identifier currency must be set.",
                new string[] { nameof(Identifier.Currency) });
        }
    }
}
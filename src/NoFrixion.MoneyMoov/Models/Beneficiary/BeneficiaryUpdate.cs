// -----------------------------------------------------------------------------
//  Filename: BeneficiaryUpdate.cs
// 
//  Description: Class containing information for a beneficiary to be updated:
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 11 2023  Saurav Maiti   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class BeneficiaryUpdate
{
    /// <summary>
    /// ID of the accounts which are authorised to act as a source for the beneficiary.
    /// </summary>
    public List<Guid>? SourceAccountIDs { get; set; }

    /// <summary>
    /// The descriptive name for the beneficiary.
    /// </summary>
    public string? Name { get; set; }
    
    public CurrencyTypeEnum? Currency { get; set; }

    public Counterparty? Destination { get; set; }
    
    /// <summary>
    /// The default reference that will be used by default as TheirReference when creating payouts to this beneficiary
    /// if no TheirReference is specified for the payout.
    /// </summary>
    public string? TheirReference { get; set; }
    
    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem($"The {nameof(BeneficiaryUpdate)} had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Places all the beneficiary's properties into a dictionary. Useful for testing
    /// when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the beneficiary's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();
        
        if (Name != null)
        {
            dict.Add(nameof(Name), Name);
        }
        
        if(Currency != null)
        {
            dict.Add(nameof(Currency), Currency.Value.ToString());
        }

        if (Destination != null)
        {
            dict = dict.Concat(Destination.ToDictionary($"{nameof(Destination)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        if (SourceAccountIDs?.Count() > 0)
        {
            for (int i = 0; i<SourceAccountIDs.Count(); i++)
            {
                dict.Add($"{nameof(SourceAccountIDs)}[{i}]", SourceAccountIDs[i].ToString());
            }
        }

        return dict;
    }
}
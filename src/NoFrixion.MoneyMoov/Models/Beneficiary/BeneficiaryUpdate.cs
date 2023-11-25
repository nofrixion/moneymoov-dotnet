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
    /// ID of the account to which the beneficiary belongs.
    /// </summary>
    public Guid? AccountID { get; set; }

    /// <summary>
    /// The descriptive name for the beneficiary.
    /// </summary>
    public string? Name { get; set; }
    
    public CurrencyTypeEnum? Currency { get; set; }

    public Counterparty? Destination { get; set; }
    

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

        return dict;
    }

    public bool IsDestinationUpdated()
    {
        return Destination != null && Destination.Identifier != null && (Destination.Name != null || Destination.Identifier.IBAN != null || Destination.Identifier.AccountNumber != null || Destination.Identifier.BitcoinAddress != null || Destination.Identifier.SortCode != null);
    }
}
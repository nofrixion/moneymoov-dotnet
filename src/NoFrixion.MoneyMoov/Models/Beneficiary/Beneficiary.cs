// -----------------------------------------------------------------------------
//  Filename: Beneficiary.cs
// 
//  Description: Class containing information for a beneficiary:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  11 05 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class Beneficiary : IValidatableObject
{
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets the merchant id.
    /// </summary>
    
    public Guid MerchantID { get; set; }

    public Guid? AccountID { get; set; }

    /// <summary>
    /// The descriptive name for the beneficiary.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the currency.
    /// </summary>
    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    public Counterparty? Destination { get; set; }
    
    public string? ApprovalCallbackUrl { get; set; }
    
    public bool Enabled { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var payout = ToPayout();
        payout.Amount = 0.01M;
        // Use dummy valid values for references. Beneficiaries can have invalid references as
        // they will be forced to fix them when the payout is created.
        payout.TheirReference = "GoodRef";
        payout.YourReference = "GoodRef";
        return payout.Validate(validationContext);
    }

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        // Apply biz validation rules.
        var bizValidationResults = Validate(context);
        if(bizValidationResults.Count() > 0)
        {
            isValid = false;
            validationResults.AddRange(bizValidationResults);
        }

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem($"The {nameof(Beneficiary)} had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Maps the beneficiary to a Payout. Used for creating a new Payout and also validating the 
    /// Beneficiary.
    /// </summary>
    /// <returns>A new Payout object.</returns>
    public Payout ToPayout()
    {
        return new Payout
        {
            ID = Guid.NewGuid(),
            Type = Destination?.Identifier?.Type ?? AccountIdentifierType.Unknown,
            Currency = Currency,
            Destination = Destination
        };
    }
}
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

using LanguageExt.Pretty;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class Beneficiary : IValidatableObject
{
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets the merchant id.
    /// </summary>
    
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Gets or Sets the beneficiary name.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the beneficiary reference.
    /// </summary>
    [Required(ErrorMessage = "Your Reference is required.")]
    public string YourReference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Their Reference is required.")]
    public string TheirReference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destination Account Name is required.")]
    public string DestinationAccountName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the currency.
    /// </summary>
    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Gets or Sets the beneficiary IBAN.
    /// </summary>
    [Required(ErrorMessage = "Identifier is required.")]
    public AccountIdentifier? Identifier { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var payout = ToPayout();
        payout.Amount = 0.01M;
        return payout.Validate(validationContext);
    }

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem($"The {nameof(Beneficiary)} had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Places all the beneficiary's properties into a dictionary. Useful for testing
    /// when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the beneficiary's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(ID), ID.ToString() },
            { nameof(MerchantID), MerchantID.ToString() },
            { nameof(Name), Name },
            { nameof(YourReference), YourReference },
            { nameof(TheirReference), TheirReference },
            { nameof(DestinationAccountName), DestinationAccountName },
            { nameof(Currency), Currency.ToString() },
            { nameof(Identifier) + "." + nameof(Identifier.IBAN), Identifier?.IBAN ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.AccountNumber), Identifier?.AccountNumber ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.SortCode), Identifier?.SortCode ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.Type), Identifier?.Type.ToString() ?? string.Empty }
        };
    }

    /// <summary>
    /// Maps the benficiary to a Payout. Used for creating a new Payout and also validating the 
    /// Beneficiary.
    /// </summary>
    /// <returns>A new Payout object.</returns>
    public Payout ToPayout()
    {
        return new Payout
        {
            ID = Guid.NewGuid(),
            Type = Identifier?.Type ?? AccountIdentifierType.Unknown,
            Currency = Currency,
            YourReference = YourReference,
            TheirReference = TheirReference,
            DestinationAccountName = DestinationAccountName,
            DestinationAccountNumber = Identifier?.AccountNumber ?? string.Empty,
            DestinationIBAN = Identifier?.IBAN ?? string.Empty,
            DestinationSortCode = Identifier?.SortCode ?? string.Empty,
            DestinationAccount = new Counterparty
            {
                Name = DestinationAccountName,
                Identifier = new AccountIdentifier
                {
                    IBAN = Identifier?.IBAN ?? string.Empty,
                    SortCode = Identifier?.SortCode ?? string.Empty,
                    AccountNumber = Identifier?.AccountNumber ?? string.Empty,
                    Currency = Currency.ToString()
                }
            },
        };
    }
}
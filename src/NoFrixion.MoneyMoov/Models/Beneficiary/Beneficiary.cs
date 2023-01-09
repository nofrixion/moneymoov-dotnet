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
using System.Text.RegularExpressions;
using NoFrixion.MoneyMoov.Validators;

namespace NoFrixion.MoneyMoov.Models;

public class Beneficiary : IValidatableObject
{
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets the merchant id.
    /// </summary>
    [Required]
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Gets or Sets the beneficiary name.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or Sets the beneficiary reference.
    /// </summary>
    [Required(ErrorMessage = "Your Reference is required.")]
    [MaxLength(50, ErrorMessage = "Your Reference must be within 50 characters.")]
    [RegularExpression(@"[\w\s]*", ErrorMessage = "Your Reference can't have any special characters except underscore")]
    public string? YourReference { get; set; }

    [Required(ErrorMessage = "Their Reference is required.")]
    [MaxLength(18, ErrorMessage = "Your Reference must be within 50 characters.")]
    [MinLength(6, ErrorMessage = "Your Reference must be 6 characters or greater.")]
    public string? TheirReference { get; set; }

    [Required(ErrorMessage = "Destination Account Name is required.")]
    [RegularExpression(@"^([^\p{L}0-9]*?[\p{L}0-9]){1,}['\.\-\/&\s]*", ErrorMessage = "Destination Account Name can only include alpha-numeric characters and `.-/& and space. Example 'Test Account-0123'.")]
    [MaxLength(100, ErrorMessage = "Your Reference must be within 100 characters.")]
    public string? DestinationAccountName { get; set; }

    /// <summary>
    /// Gets or Sets the currency.
    /// </summary>
    [Required(ErrorMessage = "Currency is required.")]
    public string? Currency { get; set; }

    /// <summary>
    /// Gets or Sets the beneficiary IBAN.
    /// </summary>
    [Required(ErrorMessage = "Identifier is required.")]
    public AccountIdentifier? Identifier { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        Regex accountNameRegex = new Regex(@"^([^\p{L}0-9]*?[\p{L}0-9]){1,}['\.\-\/&\s]*");

        if (!PaymentsValidator.IsValidAccount(accountNameRegex, DestinationAccountName!))
        {
            yield return new ValidationResult($"Destination Account Name is invalid. Must match format '{accountNameRegex}'");
        }

        if (!PaymentsValidator.ValidateCurrency(Currency!))
        {
            yield return new ValidationResult("The Payout currency was not recognised. Currently only EUR and GBP are supported.");
        }

        if (Identifier?.Type == AccountIdentifierType.Unknown)
        {
            yield return new ValidationResult($"Please use either IBAN or Number and Sort code as destination");
        }

        if (Identifier?.Type == AccountIdentifierType.IBAN && !PaymentsValidator.ValidateIBAN(Identifier?.IBAN!))
        {
            yield return new ValidationResult("Destination IBAN is invalid, Please enter a valid IBAN.");
        }

        if (Identifier?.Type == AccountIdentifierType.IBAN && Currency != CurrencyTypeEnum.EUR.ToString())
        {
            yield return new ValidationResult($"Currency {Currency} does not support type {Identifier?.Type}");
        }

        if (Identifier?.Type == AccountIdentifierType.SCAN && Currency != CurrencyTypeEnum.GBP.ToString())
        {
            yield return new ValidationResult($"Currency {Currency} does not support type {Identifier?.Type}");
        }

        if (!PaymentsValidator.ValidateTheirReference(TheirReference!))
        {
            yield return new ValidationResult("Reference is invalid, all characters can not be the same.");
        }
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
            { nameof(Name), Name ?? string.Empty },
            { nameof(YourReference), YourReference ?? string.Empty },
            { nameof(TheirReference), TheirReference ?? string.Empty },
            { nameof(DestinationAccountName), DestinationAccountName ?? string.Empty },
            { nameof(Currency), Currency ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.IBAN), Identifier?.IBAN ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.AccountNumber), Identifier?.AccountNumber ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.SortCode), Identifier?.SortCode ?? string.Empty },
            { nameof(Identifier) + "." + nameof(Identifier.Type), Identifier?.Type.ToString() ?? string.Empty }
        };
    }
}
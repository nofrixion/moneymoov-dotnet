// -----------------------------------------------------------------------------
//  Filename: PayoutCreate.cs
// 
//  Description: Contains details of a payout request:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  05 Nov 2021  Arif Matin     Created, Carmichael House, Dublin, Ireland.
//  09 Nov 2021  Arif Matin     Changed Name to Payout.
//  10 Dec 2022  Aaron Clauson  Renamed to PayoutCreate.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

#nullable disable

namespace NoFrixion.MoneyMoov.Models;

public class PayoutCreate : IValidatableObject, IPayout
{
    public Guid ID { get; set; }

    [Required(ErrorMessage = "AccountID is required.")]
    public Guid AccountID { get; set; }

    public AccountIdentifierType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue,ErrorMessage ="Minimum value of 0.01 is required for Amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or Sets your reference ID
    /// </summary>
    [Required(ErrorMessage = "Your Reference is required.")]
    public string YourReference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Their Reference is required.")]
    public string TheirReference { get; set; } = string.Empty;

    public Guid DestinationAccountID { get; set; }

    public string DestinationIBAN { get; set; } = string.Empty;

    public string DestinationAccountNumber { get; set; } = string.Empty;

    public string DestinationSortCode { get; set; } = string.Empty;

    public string DestinationAccountName { get; set; } = string.Empty;

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        if (isValid)
        {
            // If the property validations passed, apply the overall business validation rules.
            var bizRulesvalidationResults = PayoutsValidator.Validate(this, context);
            if (bizRulesvalidationResults.Count() > 0)
            {
                isValid = false;
                validationResults ??= new List<ValidationResult>();
                validationResults.AddRange(bizRulesvalidationResults);
            }
        }

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem("The Payout create model had one or more validation errors.", validationResults);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return PayoutsValidator.Validate(this, validationContext);
    }

    /// <summary>
    /// Places all the payout's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payout's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(ID), ID.ToString() },
            { nameof(AccountID), AccountID.ToString() },
            { nameof(Type), Type.ToString() },
            { nameof(Description), Description },
            { nameof(Currency), Currency.ToString() },
            { nameof(Amount), Amount.ToString() },
            { nameof(YourReference), YourReference },
            { nameof(TheirReference), TheirReference },
            { nameof(DestinationAccountID), DestinationAccountID.ToString() },
            { nameof(DestinationIBAN), DestinationIBAN },
            { nameof(DestinationAccountNumber), DestinationAccountNumber },
            { nameof(DestinationSortCode), DestinationSortCode },
            { nameof(DestinationAccountName), DestinationAccountName },
        };
    }
}

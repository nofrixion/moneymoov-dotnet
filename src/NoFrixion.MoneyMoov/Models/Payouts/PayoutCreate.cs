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
    /// <summary>
    /// The ID for the payout.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets Account Id of sending account.
    /// </summary>
    [Required(ErrorMessage = "AccountID is required.")]
    public Guid AccountID { get; set; }

    /// <summary>
    /// Gets or Sets User ID of who created the payout request.
    /// </summary>
    public Guid UserID { get; set; }

    /// <summary>
    /// Sets the type of the destination account.
    /// </summary>
    [Required(ErrorMessage = "Type is required.")]
    public AccountIdentifierType? Type { get; set; }

    /// <summary>
    /// Gets or Sets description of payout request
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets Currency of payout request
    /// </summary>
    [Required(ErrorMessage = "Currency is required.")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets payout amount
    /// </summary>
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue,ErrorMessage ="Minimum value of 0.01 is required for Amount")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// Gets or Sets your reference ID
    /// </summary>
    [Required(ErrorMessage = "Your Reference is required.")]
    public string YourReference { get; set; }

    /// <summary>
    /// See <see cref="IPayout.DestinationAccount"/>
    /// </summary>
    public Counterparty DestinationAccount { get; set; }

    /// <summary>
    /// Gets or Sets destination reference ID
    /// </summary>
    [Required(ErrorMessage = "Their Reference is required.")]
    public string TheirReference { get; set; }

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
}

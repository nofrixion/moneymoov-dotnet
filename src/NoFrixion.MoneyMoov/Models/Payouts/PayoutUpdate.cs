// -----------------------------------------------------------------------------
//  Filename: PayoutUpdate.cs
// 
//  Description: Contains details of a payout update request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  22 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PayoutUpdate : IValidatableObject, IPayout
{
    public Guid ID { get; set; }

    public Guid AccountID { get; set; }

    public Guid UserID { get; set; }

    public AccountIdentifierType? Type { get; set; }

    public string? Description { get; set; }

    public string? Currency { get; set; }

    public decimal? Amount { get; set; }

    public string? YourReference { get; set; }

    public Counterparty? DestinationAccount { get; set; }

    public string? DestinationAccountID { get; set; }

    public string? DestinationIBAN { get; set; }

    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public string? DestinationAccountName { get; set; }

    public string? TheirReference { get; set; }

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
            new NoFrixionProblem("The Payout update model had one or more validation errors.", validationResults);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return PayoutsValidator.Validate(this, validationContext);
    }
}
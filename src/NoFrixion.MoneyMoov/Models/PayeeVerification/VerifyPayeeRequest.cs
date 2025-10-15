// -----------------------------------------------------------------------------
//  Filename: VerifyPayeeRequest.cs
// 
//  Description: Request to verify a payee
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  15 Oct 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.PayeeVerification;

public class VerifyPayeeRequest: IValidatableObject
{
    /// <summary>
    /// The name of the account to verify
    /// </summary>
    [Required(ErrorMessage = "Account name is required")]
    public string? AccountName { get; set; }
    
    /// <summary>
    /// The IBAN of the account to verify (for VoP checks)
    /// </summary>
    [Required(ErrorMessage = "IBAN is required")]
    public string? IBAN { get; set; }
    
    /// <summary>
    /// The sort code of the account to verify (for CoP checks)
    /// </summary>
    public string? SortCode { get; set; }
    
    /// <summary>
    /// The account number of the account to verify (for CoP checks)
    /// </summary>
    public string? AccountNumber { get; set; }
    
    /// <summary>
    /// Optional secondary identifier for the account to verify.
    /// It is usually the reason why the payment is being made or what invoice or obligation it relates to.
    /// Some responders may require this where just the identifier is not sufficient to uniquely identify the account.
    /// </summary>
    public string? SecondaryIdentification { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(AccountName))
        {
            yield return new ValidationResult("Account name is required", [nameof(AccountName)]);
        }

        if (string.IsNullOrWhiteSpace(IBAN))
        {
            yield return new ValidationResult("IBAN is required", [nameof(IBAN)]);
        }
    }
}
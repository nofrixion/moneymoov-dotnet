// -----------------------------------------------------------------------------
//  Filename: PayeeVerificationResult.cs
// 
//  Description: The result of a payee verification attempt.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  19 Sep 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models.PayeeVerification;

public class PayeeVerificationResult
{
    /// <summary>
    /// An optional reference that was passed in with the verification request
    /// </summary>
    public string? Reference { get; init; }
    
    /// <summary>
    /// The result of the payee verification
    /// </summary>
    public PayeeVerificationResultEnum Result { get; init; }
    
    /// <summary>
    /// The verified account name of the payee, if available (in case of a close match)
    /// </summary>
    public string? PayeeVerifiedAccountName { get; init; }
    
    /// <summary>
    /// The raw response from the supplier (e.g. Technoxander) as a JSON string, if available
    /// </summary>
    public string? SupplierRawResponse { get; set; }
    
    public string? SupplierStatus { get; set; }
}
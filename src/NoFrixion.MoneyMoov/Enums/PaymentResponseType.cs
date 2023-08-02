// -----------------------------------------------------------------------------
//  Filename: PaymentResponseType.cs
// 
//  Description: An enum to help determine the response type in case of 
//  creating and processing payment request at the same time.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  29 11 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum PaymentResponseType
{
    /// <summary>
    /// Default response type when it is not set explicitly.
    /// </summary>
    None,

    /// <summary>
    /// Response type when card payer authentication is setup.
    /// </summary>
    CardPayerAuthenticationSetupResponse,
    
    /// <summary>
    /// Response type after submitting a card payment.
    /// </summary>
    CardPaymentResponse,

    /// <summary>
    /// Response type after submitting PISP payment.
    /// </summary>
    PaymentInitiationResponse
}
// -----------------------------------------------------------------------------
//  Filename: PaymentRequestAuthorization.cs
// 
//  Description: Represents a pisp payment authorization for a payment request.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  07 02 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestAuthorization
{
    /// <summary>
    /// The ID of the payment request the pisp authorization is for.
    /// </summary>
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// Timestamp the pisp authorization occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// The payment type for the received money.
    /// </summary>
    public PaymentMethodTypeEnum PaymentMethod { get; set; } = PaymentMethodTypeEnum.pisp;

    /// <summary>
    /// The authorised payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The authorised payment currency.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// For a payment initiation this is the ID returned by the service provider initiating
    /// the payment for us.
    /// </summary>
    public string? PispPaymentInitiationID { get; set; }
}
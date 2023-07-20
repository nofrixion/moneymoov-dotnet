//-----------------------------------------------------------------------------
// Filename: PaymentRequestPayment.cs
//
// Description: Represents a payment received for a payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Mar 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestPayment
{
    /// <summary>
    /// The ID of the payment request the result is for.
    /// </summary>
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// Timestamp the payment occurred. For cards this will be the time the
    /// original authorisation occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// The payment type for the received money.
    /// </summary>
    public PaymentMethodTypeEnum PaymentMethod { get; set; }

    /// <summary>
    /// The authorised payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The authorised payment currency.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// For card payments the merchant can request a reusable token for this payer and
    /// use it to submit subsequent merchant initiated payments.
    /// </summary>
    public string? TokenisedCardID { get; set; }

    /// <summary>
    /// For card payments this is the ID from the initial successful authorization or sale.
    /// Required for voids and capture operations.
    /// </summary>
    public string? CardAuthorizationID { get; set; }

    /// <summary>
    /// The captured amount for a card payment.
    /// </summary>
    public decimal CardCapturedAmount { get; set; }

    /// <summary>
    /// If true indicates that the card payment was voided.
    /// </summary>
    public bool CardIsVoided { get; set; }

    /// <summary>
    /// The card processor that was used for the payment event.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.None;

    /// <summary>
    /// Refunded Amount
    /// </summary>
    public decimal RefundedAmount { get; set; }
}

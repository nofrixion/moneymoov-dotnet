//-----------------------------------------------------------------------------
// Filename: PaymentRequestPaymentAttempt.cs
//
// Description: Represents a distinct payment attempt for a payment request. An
// example of an attempt would be a Pay by Bank (PIS), card payment etc. Attempts
// will be collated from 1 or more payment request events and are a grouping
// of events.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 17 May 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestPaymentAttempt
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
    /// The card processor that was used for the payment event.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.None;

    /// <summary>
    /// For pay by bank attempts this is the ID that gets set on all the events (initiate,
    /// callback, webhoo &, settlement) for the same attempt.
    /// </summary>
    public string? PispPaymentInitiationID { get; set; }
}

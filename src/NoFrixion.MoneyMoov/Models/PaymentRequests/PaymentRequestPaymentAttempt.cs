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
    /// For pay by bank attempts this is the ID that gets set on all the events (initiate,
    /// callback, webhook and settlement) for the same attempt. For cards and lightning different
    /// fields are used to group payment request events.
    /// </summary>
    public string AttemptKey { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the payment request the result is for.
    /// </summary>
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// Timestamp the payment was initiated at.
    /// </summary>
    public DateTimeOffset InitiatedAt { get; set; }

    /// <summary>
    /// If the attempt was authorised this is the timestamp it occurred at.
    /// </summary>
    public DateTimeOffset? AuthorisedAt { get; set; }

    /// <summary>
    /// If the attempt was settled this is the timestamp it occurred at.
    /// </summary>
    public DateTimeOffset? SettledAt { get; set; }

    /// <summary>
    /// If the attempt failed to settled after the expected settlement time this
    /// is the timestamp the failure was recorded at.
    /// </summary>
    public DateTimeOffset? SettleFailedAt { get; set; }

    /// <summary>
    /// The payment type for the received money.
    /// </summary>
    public PaymentMethodTypeEnum PaymentMethod { get; set; }

    /// <summary>
    /// The payment amount attempted.
    /// </summary>
    public decimal AttemptedAmount { get; set; }

    /// <summary>
    /// The payment amount that was authorised by the payer.
    /// </summary>
    public decimal AuthorisedAmount { get; set; }

    /// <summary>
    /// The funds that were received from the payer.
    /// </summary>
    public decimal SettledAmount { get; set; }

    /// <summary>
    /// The refund attempts associated with this payment attempt.
    /// </summary>
    public List<PaymentRequestRefundAttempt> RefundAttempts { get; set; } = new List<PaymentRequestRefundAttempt>();

    /// <summary>
    /// The capture attempts associated with this payment attempt.
    /// </summary>
    public List<PaymentRequestCaptureAttempt> CaptureAttempts { get; set; } = new();

    /// <summary>
    /// The authorised payment currency.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// The card processor that was used for the payment event.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.None;

    public WalletsEnum? WalletName { get; set; }

    /// <summary>
    /// Where available this is the ID of the institution used by the payer. For example in PIS attempts
    /// this will be the ID of the bank the payer used for the attempt.
    /// </summary>
    public string? InstitutionID { get; set; }

    /// <summary>
    /// Where available this is the name of the institution used by the payer. For example,
    /// in PIS attempts this will be the name of the bank the payer used for the attempt.
    /// </summary>
    public string? InstitutionName { get; set; }

    public PaymentResultEnum Status
    {
        get
        {
            return this switch
            {
                var att when att.SettledAmount > 0 && att.SettledAmount == att.AttemptedAmount => PaymentResultEnum.FullyPaid,
                var att when att.SettledAmount > 0 && att.SettledAmount > att.AttemptedAmount => PaymentResultEnum.OverPaid,
                var att when att.SettledAmount > 0 && att.SettledAmount < att.AttemptedAmount => PaymentResultEnum.PartiallyPaid,
                var att when att.SettleFailedAt != null => PaymentResultEnum.None,
                var att when att.AuthorisedAt != null => PaymentResultEnum.Authorized,
                _ => PaymentResultEnum.None
            };
        }
    }
}


public class PaymentRequestRefundAttempt
{
    public Guid? RefundPayoutID { get; set; }

    public DateTimeOffset? RefundInitiatedAt { get; set; }

    public DateTimeOffset? RefundSettledAt { get; set; }

    public DateTimeOffset? RefundCancelledAt { get; set; }

    public decimal RefundInitiatedAmount { get; set; }

    public decimal RefundSettledAmount { get; set; }

    public decimal RefundCancelledAmount { get; set; }
}

/// <summary>
/// Represents each individual payment capture attempt for a payment request.
/// </summary>
public class PaymentRequestCaptureAttempt
{
    /// <summary>
    /// Date and time the capture was initiated.
    /// </summary>
    public DateTimeOffset? CapturedAt { get; set; }
    
    /// <summary>
    /// The amount that was captured.
    /// </summary>
    public decimal CapturedAmount { get; set; }
}
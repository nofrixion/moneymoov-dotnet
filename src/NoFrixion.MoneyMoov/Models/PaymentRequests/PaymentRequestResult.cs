//-----------------------------------------------------------------------------
// Filename: PaymentRequestResult.cs
//
// Description: Represents the result of a payment request attempt. The result
// is determined from the events that have been recorded for a payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Dec 2021  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestResult
{
    /// <summary>
    /// The status returned from Plaid when a PIS payment is successfully initiated in production.
    /// </summary>
    /// <remarks>
    /// Plaid description from https://plaid.com/docs/api/products/payment-initiation/#payment_initiationconsentpaymentexecute:
    /// PAYMENT_STATUS_INITIATED: The payment has been successfully authorised and 
    /// accepted by the financial institution but has not been executed.
    /// </remarks>
    public const string PISP_PLAID_INITIATED_STATUS = "PaymentStatusInitiated";

    /// <summary>
    /// An alternative status returned by Plaid when a PIS payment is successfully initiated in production.
    /// </summary>
    /// <remarks>
    /// Plaid description from https://plaid.com/docs/api/products/payment-initiation/#payment_initiationconsentpaymentexecute:
    /// PAYMENT_STATUS_INPUT_NEEDED: This is the initial state of all payments. It indicates that the payment is waiting on user input to continue processing. 
    /// A payment may re-enter this state later on if further input is needed.
    /// </remarks>
    public const string PISP_PLAID_INPUT_NEEDED_STATUS = "PaymentStatusInputNeeded";

    /// <summary>
    /// The status returned from Plaid when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_PLAID_SUCCESS_STATUS = "PaymentStatusExecuted";

    /// <summary>
    /// The status returned from Modulr when a PIS payment is successfully initiated in the sandbox
    /// environment.
    /// </summary>
    public const string PISP_MODULR_SUCCESS_STATUS = "EXECUTED";

    /// <summary>
    /// The status returned from Yapily when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_YAPILY_COMPLETED_STATUS = "COMPLETED";

    /// <summary>
    /// The status returned from Yapily when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_YAPILY_PENDING_STATUS = "PENDING";

    /// <summary>
    /// The ID of the payment request the result is for.
    /// </summary>
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// The authorised payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The authorised payment currency.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// The result of the payment attempt.
    /// </summary>
    public PaymentResultEnum Result { get; set; } = PaymentResultEnum.None;

    /// <summary>
    /// The full original payment amount requested.
    /// </summary>
    public decimal RequestedAmount { get; set; }

    /// <summary>
    /// The list of payment attempts that have been received for the payment request.
    /// </summary>
    public List<PaymentRequestPayment> Payments { get; set; }

    public PaymentRequestResult()
    {
        Payments = new List<PaymentRequestPayment>();
    }

    public PaymentRequestResult(PaymentRequest paymentRequest)
    {
        PaymentRequestID = paymentRequest.ID;
        RequestedAmount = paymentRequest.Amount;
        Payments = new List<PaymentRequestPayment>();

        if(paymentRequest != null &&
           paymentRequest.Events != null &&
           paymentRequest.Events.Count() > 0)
        {
            var orderedEvents = paymentRequest.Events.OrderByDescending(x => x.Inserted);

            foreach(var payEvent in orderedEvents)
            {
                if(payEvent.EventType == PaymentRequestEventTypesEnum.pisp_callback &&
                    (payEvent.Status == PISP_PLAID_SUCCESS_STATUS ||
                     payEvent.Status == PISP_PLAID_INITIATED_STATUS ||
                     payEvent.Status == PISP_PLAID_INPUT_NEEDED_STATUS ||
                     payEvent.Status == PISP_MODULR_SUCCESS_STATUS ||
                     payEvent.Status == PISP_YAPILY_COMPLETED_STATUS ||
                     payEvent.Status == PISP_YAPILY_PENDING_STATUS))
                {
                    // Successfully authorised payment initiation.
                    Payments.Add(
                        new PaymentRequestPayment
                        {
                            PaymentRequestID = PaymentRequestID,
                            OccurredAt = payEvent.Inserted,
                            PaymentMethod = PaymentMethodTypeEnum.pisp,
                            Amount = Math.Round(payEvent.Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                            Currency = payEvent.Currency
                        });
                }
                else if(payEvent.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid)
                {
                    Payments.Add(
                        new PaymentRequestPayment
                        {
                            PaymentRequestID = PaymentRequestID,
                            OccurredAt = payEvent.Inserted,
                            PaymentMethod = PaymentMethodTypeEnum.lightning,
                            Amount = payEvent.Amount,
                            Currency = payEvent.Currency
                        });
                }
                else if(
                    (payEvent.EventType == PaymentRequestEventTypesEnum.card_authorization ||
                    payEvent.EventType == PaymentRequestEventTypesEnum.card_sale) &&
                    (payEvent.Status == CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS || 
                    payEvent.Status == CardPaymentResponseStatus.CARD_PAYMENT_SOFT_DECLINE_STATUS ||
                    payEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS))
                {
                    decimal amount = Math.Round(payEvent.Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES);

                    // The captured amount can be different to the authorised amount. Card payments can be 
                    // authorised AND settled, in which case the full amount is captured. They can be authorised
                    // only in which case the amount is not settled and a subsequent "capture" call is required.
                    decimal capturedAmount = payEvent switch
                    {
                        var p when p.EventType == PaymentRequestEventTypesEnum.card_sale &&
                            payEvent.Status == CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS => amount,
                        var p when p.EventType == PaymentRequestEventTypesEnum.card_sale &&
                            payEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS => amount,
                        _ => 0,
                    };

                    // Initial authorisation for a card payment.
                    var payment = new PaymentRequestPayment
                    {
                        PaymentRequestID = PaymentRequestID,
                        OccurredAt = payEvent.Inserted,
                        PaymentMethod = PaymentMethodTypeEnum.card,
                        Amount = amount,
                        Currency = payEvent.Currency,
                        CardCapturedAmount = capturedAmount,
                        CardAuthorizationID = payEvent.CardAuthorizationResponseID,
                        CardTokenCustomerID = payEvent.CardTokenCustomerID,
                        CardTransactionID = payEvent.CardTransactionID,
                        PaymentProcessor = payEvent.PaymentProcessorName
                    };

                    var relatedCardEvents = orderedEvents.Where(x => x.CardAuthorizationResponseID == payEvent.CardAuthorizationResponseID);
                    foreach (var relatedEvent in relatedCardEvents)
                    {
                        if (relatedEvent.EventType == PaymentRequestEventTypesEnum.card_void)
                        {
                            payment.CardIsVoided = true;
                            payment.Amount = 0;
                            payment.CardCapturedAmount = 0;
                            break;
                        }
                        else if (relatedEvent.EventType == PaymentRequestEventTypesEnum.card_capture &&
                            (relatedEvent.Status == CardPaymentResponseStatus.CARD_CAPTURE_SUCCESS_STATUS ||
                            relatedEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS))
                        {
                            payment.CardCapturedAmount += Math.Round(relatedEvent.Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES);
                        }
                    }

                    Payments.Add(payment);
                }
            }

            Currency = paymentRequest.Currency;
            Amount = Payments.Where(x => x.Currency == Currency).Sum(x => x.Amount);
            Result = Amount switch
            {
                _ when Amount == paymentRequest.Amount => PaymentResultEnum.FullyPaid,
                _ when Amount > paymentRequest.Amount => PaymentResultEnum.OverPaid,
                _ when paymentRequest.Amount == 0 => PaymentResultEnum.FullyPaid,
                _ when Amount > 0 && Amount < paymentRequest.Amount => PaymentResultEnum.PartiallyPaid,
                _ when Payments.Count > 0 && Payments.All(x => x.CardIsVoided) => PaymentResultEnum.Voided,
                _ => PaymentResultEnum.None
            };
        }
    }

    public decimal AmountOutstanding() =>
        Currency switch
        {
            CurrencyTypeEnum.BTC or CurrencyTypeEnum.LBTC => RequestedAmount - Amount,
            _ => Math.Round(RequestedAmount - Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES)
        };

    /// <summary>
    /// Applies a cap on the amount a partial payment can be for to avoid exceeding
    /// the total payment amount for the payment request.
    /// </summary>
    /// <param name="amountRequested">The partial payment amount being requested.</param>
    /// <returns>A corrected payment amount.</returns>
    public decimal GetCappedPartialAmount(decimal amountRequested)
    {
        if (amountRequested > decimal.Zero)
        {
            decimal outstandingAmount = RequestedAmount - Amount;
            return amountRequested > outstandingAmount ? outstandingAmount : amountRequested;
        }
        else
        {
            return RequestedAmount - Amount;
        }
    }
}
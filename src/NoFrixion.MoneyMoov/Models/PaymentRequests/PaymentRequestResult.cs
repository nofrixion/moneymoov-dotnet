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

using NoFrixion.MoneyMoov.Models.PaymentRequests;

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
    /// The status returned from Plaid when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_PLAID_SUCCESS_STATUS = "PaymentStatusExecuted";

    /// <summary>
    /// The status returned from Modulr when a PIS payment is successfully initiated in the sandbox
    /// environment.
    /// </summary>
    public const string PISP_MODULR_SUCCESS_STATUS = "EXECUTED";

    /// <summary>
    /// With Modulr responses an additional AspspPaymentStatus is returned along with the main Status.
    /// Both need to be checked. Other than rejected all other statuses indicate the payment attempt
    /// is in progress.
    /// </summary>
    /// <remarks>
    /// See https://modulr.readme.io/docs/single-immediate-payments-overview#payment-initiation-request-lifecycle
    /// </remarks>
    public const string PISP_MODULR_BANK_REJECTED_STATUS = "Rejected";

    /// <summary>
    /// The status returned from Yapily when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_YAPILY_COMPLETED_STATUS = "COMPLETED";

    /// <summary>
    /// The status returned from Yapily when a PIS payment is successfully initiated.
    /// </summary>
    public const string PISP_YAPILY_PENDING_STATUS = "PENDING";

    /// <summary>
    /// The status returned in the Yapily callback for a payment authorisation error. 
    /// </summary>
    public const string PISP_YAPILY_AUTHORISATION_ERROR = "payment_authorisation_error";

    /// <summary>
    /// NoFrixion payment initiation authorisation error status.
    /// </summary>
    public const string PISP_NOFRIXION_AUTHORISATION_ERROR = "payment_error";

    /// <summary>
    /// Modulr payment initiation authorisation error status.
    /// </summary>
    public const string PISP_MODULR_AUTHORISATION_ERROR = "CONSENT_REJECTED";

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

    public List<PaymentRequestAuthorization> PispAuthorizations { get; set; }
    
    public decimal AmountReceived { get; set; }
    
    public decimal AmountRefunded { get; set; }
    
    public decimal AmountPending { get; set; }

    /// <summary>
    /// The customer id
    /// </summary>
    public string? CustomerID { get; set; }

    public PaymentRequestResult()
    {
        Payments = new List<PaymentRequestPayment>();
        PispAuthorizations = new List<PaymentRequestAuthorization>();
    }

    public PaymentRequestResult(PaymentRequest paymentRequest)
    {
        PaymentRequestID = paymentRequest.ID;
        RequestedAmount = paymentRequest.Amount;
        Payments = new List<PaymentRequestPayment>();
        PispAuthorizations = new List<PaymentRequestAuthorization>();
        CustomerID = paymentRequest.CustomerID;

        var paymentAttempts = paymentRequest.PaymentAttempts;

        if (paymentRequest != null &&
           paymentRequest.Events != null &&
           paymentRequest.Events.Count() > 0)
        {
            AmountReceived = paymentRequest.GetTotalAmountReceived();
            
            AmountRefunded = paymentRequest.GetTotalAmountRefunded();
            
            AmountPending = paymentRequest.GetTotalAmountPending();
            
            var orderedEvents = paymentRequest.Events.OrderByDescending(x => x.Inserted);

            foreach (var payEvent in orderedEvents)
            {
                if (payEvent.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid)
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
            }

            foreach (var attempt in paymentAttempts.Where(x => x.PaymentMethod == PaymentMethodTypeEnum.card))
            {
                if (attempt.CardAuthorisedAmount > 0)
                {
                    Payments.Add(new PaymentRequestPayment
                    {
                        PaymentRequestID = PaymentRequestID,
                        OccurredAt = attempt.InitiatedAt,
                        PaymentMethod = PaymentMethodTypeEnum.card,
                        Amount = Math.Round(attempt.CardAuthorisedAmount,
                            PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                        Currency = attempt.Currency,
                        CardCapturedAmount = Math.Round(attempt.CaptureAttempts.Sum(x => x.CapturedAmount),
                            PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                        CardAuthorizationID = attempt.AttemptKey,
                        TokenisedCardID = attempt.TokenisedCardID,
                        PaymentProcessor = attempt.PaymentProcessor,
                        RefundedAmount = Math.Round(attempt.RefundAttempts.Sum(x => x.RefundSettledAmount),
                            PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                        CardIsVoided =
                            attempt.CardAuthorisedAmount - attempt.RefundAttempts.Sum(x => x.RefundSettledAmount) == 0
                    });
                }
            }

            foreach (var attempt in paymentAttempts.Where(x => x.PaymentMethod == PaymentMethodTypeEnum.pisp))
            {
                if (attempt.Status == PaymentResultEnum.FullyPaid ||
                    attempt.Status == PaymentResultEnum.PartiallyPaid ||
                    attempt.Status == PaymentResultEnum.OverPaid)
                {
                    Payments.Add(
                        new PaymentRequestPayment
                        {
                            PaymentRequestID = PaymentRequestID,
                            OccurredAt = attempt.InitiatedAt,
                            PaymentMethod = PaymentMethodTypeEnum.pisp,
                            Amount = Math.Round(attempt.SettledAmount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                            Currency = attempt.Currency,
                            RefundedAmount = Math.Round(attempt.RefundAttempts.Sum(x => x.RefundSettledAmount), PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                        });
                }
                else if (attempt.Status == PaymentResultEnum.Authorized)
                {
                    PispAuthorizations.Add(
                            new PaymentRequestAuthorization
                            {
                                PaymentRequestID = PaymentRequestID,
                                OccurredAt = attempt.InitiatedAt,
                                PaymentMethod = PaymentMethodTypeEnum.pisp,
                                Amount = Math.Round(attempt.AuthorisedAmount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES),
                                Currency = attempt.Currency,
                                PispPaymentInitiationID = attempt.AttemptKey
                            });
                }
            }

            Currency = paymentRequest.Currency;
            Amount = Payments.Where(x => x.Currency == Currency).Sum(x => x.Amount)
                     - Payments.Where(x => x.Currency == Currency).Sum(x => x.RefundedAmount); // Refunds are negative payments.
            Result = Amount switch
            {
                _ when Amount == paymentRequest.Amount => PaymentResultEnum.FullyPaid,
                _ when Amount > paymentRequest.Amount => PaymentResultEnum.OverPaid,
                _ when Amount > 0 && Amount < paymentRequest.Amount => PaymentResultEnum.PartiallyPaid,
                _ when Amount == 0 && PispAmountAuthorized() > 0 => PaymentResultEnum.Authorized,
                _ when Payments.Count > 0 && Payments.All(x => x.CardIsVoided) => PaymentResultEnum.Voided,
                _ => PaymentResultEnum.None
            };
        }
    }

    /// <summary>
    /// Returns the amount that has been authorised in pisp attempts but has not yet settled.
    /// </summary>
    /// <returns></returns>
    public decimal PispAmountAuthorized() =>
        PispAuthorizations.Where(x => x.Currency == Currency).Sum(x => x.Amount);

    public decimal AmountOutstanding()
    {
        var outstanding = Currency switch
        {
            CurrencyTypeEnum.BTC => RequestedAmount - Amount,
            _ => Math.Round(RequestedAmount - (Amount + PispAmountAuthorized()), PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES)
        };

        return outstanding >= 0 ? outstanding : 0;
    }

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
            decimal outstandingAmount = this.AmountOutstanding();
            return amountRequested > outstandingAmount ? outstandingAmount : amountRequested;
        }
        else
        {
            return this.AmountOutstanding();
        }
    }
}

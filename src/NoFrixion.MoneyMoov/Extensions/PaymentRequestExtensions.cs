//  -----------------------------------------------------------------------------
//   Filename: PaymentRequestExtensions.cs
// 
//   Description: Contains extension methods for the PaymentRequest class.:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   09 06 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class PaymentRequestExtensions
{
    public static List<PaymentRequestPaymentAttempt> GetPaymentAttempts(this IEnumerable<PaymentRequestEvent> events)
    {
        if (events == null || !events.Any())
        {
            return new List<PaymentRequestPaymentAttempt>();
        }
        else
        {
            var paymentAttempts = new List<PaymentRequestPaymentAttempt>();

            paymentAttempts.AddRange(GetCardPaymentAttempts(events));
            paymentAttempts.AddRange(GetPispPaymentAttempts(events));
            paymentAttempts.AddRange(GetLightningPaymentAttempts(events));

            return paymentAttempts;
        }
    }

    /// <summary>
    /// Groups the payment request events into a list of payment attempts for Card payments.
    /// Card events flow:
    /// 
    /// Cybersource flow → 
    /// Card Sale:
    /// card_payer_authentication_setup → card_authorization → card_sale → card_void
    /// Card Capture:
    /// card_payer_authentication_setup → card_authorization → card_capture → card_void
    /// 
    /// Checkout flow → 
    /// Card Sale:
    /// card_payer_authentication_setup → card_sale → card_void
    /// Card Capture:
    /// card_payer_authentication_setup → card_authorization → card_capture → card_void
    ///
    /// Card events statuses:
    ///
    /// card_payer_authentication_setup:
    /// Checkout - Pending
    /// Cybersource - COMPLETED
    /// card_payer_authentication_setup_failure:
    /// Checkout - Event itself means 3Dsecure failure. Can be considered a failed attempt.
    /// card_authorization:
    /// Cybersource - AUTHORIZED, 202 (Soft decline)
    /// Checkout - Authorized, CardVerified (In case of checkout, the card_authorization event is logged only in case of Authorize only flag enabled on the payment request. Otherwise, a successful payment, logs a card_sale event)
    /// card_sale:
    /// Cybersource - AUTHORIZED, 202 (Soft decline)
    /// Checkout - AUTHORIZED
    /// card_capture:
    /// Cybersource - PENDING
    /// Checkout - CAPTURED
    /// card_void:
    /// Cybersource - VOIDED
    /// Checkout - VOIDED
    /// </summary>
    /// <returns>A list of payment attempts generated from payment request events.</returns>
    public static IEnumerable<PaymentRequestPaymentAttempt> GetCardPaymentAttempts(
        this IEnumerable<PaymentRequestEvent> events)
    {
        var cardPaymentAttempts = new List<PaymentRequestPaymentAttempt>();

        var cardAttempts = events.GetGroupedCardEvents();

        foreach (var attempt in cardAttempts)
        {
            var paymentAttempt = new PaymentRequestPaymentAttempt();

            attempt.HandleCardAuthorisationEvents(paymentAttempt);

            attempt.HandleCardCaptureEvents(paymentAttempt);

            attempt.HandleCardSaleEvents(paymentAttempt);

            attempt.HandleCardWebhookEvents(paymentAttempt);

            // If there is a card void event, then the payment attempt was refunded.
            attempt.HandleCardVoidEvents(paymentAttempt);
            
            attempt.HandleCardRefundEvents(paymentAttempt);

            attempt.SetWalletName(paymentAttempt);

            cardPaymentAttempts.Add(paymentAttempt);
        }

        // Handle failed auth setup events
        var failedCardAttempts = events
            .Where(e =>
                e.EventType == PaymentRequestEventTypesEnum.card_payer_authentication_failure ||
                (e.EventType == PaymentRequestEventTypesEnum.card_payer_authentication_setup &&
                 (!string.IsNullOrWhiteSpace(e.ErrorMessage) ||
                  !string.IsNullOrWhiteSpace(e.ErrorReason) ||
                  (e.Status != CardPaymentResponseStatus.CARD_PAYER_AUTHENTICATION_SETUP_COMPLETE &&
                   e.Status != CardPaymentResponseStatus.CARD_CAPTURE_SUCCESS_STATUS))));

        foreach (var failedCardAttempt in failedCardAttempts)
        {
            var failedAttempt = new PaymentRequestPaymentAttempt();
            failedAttempt.AttemptKey = failedCardAttempt.CardRequestID ?? string.Empty;
            failedAttempt.PaymentRequestID = failedCardAttempt.PaymentRequestID;
            failedAttempt.InitiatedAt = failedCardAttempt.Inserted;
            failedAttempt.PaymentMethod = PaymentMethodTypeEnum.card;
            failedAttempt.AttemptedAmount = failedCardAttempt.Amount;
            failedAttempt.Currency = failedCardAttempt.Currency;
            failedAttempt.PaymentProcessor = failedCardAttempt.PaymentProcessorName;
            failedAttempt.CardPayerAuthenticationSetupFailedAt = failedCardAttempt.Inserted;

            cardPaymentAttempts.Add(failedAttempt);
        }

        return cardPaymentAttempts;
    }

    /// <summary>
    /// Groups the payment request events into a list of payment attempts for PISP payments.
    /// </summary>
    /// <returns>A list of payment initiation attempts for the payment request events.</returns>
    public static IEnumerable<PaymentRequestPaymentAttempt> GetPispPaymentAttempts(this IEnumerable<PaymentRequestEvent> events)
    {
        var pispPaymentAttempts = new List<PaymentRequestPaymentAttempt>();
        // Get PIS attempts.
        var pispAttempts = events.Where(x => !string.IsNullOrEmpty(x.PispPaymentInitiationID) &&
                                                           ((x.EventType == PaymentRequestEventTypesEnum.pisp_initiate) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_callback) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_webhook) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_settle) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_settle_failure) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_refund_initiated) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.pisp_refund_settled)))
            .OrderBy(x => x.Inserted)
            .GroupBy(x => x.PispPaymentInitiationID)
            .ToList();

        foreach (var attempt in pispAttempts)
        {
            // The pisp_initiate event should always be present but if for some reason it's not the next best event
            // will be used as the starting point for the attempt.
            var initiateEvent =
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_initiate).FirstOrDefault() ??
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_callback).FirstOrDefault() ??
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_webhook).FirstOrDefault() ??
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle).FirstOrDefault();

            if (initiateEvent != null)
            {
                var paymentAttempt = new PaymentRequestPaymentAttempt
                {
                    AttemptKey = attempt.Key ?? string.Empty,
                    PaymentRequestID = initiateEvent.PaymentRequestID,
                    InitiatedAt = initiateEvent.Inserted,
                    PaymentMethod = PaymentMethodTypeEnum.pisp,
                    Currency = initiateEvent.Currency,
                    AttemptedAmount = initiateEvent.Amount,
                    PaymentProcessor = initiateEvent.PaymentProcessorName,
                    InstitutionID = initiateEvent.PispPaymentServiceProviderID,
                    InstitutionName = initiateEvent.PispPaymentInstitutionName
                };

                foreach (var pispCallbackOrWebhook in attempt.Where(x =>
                    x.EventType == PaymentRequestEventTypesEnum.pisp_callback ||
                    x.EventType == PaymentRequestEventTypesEnum.pisp_webhook))
                {
                    var authorisationEvent = pispCallbackOrWebhook switch
                    {
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Modulr
                            && cbk.Status == PaymentRequestResult.PISP_MODULR_SUCCESS_STATUS
                            && cbk.PispBankStatus != PaymentRequestResult.PISP_MODULR_BANK_REJECTED_STATUS => cbk,
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Nofrixion
                        && (cbk.Status == PayoutStatus.QUEUED.ToString() ||
                            cbk.Status == PayoutStatus.QUEUED_UPSTREAM.ToString() ||
                            cbk.Status == PayoutStatus.PENDING.ToString() ||
                            cbk.Status == PayoutStatus.PROCESSED.ToString()) => cbk,
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Plaid
                            && (cbk.Status == PaymentRequestResult.PISP_PLAID_INITIATED_STATUS ||
                                cbk.Status == PaymentRequestResult.PISP_PLAID_SUCCESS_STATUS) => cbk,
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Yapily
                            && (cbk.Status == PaymentRequestResult.PISP_YAPILY_PENDING_STATUS ||
                               cbk.Status == PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS) => cbk,
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Nofrixion
                            && cbk.Status == PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS => cbk,
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.Simulator
                            && cbk.Status == PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS => cbk,
                        _ => null
                    };

                    if (authorisationEvent != null)
                    {
                        paymentAttempt.AuthorisedAt = authorisationEvent.Inserted;
                        paymentAttempt.AuthorisedAmount = authorisationEvent.Amount;
                        break;
                    }
                }

                if (attempt.Any(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle))
                {
                    var settleEvent = attempt.First(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle);

                    paymentAttempt.SettledAt = settleEvent.Inserted;
                    paymentAttempt.SettledAmount = settleEvent.Amount;
                    paymentAttempt.ReconciledTransactionID = settleEvent.ReconciledTransactionID;

                    paymentAttempt.RefundAttempts = GetPispRefundAttempts(events, settleEvent.PispPaymentInitiationID!).ToList();
                }
                else if (attempt.Any(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle_failure))
                {
                    var settleFailedEvent = attempt.First(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle_failure);

                    paymentAttempt.SettleFailedAt = settleFailedEvent.Inserted;
                }

                pispPaymentAttempts.Add(paymentAttempt);
            }
        }

        return pispPaymentAttempts;
    }

    private static IEnumerable<PaymentRequestRefundAttempt> GetPispRefundAttempts(
        this IEnumerable<PaymentRequestEvent> events, string pispPaymentInitiationID)
    {
        var pispRefundAttempts = new List<PaymentRequestRefundAttempt>();

        // Get pisp refund attempts for the given pisp payment initiation id.
        var pispRefundEvents = events.Where(x =>
                !string.IsNullOrEmpty(x.PispPaymentInitiationID) &&
                x.PispPaymentInitiationID == pispPaymentInitiationID && x.RefundPayoutID != null
                &&
                (
                    (x.EventType == PaymentRequestEventTypesEnum.pisp_refund_initiated) ||
                    (x.EventType == PaymentRequestEventTypesEnum.pisp_refund_settled) ||
                    (x.EventType == PaymentRequestEventTypesEnum.pisp_refund_cancelled)))
            .OrderBy(x => x.Inserted)
            .GroupBy(x => x.RefundPayoutID)
            .ToList();

        foreach (var pispRefundEvent in pispRefundEvents)
        {
            if (pispRefundEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.pisp_refund_initiated))
            {
                var refundInitiatedEvent =
                    pispRefundEvent.First(x => x.EventType == PaymentRequestEventTypesEnum.pisp_refund_initiated);

                var refundAttempt = new PaymentRequestRefundAttempt
                {
                    RefundPayoutID = refundInitiatedEvent.RefundPayoutID,
                    RefundInitiatedAt = refundInitiatedEvent.Inserted,
                    RefundInitiatedAmount = refundInitiatedEvent.Amount,
                };

                if (pispRefundEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.pisp_refund_cancelled))
                {
                    var refundCancelledEvent = pispRefundEvent.First(x =>
                        x.EventType == PaymentRequestEventTypesEnum.pisp_refund_cancelled);

                    refundAttempt.RefundCancelledAt = refundCancelledEvent.Inserted;
                    refundAttempt.RefundCancelledAmount = refundCancelledEvent.Amount;
                }
                else if (pispRefundEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.pisp_refund_settled))
                {
                    var refundSettledEvent = pispRefundEvent.First(x =>
                        x.EventType == PaymentRequestEventTypesEnum.pisp_refund_settled);

                    refundAttempt.RefundSettledAt = refundSettledEvent.Inserted;
                    refundAttempt.RefundSettledAmount = refundSettledEvent.Amount;
                }

                pispRefundAttempts.Add(refundAttempt);
            }
        }

        return pispRefundAttempts;
    }

    /// <summary>
    /// Groups the payment request events into a list of payment attempts for Lightning payments.
    /// </summary>
    /// <returns>A list of Lightning attempts for the payment request events.</returns>
    public static IEnumerable<PaymentRequestPaymentAttempt> GetLightningPaymentAttempts(this IEnumerable<PaymentRequestEvent> events)
    {
        var lightningPaymentAttempts = new List<PaymentRequestPaymentAttempt>();
        
        var lightningAttempts = events.Where(x => !string.IsNullOrEmpty(x.LightningRHash) &&
                                                           ((x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_created) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid) ||
                                                            (x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_expired)))
            .OrderBy(x => x.Inserted)
            .GroupBy(x => x.LightningRHash)
            .ToList();

        foreach (var attempt in lightningAttempts)
        {
            // The invoice created event should always be present but if for some reason it's not the next best event
            // will be used as the starting point for the attempt.
            var initiateEvent =
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_created).FirstOrDefault() ??
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid).FirstOrDefault() ??
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_expired).FirstOrDefault();

            if (initiateEvent != null)
            {
                var paymentAttempt = new PaymentRequestPaymentAttempt
                {
                    AttemptKey = attempt.Key ?? string.Empty,
                    PaymentRequestID = initiateEvent.PaymentRequestID,
                    InitiatedAt = initiateEvent.Inserted,
                    PaymentMethod = PaymentMethodTypeEnum.lightning,
                    Currency = initiateEvent.Currency,
                    AttemptedAmount = initiateEvent.Amount,
                    PaymentProcessor = initiateEvent.PaymentProcessorName
                };

                if (attempt.Any(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid))
                {
                    var settleEvent = attempt.First(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_paid);

                    paymentAttempt.SettledAt = settleEvent.Inserted;
                    paymentAttempt.SettledAmount = settleEvent.Amount;
                    paymentAttempt.ReconciledTransactionID = settleEvent.ReconciledTransactionID;
                }
                else if (attempt.Any(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_expired))
                {
                    var settleFailedEvent = attempt.First(x => x.EventType == PaymentRequestEventTypesEnum.lightning_invoice_expired);

                    paymentAttempt.SettleFailedAt = settleFailedEvent.Inserted;
                }

                lightningPaymentAttempts.Add(paymentAttempt);
            }
        }

        return lightningPaymentAttempts;
    }
}
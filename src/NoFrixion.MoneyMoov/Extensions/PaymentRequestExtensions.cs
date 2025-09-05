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
//   MIT.
//  -----------------------------------------------------------------------------

using System.Globalization;
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
            paymentAttempts.AddRange(GetDirectDebitPaymentAttempts(events));
            
            paymentAttempts.Sort(
                (x, y) => y.InitiatedAt.CompareTo(x.InitiatedAt));

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
                attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_webhook).FirstOrDefault();

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
                    x.EventType is 
                        PaymentRequestEventTypesEnum.pisp_callback or
                        PaymentRequestEventTypesEnum.pisp_webhook))
                {
                    var authorisationEvent = pispCallbackOrWebhook switch
                    {
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
                        PaymentRequestEvent cbk when cbk.PaymentProcessorName == PaymentProcessorsEnum.BankingCircleDirectDebit
                            && cbk.Status == PaymentRequestResult.BANKINGCIRCLE_DIRECTDEBIT_CREATED_STATUS => cbk,
                        _ => null
                    };

                    if (pispCallbackOrWebhook.Status is PaymentRequestResult.PISP_YAPILY_AUTHORISATION_ERROR
                        or PaymentRequestResult.PISP_NOFRIXION_AUTHORISATION_ERROR)
                    {
                        paymentAttempt.PispAuthorisationFailedAt = pispCallbackOrWebhook.Inserted;
                    }

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

                    foreach (var paymentRequestEvent in attempt.Where(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle).Skip(1).ToList())
                    {
                        var otherPaymentAttempt = AddPaymentAttempt(events, attempt, paymentRequestEvent);

                        pispPaymentAttempts.Add(otherPaymentAttempt);
                    }
                }
                else if (attempt.Any(x => x.EventType is PaymentRequestEventTypesEnum.pisp_settle_failure))
                {
                    var settleFailedEvent = attempt.First(x => x.EventType is PaymentRequestEventTypesEnum.pisp_settle_failure);

                    paymentAttempt.SettleFailedAt = settleFailedEvent.Inserted;

                    //Set authorised amount to zero when there is settlement failure for this attempt.
                    paymentAttempt.AuthorisedAmount = 0;

                }

                pispPaymentAttempts.Add(paymentAttempt);
            }
            else
            {
                var settledEvent = attempt.FirstOrDefault(x => x.EventType == PaymentRequestEventTypesEnum.pisp_settle);

                if (settledEvent != null)
                {
                    var paymentAttempt = AddPaymentAttempt(events, attempt, settledEvent);
                    
                    pispPaymentAttempts.Add(paymentAttempt);
                }
                
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

    /// <summary>
    /// Gets payment requests attempts for Direct Debit payments.
    /// </summary>
    /// <param name="events">Entire list of payment request events</param>
    /// <returns>List of Direct Debit payment attempts.</returns>
    public static IEnumerable<PaymentRequestPaymentAttempt> GetDirectDebitPaymentAttempts(this IEnumerable<PaymentRequestEvent> events)
    {
        var ddPaymentAttempts = new List<PaymentRequestPaymentAttempt>();
        
        var ddAttempts = events
            .Where(x => x.EventType is
                PaymentRequestEventTypesEnum.direct_debit_create or
                PaymentRequestEventTypesEnum.direct_debit_failed or 
                PaymentRequestEventTypesEnum.direct_debit_paid)
            .OrderBy(x => x.Inserted)
            .GroupBy(x => x.DirectDebitPaymentReference)
            .ToList();
        
        foreach (var attempt in ddAttempts)
        {
            var createEvent = attempt.FirstOrDefault(x =>
                x.EventType is PaymentRequestEventTypesEnum.direct_debit_create);

            if (createEvent is null)
            {
                continue;
            }
            
            var paymentAttempt = new PaymentRequestPaymentAttempt
            {
                AttemptKey = attempt.Key ?? string.Empty,
                PaymentRequestID = createEvent.PaymentRequestID,
                InitiatedAt = createEvent.Inserted,
                PaymentMethod = PaymentMethodTypeEnum.directDebit,
                Currency = createEvent.Currency,
                AttemptedAmount = createEvent.Amount,
                PaymentProcessor = createEvent.PaymentProcessorName,
                AuthorisedAt = createEvent.Inserted,
                AuthorisedAmount = createEvent.Amount
            };

            // Note DD chargebacks can happen after a DD is successfully paid so check for failures first and only process settles if no failures.
            if (attempt.Any(x => x.EventType is PaymentRequestEventTypesEnum.direct_debit_failed))
            {
                var settleFailedEvent = attempt.First(x => x.EventType is PaymentRequestEventTypesEnum.direct_debit_failed);

                paymentAttempt.AuthorisedAmount = 0;
                paymentAttempt.SettledAmount = 0;
                paymentAttempt.SettleFailedAt = settleFailedEvent.Inserted;
            }
            else if (attempt.Any(x => x.EventType is PaymentRequestEventTypesEnum.direct_debit_paid))
            {
                var paidEvent = attempt.First(x => x.EventType is PaymentRequestEventTypesEnum.direct_debit_paid);
                
                paymentAttempt.SettledAt = paidEvent.Inserted;
                paymentAttempt.SettledAmount = paidEvent.Amount;
                paymentAttempt.ReconciledTransactionID = paidEvent.ReconciledTransactionID;
            }

            ddPaymentAttempts.Add(paymentAttempt);
        }

        return ddPaymentAttempts;
    }

    public static string ToCsvRowString(this PaymentRequest paymentRequest)
    {
        if (paymentRequest == null)
        {
            return string.Empty;
        }
        
        var transactionIDs = paymentRequest.Transactions.Count != 0
            ? string.Join(",", paymentRequest.Transactions.Select(t => t.ID))
            : "";

        var values = new List<string>
        {
            paymentRequest.ID.ToString(),
            paymentRequest.MerchantID.ToString(),
            PaymentAmount.GetRoundedAmount(paymentRequest.Currency, paymentRequest.Amount).ToString(CultureInfo.InvariantCulture),
            paymentRequest.Currency.ToString(),
            string.Join(",", paymentRequest.PaymentMethods.Select(x => x.ToString())),
            paymentRequest.Description ?? "",
            paymentRequest.CustomerID ?? "",
            paymentRequest.OrderID ?? "",
            paymentRequest.Inserted.ToString("o"),
            paymentRequest.LastUpdated.ToString("o"),
            paymentRequest.PispAccountID?.ToString() ?? "",
            paymentRequest.DestinationAccount != null ? paymentRequest.DestinationAccount.AccountName : "",
            paymentRequest.BaseOriginUrl ?? "",
            paymentRequest.CardAuthorizeOnly.ToString(),
            paymentRequest.CardCreateToken.ToString(),
            paymentRequest.CardCreateTokenMode.ToString(),
            paymentRequest.Status.ToString(),
            paymentRequest.PartialPaymentMethod.ToString(),
            paymentRequest.CustomerEmailAddress ?? "",
            paymentRequest.CardStripePaymentIntentID ?? "",
            paymentRequest.CardStripePaymentIntentSecret ?? "",
            paymentRequest.NotificationEmailAddresses ?? "",
            paymentRequest.PriorityBankID?.ToString() ?? "",
            paymentRequest.Title ?? "",
            paymentRequest.PartialPaymentSteps ?? "",
            PaymentAmount.GetRoundedAmount(paymentRequest.Currency, paymentRequest.AmountReceived)
                .ToString(CultureInfo.InvariantCulture),
            PaymentAmount.GetRoundedAmount(paymentRequest.Currency, paymentRequest.AmountRefunded)
                .ToString(CultureInfo.InvariantCulture),
            PaymentAmount.GetRoundedAmount(paymentRequest.Currency, paymentRequest.AmountPending)
                .ToString(CultureInfo.InvariantCulture),
            paymentRequest.CreatedByUser != null ? paymentRequest.CreatedByUser.ID.ToString() : "",
            paymentRequest.CreatedByUser != null ? $"{paymentRequest.CreatedByUser.FirstName} {paymentRequest.CreatedByUser.LastName}" : "",
            paymentRequest.MerchantTokenDescription ?? "",
            transactionIDs,
            paymentRequest.PayrunID.ToString() ?? "",
            paymentRequest.Addresses?.FirstOrDefault(x => x.AddressType == AddressTypesEnum.Shipping)
                ?.ToDisplayString() ?? "",
            paymentRequest.Addresses?.FirstOrDefault(x => x.AddressType == AddressTypesEnum.Billing)
                ?.ToDisplayString() ?? "",
            paymentRequest.Addresses?.Count > 0
                ? $"{paymentRequest.Addresses.First().FirstName} {paymentRequest.Addresses.First().LastName}"
                : "",
            paymentRequest.PaymentProcessor.ToString(),
            paymentRequest.Tags != null && paymentRequest.Tags.Count != 0
                ? string.Join(",", paymentRequest.Tags.Select(t => t.Name))
                : "",
            paymentRequest.DueDate?.ToString("o") ?? "",
            paymentRequest.CustomFields?.Select(cf => $"{cf.Name}: {cf.Value}")
                .Aggregate((current, next) => $"{current}, {next}") ?? "",
        };


        // Quote values to handle commas in the data
        return string.Join(",", values.Select(x => x.ToSafeCsvString()));
    }
    
    private static PaymentRequestPaymentAttempt AddPaymentAttempt(IEnumerable<PaymentRequestEvent> events,
        IGrouping<string?, PaymentRequestEvent> attempt,
        PaymentRequestEvent paymentRequestEvent)
    {
        var otherPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            AttemptKey = attempt.Key ?? string.Empty,
            PaymentRequestID = paymentRequestEvent.PaymentRequestID,
            InitiatedAt = paymentRequestEvent.Inserted,
            PaymentMethod = PaymentMethodTypeEnum.pisp,
            Currency = paymentRequestEvent.Currency,
            AttemptedAmount = paymentRequestEvent.Amount,
            PaymentProcessor = paymentRequestEvent.PaymentProcessorName,
            InstitutionID = paymentRequestEvent.PispPaymentServiceProviderID,
            InstitutionName = paymentRequestEvent.PispPaymentInstitutionName,
            SettledAmount = paymentRequestEvent.Amount,
            SettledAt = paymentRequestEvent.Inserted,
            ReconciledTransactionID = paymentRequestEvent.ReconciledTransactionID,
            RefundAttempts = GetPispRefundAttempts(events, paymentRequestEvent.PispPaymentInitiationID!).ToList(),
            AuthorisedAmount = paymentRequestEvent.Amount,
            AuthorisedAt = paymentRequestEvent.Inserted
        };
        return otherPaymentAttempt;
    }
}
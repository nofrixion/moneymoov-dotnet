// -----------------------------------------------------------------------------
//  Filename: PaymentRequestEventExtensions.cs
// 
//  Description: Extension methods for PaymentRequestEvent model
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  21 07 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------


using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class PaymentRequestEventExtensions
{
    /// <summary>
    /// Sets the payment attempt card properties from a grouping of card events.
    /// This method takes care of setting card authorisation specific properties in payment attempt.
    /// </summary>
    /// <param name="groupedCardEvents">A set of card payment events with the same CardAuthorisationResponseID.</param>
    /// <param name="paymentAttempt">The payment attempt object in which the authorisation properties are to be set.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void HandleCardAuthorisationEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvents,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        if (paymentAttempt == null) throw new ArgumentNullException(nameof(paymentAttempt));

        if (groupedCardEvents.All(x => x.EventType != PaymentRequestEventTypesEnum.card_authorization))
        {
            return;
        }

        var cardAuthorizationEvent =
            groupedCardEvents.First(x => x.EventType == PaymentRequestEventTypesEnum.card_authorization);

        // The CardAuthorizationResponseID is NULL for card_payer_authentication_setup events.
        var cardAuthorizationSetupEvent = groupedCardEvents.FirstOrDefault(x =>
            x.EventType == PaymentRequestEventTypesEnum.card_payer_authentication_setup
            && x.CardRequestID == cardAuthorizationEvent.CardRequestID);


        var initialEvent = cardAuthorizationSetupEvent ?? cardAuthorizationEvent;
        paymentAttempt.AttemptKey = groupedCardEvents.Key ?? string.Empty;
        paymentAttempt.PaymentRequestID = initialEvent.PaymentRequestID;
        paymentAttempt.InitiatedAt = initialEvent.Inserted;
        paymentAttempt.PaymentMethod = PaymentMethodTypeEnum.card;
        paymentAttempt.Currency = initialEvent.Currency;
        paymentAttempt.AttemptedAmount = cardAuthorizationEvent.Amount;
        paymentAttempt.PaymentProcessor = initialEvent.PaymentProcessorName;
        paymentAttempt.TokenisedCardID = cardAuthorizationEvent.TokenisedCardID?.ToString();

        var isSuccessfullAuthorisationEvent = cardAuthorizationEvent.IsSuccessfulCardAuthorisationEvent();


        // If the card authorization event was successful, then the payment attempt was authorised.
        if (isSuccessfullAuthorisationEvent)
        {
            paymentAttempt.CardAuthorisedAt = cardAuthorizationEvent.Inserted;
            paymentAttempt.CardAuthorisedAmount = cardAuthorizationEvent.Amount;
        }
        else
        {
            paymentAttempt.CardAuthoriseFailedAt = cardAuthorizationEvent.Inserted;
        }
    }

    public static void HandleCardCaptureEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvents,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        var cardCaptureEvents = groupedCardEvents.GetAllCardCaptureEvents();

        if (!cardCaptureEvents.Any())
        {
            return;
        }

        foreach (var cardCaptureEvent in cardCaptureEvents)
        {
            // Successful capture events are added to the capture attempts list.
            if (cardCaptureEvent.IsSuccessfulCardCaptureOrSaleEvent())
            {
                paymentAttempt.CaptureAttempts.AddRange(new[]
                {
                    new PaymentRequestCaptureAttempt
                        { CapturedAt = cardCaptureEvent.Inserted, CapturedAmount = cardCaptureEvent.Amount }
                });
            }
            else
            {
                paymentAttempt.CardAuthoriseFailedAt = cardCaptureEvent.Inserted;
                // Failed capture events are added to the capture attempts list.
                paymentAttempt.CaptureAttempts.AddRange(new[]
                {
                    new PaymentRequestCaptureAttempt
                    {
                        CaptureFailedAt = cardCaptureEvent.Inserted, CaptureFailureError = cardCaptureEvent.ErrorMessage
                    }
                });
            }
        }
    }

    public static void HandleCardSaleEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvent,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        var cardSaleEvents = groupedCardEvent.GetAllCardSaleEvents();

        if (!cardSaleEvents.Any())
        {
            return;
        }

        // This is done because in some cases the card authorise event may not be present
        // and card sale event is the first event. In such cases, the payment attempt is
        // created with the card sale event.
        if (string.IsNullOrEmpty(paymentAttempt.AttemptKey))
        {
            paymentAttempt.AttemptKey = groupedCardEvent.Key ?? string.Empty;
            paymentAttempt.PaymentRequestID = cardSaleEvents.First().PaymentRequestID;
            paymentAttempt.InitiatedAt = cardSaleEvents.First().Inserted;
            paymentAttempt.PaymentMethod = PaymentMethodTypeEnum.card;
            paymentAttempt.Currency = cardSaleEvents.First().Currency;
            paymentAttempt.AttemptedAmount = cardSaleEvents.First().Amount;
            paymentAttempt.PaymentProcessor = cardSaleEvents.First().PaymentProcessorName;
        }

        foreach (var cardSaleEvent in cardSaleEvents)
        {
            if (cardSaleEvent.IsSuccessfulCardCaptureOrSaleEvent())
            {
                paymentAttempt.CaptureAttempts.AddRange(new[]
                {
                    new PaymentRequestCaptureAttempt()
                    {
                        CapturedAmount = cardSaleEvent.Amount,
                        CapturedAt = cardSaleEvent.Inserted
                    }
                });

                // In case of Checkout, we don't get a card authorisation event. Hence, to fill the authorised
                // amount and authorised at date, we use the card sale event.
                paymentAttempt.CardAuthorisedAt ??= cardSaleEvent.Inserted;

                paymentAttempt.CardAuthorisedAmount = paymentAttempt.CardAuthorisedAmount == 0
                    ? cardSaleEvent.Amount
                    : paymentAttempt.CardAuthorisedAmount;

                if (string.IsNullOrEmpty(paymentAttempt.TokenisedCardID))
                {
                    paymentAttempt.TokenisedCardID = cardSaleEvent.TokenisedCardID?.ToString();
                }

                break;
            }

            // Soft decline event means that it has to be captured later.
            if (!cardSaleEvent.IsSoftDeclineSaleEvent())
            {
                paymentAttempt.CardAuthoriseFailedAt = cardSaleEvent.Inserted;
                paymentAttempt.CaptureAttempts.AddRange(new[]
                {
                    new PaymentRequestCaptureAttempt
                    {
                        CaptureFailedAt = cardSaleEvent.Inserted,
                        CaptureFailureError = cardSaleEvent.ErrorMessage
                    }
                });
            }
            else
            {
                paymentAttempt.CardAuthorisedAt ??= cardSaleEvent.Inserted;

                paymentAttempt.CardAuthorisedAmount = paymentAttempt.CardAuthorisedAmount == 0
                    ? cardSaleEvent.Amount
                    : paymentAttempt.CardAuthorisedAmount;
            }
        }
    }

    public static void HandleCardVoidEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvent,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        var cardVoidEvents =
            groupedCardEvent.Where(x => x.EventType == PaymentRequestEventTypesEnum.card_void).ToList();

        if (!cardVoidEvents.Any())
        {
            return;
        }

        var refundAttempts = (from cardVoidEvent in cardVoidEvents
                              where cardVoidEvent.Status == CardPaymentResponseStatus.CARD_VOIDED_SUCCESS_STATUS ||
                                    cardVoidEvent.Status == CardPaymentResponseStatus.CARD_CYBERSOURCE_AUTHORISATION_VOIDED_SUCCESS_STATUS ||
                                    cardVoidEvent.Status == CardPaymentResponseStatus.CARD_CYBERSOURCE_CAPTURE_VOIDED_SUCCESS_STATUS
                              select new PaymentRequestRefundAttempt
                              {
                                  RefundInitiatedAt = cardVoidEvent.Inserted,
                                  RefundInitiatedAmount = cardVoidEvent.Amount,
                                  RefundSettledAt = cardVoidEvent.Inserted,
                                  RefundSettledAmount = cardVoidEvent.Amount,
                                  IsCardVoid = true
                              }).ToList();

        paymentAttempt.RefundAttempts = refundAttempts;
    }
    
    public static void HandleCardRefundEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvent,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        var cardRefundEvents =
            groupedCardEvent.Where(x => x.EventType == PaymentRequestEventTypesEnum.card_refund).ToList();

        if (!cardRefundEvents.Any())
        {
            return;
        }

        var refundAttempts = (from cardRefundEvent in cardRefundEvents
            where cardRefundEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_REFUNDED_SUCCESS_STATUS ||
                  cardRefundEvent.Status == CardPaymentResponseStatus.CARD_CYBERSOURCE_REFUNDED_SUCCESS_STATUS
            select new PaymentRequestRefundAttempt
            {
                RefundInitiatedAt = cardRefundEvent.Inserted,
                RefundInitiatedAmount = cardRefundEvent.Amount,
                RefundSettledAt = cardRefundEvent.Inserted,
                RefundSettledAmount = cardRefundEvent.Amount,
                IsCardVoid = false
            }).ToList();

        paymentAttempt.RefundAttempts = refundAttempts;
    }

    public static void HandleCardWebhookEvents(this IGrouping<string?, PaymentRequestEvent> groupedCardEvent,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        var cardWebhookEvents = groupedCardEvent
            .Where(x => x.EventType == PaymentRequestEventTypesEnum.card_webhook)
            .ToList();

        if (!cardWebhookEvents.Any())
        {
            return;
        }

        if (string.IsNullOrEmpty(paymentAttempt.AttemptKey))
        {
            var firstEvent = cardWebhookEvents.First();

            paymentAttempt.AttemptKey = groupedCardEvent.Key ?? string.Empty;
            paymentAttempt.PaymentRequestID = firstEvent.PaymentRequestID;
            paymentAttempt.InitiatedAt = firstEvent.Inserted;
            paymentAttempt.PaymentMethod = PaymentMethodTypeEnum.card;
            paymentAttempt.Currency = firstEvent.Currency;
            paymentAttempt.AttemptedAmount = firstEvent.Amount;
            paymentAttempt.PaymentProcessor = firstEvent.PaymentProcessorName;
        }

        foreach (var cdEvent in cardWebhookEvents)
        {
            //Check payment is authorised.
            if (cdEvent.Status == CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS ||
                cdEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_AUTHORIZED_STATUS ||
                cdEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CARDVERFIED_STATUS ||
                cdEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS)
            {
                if (paymentAttempt.CardAuthorisedAmount == default)
                {
                    paymentAttempt.CardAuthorisedAt = cdEvent.Inserted;
                    paymentAttempt.CardAuthorisedAmount = cdEvent.Amount;
                }

                //Check event status is capture and no other capture event types in the attempt.
                //This is because we don’t want to add duplicate captured amount.
                //Only take the captured amount from event type capture or webhook if event type capture is missing.
                //Don't add captured amount if it's already set.
                if (cdEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS &&
                    !groupedCardEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.card_capture) &&
                    !paymentAttempt.CaptureAttempts.Any())
                {
                    //Add success capture attempt.
                    paymentAttempt.CaptureAttempts.Add(new PaymentRequestCaptureAttempt()
                    {
                        CapturedAmount = cdEvent.Amount,
                        CapturedAt = cdEvent.Inserted,
                    });
                }
            }
        }
    }

    public static void SetWalletName(this IGrouping<string?, PaymentRequestEvent> groupedCardEvent,
        PaymentRequestPaymentAttempt paymentAttempt)
    {
        if (groupedCardEvent.Any(x => x.WalletName != null))
        {
            paymentAttempt.WalletName = groupedCardEvent.First(x => x.WalletName != null).WalletName;
        }
    }

    private static bool IsCardRelatedEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_payer_authentication_setup
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_authorization
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_sale
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_capture
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_void
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_webhook
                || paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_refund;
    }

    public static List<IGrouping<string?, PaymentRequestEvent>> GetGroupedCardEvents(
        this IEnumerable<PaymentRequestEvent> paymentRequestEvents)
    {
        return paymentRequestEvents
            .Where(
                x => !string.IsNullOrEmpty(x.CardAuthorizationResponseID)
                     && x.IsCardRelatedEvent()).OrderBy(x => x.Inserted)
            .GroupBy(x => x.CardAuthorizationResponseID).ToList();
    }

    private static bool IsSuccessfulCardAuthorisationEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_authorization &&
               paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS
               || paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_PAYMENT_SOFT_DECLINE_STATUS
               || paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_AUTHORIZED_STATUS
               || paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CARDVERFIED_STATUS;
    }

    private static bool IsSuccessfulCardCaptureOrSaleEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return (paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_sale ||
                paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_capture) &&
               (
                   paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS ||
                   paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_CHECKOUT_CAPTURED_STATUS ||
                   paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_CAPTURE_SUCCESS_STATUS
               );
    }

    private static bool IsSoftDeclineSaleEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.card_sale &&
               paymentRequestEvent.Status == CardPaymentResponseStatus.CARD_PAYMENT_SOFT_DECLINE_STATUS;
    }

    private static List<PaymentRequestEvent> GetAllCardCaptureEvents(
        this IGrouping<string?, PaymentRequestEvent> groupedCardEvent)
    {
        return groupedCardEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.card_capture)
            ? groupedCardEvent
                .Where(x => x.EventType == PaymentRequestEventTypesEnum.card_capture)
                .ToList()
            : new List<PaymentRequestEvent>();
    }

    private static List<PaymentRequestEvent> GetAllCardSaleEvents(
        this IGrouping<string?, PaymentRequestEvent> groupedCardEvent)
    {
        return groupedCardEvent.Any(x => x.EventType == PaymentRequestEventTypesEnum.card_sale) ? groupedCardEvent
            .Where(x => x.EventType == PaymentRequestEventTypesEnum.card_sale)
            .ToList() : new List<PaymentRequestEvent>();
    }

    private static bool IsPispRelatedEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_initiate ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_callback ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_webhook ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_settle ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_settle_failure ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_refund_initiated ||
            paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.pisp_refund_settled;
    }

    private static bool IsDirectDebitRelatedEvent(this PaymentRequestEvent paymentRequestEvent)
    {
        return paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.direct_debit_create ||
               paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.direct_debit_failed ||
               paymentRequestEvent.EventType == PaymentRequestEventTypesEnum.direct_debit_paid;
    }

    public static PaymentMethodTypeEnum GetPaymentMethodType(this PaymentRequestEvent paymentRequestEvent)
    {
        if (paymentRequestEvent.IsCardRelatedEvent())
        {
            return PaymentMethodTypeEnum.card;
        }

        if (paymentRequestEvent.IsPispRelatedEvent())
        {
            return PaymentMethodTypeEnum.pisp;
        }

        if (paymentRequestEvent.IsDirectDebitRelatedEvent())
        {
            return PaymentMethodTypeEnum.directDebit;
        }

        return PaymentMethodTypeEnum.None;
    }
}
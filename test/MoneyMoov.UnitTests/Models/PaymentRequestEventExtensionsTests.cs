// -----------------------------------------------------------------------------
//  Filename: PaymentRequestEventExtensionsTests.cs
// 
//  Description: Tests for PaymentRequestEventExtensions.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  24 07 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------


using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace MoneyMoov.UnitTests.Models;

public class PaymentRequestEventExtensionsTests
{
    [Fact]
    public void GetGroupedCardEvents_Success()
    {
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;
        var cardEvent1 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = paymentRequestID,
            Amount = amount,
            Currency = CurrencyTypeEnum.EUR,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
            Status = "PENDING",
            PaymentProcessorName = PaymentProcessorsEnum.Checkout,
            CardAuthorizationResponseID = cardAuthorizationResponseID,
            CardRequestID = cardRequestID,
        };

        var cardEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = paymentRequestID,
            Amount = amount,
            Currency = CurrencyTypeEnum.EUR,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_authorization,
            Status = "AUTHORIZED",
            PaymentProcessorName = PaymentProcessorsEnum.Checkout,
            CardAuthorizationResponseID = cardAuthorizationResponseID,
            CardRequestID = cardRequestID,
        };

        var cardEvent3 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = paymentRequestID,
            Amount = amount,
            Currency = CurrencyTypeEnum.EUR,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_capture,
            Status = "CAPTURED",
            PaymentProcessorName = PaymentProcessorsEnum.Checkout,
            CardAuthorizationResponseID = cardAuthorizationResponseID,
            CardRequestID = cardRequestID,
        };

        var cardEvent4 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = paymentRequestID,
            Amount = amount,
            Currency = CurrencyTypeEnum.EUR,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = "CAPTURED",
            PaymentProcessorName = PaymentProcessorsEnum.Checkout,
            CardAuthorizationResponseID = cardAuthorizationResponseID,
            CardRequestID = cardRequestID,
        };

        var cardEvent5 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = paymentRequestID,
            Amount = amount,
            Currency = CurrencyTypeEnum.EUR,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_void,
            Status = "VOIDED",
            PaymentProcessorName = PaymentProcessorsEnum.Checkout,
            CardAuthorizationResponseID = cardAuthorizationResponseID,
            CardRequestID = cardRequestID,
        };

        List<PaymentRequestEvent> paymentRequestEvents = new List<PaymentRequestEvent>()
        {
            cardEvent1, cardEvent2, cardEvent3, cardEvent4, cardEvent5
        };

        List<IGrouping<string?, PaymentRequestEvent>> groupedCardEvents = paymentRequestEvents.GetGroupedCardEvents();

        Assert.Single(groupedCardEvents);
        Assert.Equal(5, groupedCardEvents.First().Count());
    }

    [Fact]
    public void HandleCardAuthorisationEvents_NoCardAuthorizationEvent_NoChangesToPaymentAttempt()
    {
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;

        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var groupedCardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardAuthorisationEvents(paymentAttempt);

        // Assert
        Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
        Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
        Assert.Null(paymentAttempt.CardAuthorisedAt);
        Assert.Equal(0, paymentAttempt.CardAuthorisedAmount);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_SuccessfulCardAuthorizationEvent_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;
        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_authorization,
                    Status = "AUTHORIZED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            };
        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardAuthorisationEvents(paymentAttempt);

        // Assert
        Assert.NotNull(paymentAttempt.CardAuthorisedAt);
        Assert.Equal(amount, paymentAttempt.CardAuthorisedAmount);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_NoCardCaptureEvent_NoChangesToPaymentAttempt()
    {
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;

        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var groupedCardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardCaptureEvents(paymentAttempt);

        // Assert
        Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
        Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.CaptureAttempts);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_SuccessfulCardCaptureEvent_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;
        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_authorization,
                    Status = "AUTHORIZED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_capture,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            };
        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardCaptureEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.CaptureAttempts);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_NoCardSaleEvent_NoChangesToPaymentAttempt()
    {
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;

        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var groupedCardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardSaleEvents(paymentAttempt);

        // Assert
        Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
        Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.CaptureAttempts);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_SuccessfulCardSaleEvent_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;
        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_sale,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            };
        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardSaleEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.CaptureAttempts);
        Assert.True(paymentAttempt.CardAuthorisedAmount > 0);
        Assert.NotNull(paymentAttempt.CardAuthorisedAt);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_NoCardVoidEvent_NoChangesToPaymentAttempt()
    {
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;

        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var groupedCardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardVoidEvents(paymentAttempt);

        // Assert
        Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
        Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.RefundAttempts);
    }

    [Fact]
    public void HandleCardAuthorisationEvents_SuccessfulCardVoidEvent_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var cardRequestID = Guid.NewGuid().ToString();
        var amount = 12.12m;
        var cardEvents = new List<PaymentRequestEvent>
            {
                new ()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_authorization,
                    Status = "AUTHORIZED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new ()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_capture,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new ()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_sale,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                },

                new ()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTime.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_void,
                    Status = "VOIDED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardRequestID,
                }
            };
        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardVoidEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.RefundAttempts);
    }

    /// <summary>
    /// Tests that payment attempt is updated from webhook event even if card sale event isn’t received in the callback.
    /// </summary>
    [Fact]
    public void HandleCardWebhookEvents_CardSale_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var amount = 12.12m;

        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_webhook,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                }
            };

        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardWebhookEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.CaptureAttempts);
        Assert.Equal(amount, paymentAttempt.CaptureAttempts.First().CapturedAmount);
        Assert.Equal(amount, paymentAttempt.AttemptedAmount);
        Assert.Equal(cardAuthorizationResponseID, paymentAttempt.AttemptKey);
        Assert.Equal(paymentRequestID, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.RefundAttempts);
        Assert.Equal(PaymentResultEnum.FullyPaid, paymentAttempt.Status);
    }

    /// <summary>
    /// Tests that payment attempt is not updated from webhooks when there is a card sale event
    /// that has already updated the attempts fields. 
    /// </summary>
    [Fact]
    public void HandleCardWebhookEvents_CardSale_NoChangesToPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var amount = 12.12m;

        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_webhook,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_sale,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                }
            };

        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        groupedCardEvents.First().HandleCardSaleEvents(paymentAttempt);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardWebhookEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.CaptureAttempts);
        Assert.Equal(amount, paymentAttempt.CaptureAttempts.First().CapturedAmount);
        Assert.Equal(amount, paymentAttempt.AttemptedAmount);
        Assert.Equal(cardAuthorizationResponseID, paymentAttempt.AttemptKey);
        Assert.Equal(paymentRequestID, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.RefundAttempts);
        Assert.Equal(PaymentResultEnum.FullyPaid, paymentAttempt.Status);
    }


    /// <summary>
    /// Tests that payment attempt is updated from webhook event even if card authorisation event isn’t received in the callback.
    /// </summary>
    [Fact]
    public void HandleCardWebhookEvents_CardAuthorization_UpdatesPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var amount = 15.15m;

        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_webhook,
                    Status = "AUTHORIZED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                }
            };

        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardWebhookEvents(paymentAttempt);

        // Assert
        Assert.Empty(paymentAttempt.CaptureAttempts);
        Assert.Equal(amount, paymentAttempt.CardAuthorisedAmount);
        Assert.Equal(amount, paymentAttempt.AttemptedAmount);
        Assert.Equal(cardAuthorizationResponseID, paymentAttempt.AttemptKey);
        Assert.Equal(paymentRequestID, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.RefundAttempts);
        Assert.Equal(PaymentResultEnum.FullyPaid, paymentAttempt.Status);
    }

    // -----------------------------------------------------------------------
    // HandleCardRefundEvents tests
    // -----------------------------------------------------------------------

    /// <summary>
    /// Tests that a legacy card_refund event with CARD_CHECKOUT_REFUNDED_SUCCESS_STATUS
    /// is treated as immediately settled (backward compatibility with pre-rework events).
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_Legacy_CardRefund_WithSuccessStatus_TreatedAsSettled()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var amount = 50M;

        var cardEvents = new List<PaymentRequestEvent>
        {
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUNDED_SUCCESS_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: legacy event treated as both initiated and settled
        Assert.Single(paymentAttempt.RefundAttempts);
        var refund = paymentAttempt.RefundAttempts.Single();
        Assert.Equal(amount, refund.RefundInitiatedAmount);
        Assert.Equal(amount, refund.RefundSettledAmount);
        Assert.Equal(0M, refund.RefundCancelledAmount);
        Assert.False(refund.IsCardVoid);
    }

    /// <summary>
    /// Tests that a card_refund_pending event with a CardRequestID creates a refund attempt
    /// with only RefundInitiatedAmount set — settled and cancelled amounts remain zero.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_NewFlow_PendingOnly_SetsInitiatedAmount()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID = "act_pending001";
        var amount = 40M;

        var cardEvents = new List<PaymentRequestEvent>
        {
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert
        Assert.Single(paymentAttempt.RefundAttempts);
        var refund = paymentAttempt.RefundAttempts.Single();
        Assert.NotNull(refund.RefundInitiatedAt);
        Assert.Equal(amount, refund.RefundInitiatedAmount);
        Assert.Equal(0M, refund.RefundSettledAmount);
        Assert.Equal(0M, refund.RefundCancelledAmount);
        Assert.Null(refund.RefundSettledAt);
        Assert.Null(refund.RefundCancelledAt);
        Assert.False(refund.IsCardVoid);
    }

    /// <summary>
    /// Tests that card_refund_pending followed by card_refund_settled (same CardRequestID)
    /// produces one refund attempt with RefundSettledAmount populated.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_NewFlow_PendingThenSettled_SetsSettledAmount()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID = "act_settle001";
        var amount = 100M;

        var cardEvents = new List<PaymentRequestEvent>
        {
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-2),
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            },
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_settled,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_SETTLED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: one refund attempt, settled
        Assert.Single(paymentAttempt.RefundAttempts);
        var refund = paymentAttempt.RefundAttempts.Single();
        Assert.NotNull(refund.RefundInitiatedAt);
        Assert.Equal(amount, refund.RefundInitiatedAmount);
        Assert.NotNull(refund.RefundSettledAt);
        Assert.Equal(amount, refund.RefundSettledAmount);
        Assert.Equal(0M, refund.RefundCancelledAmount);
        Assert.Null(refund.RefundCancelledAt);
    }

    /// <summary>
    /// Tests that card_refund_pending followed by card_refund_declined (same CardRequestID)
    /// produces one refund attempt with RefundCancelledAmount populated and settled = 0.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_NewFlow_PendingThenDeclined_SetsCancelledAmount()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID = "act_decline001";
        var amount = 75M;

        var cardEvents = new List<PaymentRequestEvent>
        {
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-2),
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            },
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_declined,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_DECLINED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: one refund attempt, declined
        Assert.Single(paymentAttempt.RefundAttempts);
        var refund = paymentAttempt.RefundAttempts.Single();
        Assert.NotNull(refund.RefundInitiatedAt);
        Assert.Equal(amount, refund.RefundInitiatedAmount);
        Assert.NotNull(refund.RefundCancelledAt);
        Assert.Equal(amount, refund.RefundCancelledAmount);
        Assert.Equal(0M, refund.RefundSettledAmount);
        Assert.Null(refund.RefundSettledAt);
    }

    /// <summary>
    /// Tests that two separate refunds (different CardRequestIDs) each produce their own
    /// independent refund attempt entry.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_NewFlow_TwoSeparateRefunds_ProduceTwoAttempts()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID1 = "act_ref001";
        var refundActionID2 = "act_ref002";

        var cardEvents = new List<PaymentRequestEvent>
        {
            // first refund — settled
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 30M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-10),
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID1
            },
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 30M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-8),
                EventType = PaymentRequestEventTypesEnum.card_refund_settled,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_SETTLED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID1
            },
            // second refund — still pending
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 20M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID2
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: two independent refund attempts
        Assert.Equal(2, paymentAttempt.RefundAttempts.Count);

        var settled = paymentAttempt.RefundAttempts.Single(r => r.RefundSettledAmount > 0);
        Assert.Equal(30M, settled.RefundSettledAmount);

        var pending = paymentAttempt.RefundAttempts.Single(r => r.RefundSettledAmount == 0 && r.RefundCancelledAmount == 0);
        Assert.Equal(20M, pending.RefundInitiatedAmount);
    }

    /// <summary>
    /// Tests that a mix of one legacy card_refund event and one new async refund lifecycle
    /// produces two separate refund attempts, both correctly populated.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_MixedLegacyAndNewFlow_ProducesTwoAttempts()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID = "act_new001";

        var cardEvents = new List<PaymentRequestEvent>
        {
            // legacy event — should be treated as settled immediately
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 25M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-20),
                EventType = PaymentRequestEventTypesEnum.card_refund,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUNDED_SUCCESS_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
            },
            // new async refund — pending then settled
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 35M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-5),
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            },
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 35M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_settled,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_SETTLED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: two refund attempts total
        Assert.Equal(2, paymentAttempt.RefundAttempts.Count);

        // Legacy attempt: both initiated and settled to same amount
        var legacy = paymentAttempt.RefundAttempts.Single(r => r.RefundSettledAmount == 25M);
        Assert.Equal(25M, legacy.RefundInitiatedAmount);
        Assert.Equal(25M, legacy.RefundSettledAmount);

        // New async attempt: settled amount from webhook
        var newAsync = paymentAttempt.RefundAttempts.Single(r => r.RefundSettledAmount == 35M);
        Assert.Equal(35M, newAsync.RefundInitiatedAmount);
        Assert.Equal(35M, newAsync.RefundSettledAmount);
    }

    /// <summary>
    /// Tests that a card_refund_settled event with no matching card_refund_pending
    /// (i.e. missing the pending event) produces no refund attempt — the guard
    /// on pendingEvent == null is enforced.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_SettledWithoutPending_IsIgnored()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var refundActionID = "act_orphan001";

        var cardEvents = new List<PaymentRequestEvent>
        {
            // settled event arrives with no prior pending event
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 50M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_settled,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_SETTLED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = refundActionID
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: no attempt created because there is no pending event to anchor it
        Assert.Empty(paymentAttempt.RefundAttempts);
    }

    /// <summary>
    /// Tests that a card_refund_pending event with a null/empty CardRequestID is filtered out
    /// and produces no refund attempt.
    /// </summary>
    [Fact]
    public void HandleCardRefundEvents_PendingWithNullCardRequestID_IsIgnored()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();

        var cardEvents = new List<PaymentRequestEvent>
        {
            new()
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = 50M,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_refund_pending,
                Status = CardPaymentResponseStatus.CARD_CHECKOUT_REFUND_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = null  // missing action ID — must be filtered out
            }
        }.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        cardEvents.Single(x => x.Key == cardAuthorizationResponseID)
            .HandleCardRefundEvents(paymentAttempt);

        // Assert: filtered by the Where(!string.IsNullOrEmpty(x.CardRequestID)) guard
        Assert.Empty(paymentAttempt.RefundAttempts);
    }

    /// <summary>
    /// Tests that payment attempt is not being updated from webhook event if a card capture event exists. 
    /// </summary>
    [Fact]
    public void HandleCardWebhookEvents_CardCapture_NoChangesToPaymentAttempt()
    {
        // Arrange
        var paymentAttempt = new PaymentRequestPaymentAttempt();
        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var paymentRequestID = Guid.NewGuid();
        var amount = 15.15m;
        var capturedAmount = 5.00m;

        var cardEvents = new List<PaymentRequestEvent>
            {
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                    Status = "PENDING",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = amount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_webhook,
                    Status = "AUTHORIZED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                },
                new()
                {
                    ID = Guid.NewGuid(),
                    PaymentRequestID = paymentRequestID,
                    Amount = capturedAmount,
                    Currency = CurrencyTypeEnum.EUR,
                    Inserted = DateTimeOffset.UtcNow,
                    EventType = PaymentRequestEventTypesEnum.card_capture,
                    Status = "CAPTURED",
                    PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                    CardAuthorizationResponseID = cardAuthorizationResponseID,
                    CardRequestID = cardAuthorizationResponseID,
                }
            };

        var groupedCardEvents = cardEvents.GroupBy(e => e.CardAuthorizationResponseID);

        // Act
        groupedCardEvents.Single(x => x.Key == cardAuthorizationResponseID).HandleCardWebhookEvents(paymentAttempt);

        // Assert
        Assert.Empty(paymentAttempt.CaptureAttempts);
        Assert.Equal(amount, paymentAttempt.CardAuthorisedAmount);
        Assert.Equal(amount, paymentAttempt.AttemptedAmount);
        Assert.Equal(cardAuthorizationResponseID, paymentAttempt.AttemptKey);
        Assert.Equal(paymentRequestID, paymentAttempt.PaymentRequestID);
        Assert.Empty(paymentAttempt.RefundAttempts);
        Assert.Equal(PaymentResultEnum.FullyPaid, paymentAttempt.Status);
    }
}
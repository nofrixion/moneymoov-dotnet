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
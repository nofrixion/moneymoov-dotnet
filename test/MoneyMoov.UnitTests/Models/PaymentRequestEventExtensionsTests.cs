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
        
        Assert.Equal(1, groupedCardEvents.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardAuthorisationEvents(paymentAttempt);

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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardAuthorisationEvents(paymentAttempt);

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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardCaptureEvents(paymentAttempt);

            // Assert
            Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
            Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
            Assert.Equal(0, paymentAttempt.CaptureAttempts.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardCaptureEvents(paymentAttempt);

            // Assert
            Assert.Equal(1, paymentAttempt.CaptureAttempts.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardSaleEvents(paymentAttempt);

            // Assert
            Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
            Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
            Assert.Equal(0, paymentAttempt.CaptureAttempts.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardSaleEvents(paymentAttempt);

            // Assert
            Assert.Equal(1, paymentAttempt.CaptureAttempts.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardVoidEvents(paymentAttempt);

            // Assert
            Assert.Equal(string.Empty, paymentAttempt.AttemptKey);
            Assert.Equal(Guid.Empty, paymentAttempt.PaymentRequestID);
            Assert.Equal(0, paymentAttempt.RefundAttempts.Count);
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
            groupedCardEvents.Single(x=>x.Key == cardAuthorizationResponseID).HandleCardVoidEvents(paymentAttempt);

            // Assert
            Assert.Equal(1, paymentAttempt.RefundAttempts.Count);
        }
}
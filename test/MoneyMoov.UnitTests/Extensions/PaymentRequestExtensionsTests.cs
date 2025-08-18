// -----------------------------------------------------------------------------
//  Filename: PaymentRequestExtensionsTests.cs
// 
//  Description: Unit tests for the PaymentRequestExtensions.
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  29 Jul 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Extensions;

public class PaymentRequestExtensionsTests
{
    [Fact]
    public void PaymentRequestExtensions_GetPaymentAttempts_ReturnsDescendingByInitiatedAt()
    {
        // Arrange
        var paymentRequestEvents = new List<PaymentRequestEvent>()
        {
            new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = Guid.NewGuid(),
                Amount = 100,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PispPaymentInitiationID = "initiation1",
                PispPaymentServiceProviderID = "bankofireland"
            },
            new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = Guid.NewGuid(),
                Amount = 100,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-5),
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PispPaymentInitiationID = "initiation2",
                PispPaymentServiceProviderID = "bankofireland"
            },
            new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = Guid.NewGuid(),
                Amount = 100,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow.AddMinutes(-10),
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PispPaymentInitiationID = "initiation3",
                PispPaymentServiceProviderID = "bankofireland"
            }
        };

        // Act
        var result = paymentRequestEvents.GetPaymentAttempts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("initiation1", result[0].AttemptKey);
        Assert.Equal("initiation2", result[1].AttemptKey);
        Assert.Equal("initiation3", result[2].AttemptKey);
    }
}
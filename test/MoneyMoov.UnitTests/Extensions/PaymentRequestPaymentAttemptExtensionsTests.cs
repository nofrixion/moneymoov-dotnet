// -----------------------------------------------------------------------------
//  Filename: PaymentRequestPaymentAttemptExtensionsTests.cs
// 
//  Description: Tests for PaymentRequestPaymentAttemptExtensions.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  01 08 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Extensions;

public class PaymentRequestPaymentAttemptExtensionsTests
{
    // unit test for GetPaymentAttemptStatus
    [Theory]
    [InlineData(100, 100, 100, PaymentResultEnum.FullyPaid)]
    [InlineData(100, 200, 200, PaymentResultEnum.OverPaid)]
    [InlineData(100, 50, 50, PaymentResultEnum.PartiallyPaid)]
    [InlineData(100, 0,0, PaymentResultEnum.None)]
    [InlineData(100, 100,0, PaymentResultEnum.Authorized)]
    public void GetPaymentAttemptStatus_WhenPaymentMethodIsPisp_WithoutRefundAttempts(decimal attemptedAmount, decimal authorisedAmount, decimal settledAmount, PaymentResultEnum expectedResult)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.pisp,
            SettledAmount = settledAmount,
            SettledAt = settledAmount > 0 ? DateTime.UtcNow : null,
            AttemptedAmount = attemptedAmount,
            AuthorisedAt = authorisedAmount > 0 ? DateTime.UtcNow : null,
            AuthorisedAmount = authorisedAmount
        };

        // Act
        var result = paymentRequestPaymentAttempt.GetPaymentAttemptStatus();

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(100, 100, 100, 100, PaymentResultEnum.None)]
    [InlineData(100, 100, 100, 50, PaymentResultEnum.PartiallyPaid)]
    [InlineData(100, 100, 100, 0, PaymentResultEnum.FullyPaid)]
    [InlineData(100, 200, 200, 100, PaymentResultEnum.FullyPaid)]
    [InlineData(100, 50, 50, 50, PaymentResultEnum.None)]
    [InlineData(100, 0,0, 0, PaymentResultEnum.None)]
    [InlineData(100, 100,0, 0,PaymentResultEnum.Authorized)]
    public void GetPaymentAttemptStatus_WhenPaymentMethodIsPisp_WithRefundAttempts(decimal attemptedAmount, decimal authorisedAmount, decimal settledAmount, decimal refundedAmount, PaymentResultEnum expectedResult)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.pisp,
            AuthorisedAmount = authorisedAmount,
            AuthorisedAt = authorisedAmount > 0 ? DateTime.UtcNow : null,
            SettledAmount = settledAmount,
            SettledAt = settledAmount > 0 ? DateTime.UtcNow : null,
            AttemptedAmount = attemptedAmount,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                },
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                }
            }
        };

        // Act
        var result = paymentRequestPaymentAttempt.GetPaymentAttemptStatus();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(100, 100, 100, 100, PaymentResultEnum.None)]
    [InlineData(100, 100, 100, 50, PaymentResultEnum.PartiallyPaid)]
    [InlineData(100, 100, 100, 0, PaymentResultEnum.FullyPaid)]
    [InlineData(100, 200, 200, 100, PaymentResultEnum.FullyPaid)]
    [InlineData(100, 50, 50, 50, PaymentResultEnum.None)]
    [InlineData(100, 0, 0, 0, PaymentResultEnum.None)]
    public void GetPaymentAttemptStatus_WhenPaymentMethodIsCard_WithRefundAttempts(decimal attemptedAmount,
        decimal cardAuthorisedAmount, decimal capturedAmount, decimal refundedAmount, PaymentResultEnum expectedResult)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = cardAuthorisedAmount,
            CardAuthorisedAt = cardAuthorisedAmount > 0 ? DateTime.UtcNow : null,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt>()
                { new() { CapturedAmount = capturedAmount / 2 }, new() { CapturedAmount = capturedAmount / 2 } },
            AttemptedAmount = attemptedAmount,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                },
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                }
            }
        };
        
        // Act
        var result = paymentRequestPaymentAttempt.GetPaymentAttemptStatus();
        
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(100, 100, 0)]
    [InlineData(100, 50, 50)]
    [InlineData(100, 0, 100)]
    public void GetAmountAvailableToRefund_Tests(decimal capturedAmount, decimal refundedAmount, decimal result)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = capturedAmount * 2,
            CardAuthorisedAt = capturedAmount > 0 ? DateTime.UtcNow : null,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt>()
                { new() { CapturedAmount = capturedAmount / 2 }, new() { CapturedAmount = capturedAmount / 2 } },
            AttemptedAmount = capturedAmount * 2,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                },
                new()
                {
                    RefundSettledAmount = refundedAmount / 2
                },
            }
        };

        if (refundedAmount < capturedAmount)
        {
            paymentRequestPaymentAttempt.RefundAttempts.Add(new PaymentRequestRefundAttempt { RefundSettledAmount = 100, IsCardVoid = true});
        }
        
        // Act
        var amountAvailableToRefund = paymentRequestPaymentAttempt.GetAmountAvailableToRefund();
        
        // Assert
        Assert.Equal(result, amountAvailableToRefund);
    }
    
    /// <summary>
    /// Tests that a pending refund (initiated but not yet settled or declined) blocks the
    /// initiated amount from being refunded again, preventing double-refunds.
    /// </summary>
    [Fact]
    public void GetAmountAvailableToRefund_PendingRefund_BlocksAmount()
    {
        // Arrange: captured 100, one pending refund of 40 in flight
        var attempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = 200M,
            CardAuthorisedAt = DateTime.UtcNow,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt> { new() { CapturedAmount = 100M } },
            AttemptedAmount = 200M,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundInitiatedAt = DateTime.UtcNow,
                    RefundInitiatedAmount = 40M,
                    RefundSettledAmount = 0M,
                    RefundCancelledAmount = 0M,
                    IsCardVoid = false
                }
            }
        };

        // Act
        var available = attempt.GetAmountAvailableToRefund();

        // Assert: 100 captured - 40 pending = 60 available
        Assert.Equal(60M, available);
    }

    /// <summary>
    /// Tests that a pending refund that was subsequently declined releases its amount back,
    /// making the full captured amount available to refund again.
    /// </summary>
    [Fact]
    public void GetAmountAvailableToRefund_DeclinedRefund_ReleasesAmount()
    {
        // Arrange: captured 100, refund of 40 was initiated then declined
        var attempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = 200M,
            CardAuthorisedAt = DateTime.UtcNow,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt> { new() { CapturedAmount = 100M } },
            AttemptedAmount = 200M,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundInitiatedAt = DateTime.UtcNow,
                    RefundInitiatedAmount = 40M,
                    RefundSettledAmount = 0M,
                    RefundCancelledAt = DateTime.UtcNow,
                    RefundCancelledAmount = 40M,
                    IsCardVoid = false
                }
            }
        };

        // Act
        var available = attempt.GetAmountAvailableToRefund();

        // Assert: declined refund does not block — full 100 captured is available
        Assert.Equal(100M, available);
    }

    /// <summary>
    /// Tests that a combination of a settled refund and a pending refund both reduce
    /// the available amount to refund correctly.
    /// </summary>
    [Fact]
    public void GetAmountAvailableToRefund_PendingAndSettledCombined_BothReduce()
    {
        // Arrange: captured 100, 30 already settled, 20 pending
        var attempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = 200M,
            CardAuthorisedAt = DateTime.UtcNow,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt> { new() { CapturedAmount = 100M } },
            AttemptedAmount = 200M,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                // settled refund
                new()
                {
                    RefundInitiatedAt = DateTime.UtcNow.AddMinutes(-10),
                    RefundInitiatedAmount = 30M,
                    RefundSettledAt = DateTime.UtcNow.AddMinutes(-5),
                    RefundSettledAmount = 30M,
                    RefundCancelledAmount = 0M,
                    IsCardVoid = false
                },
                // pending refund
                new()
                {
                    RefundInitiatedAt = DateTime.UtcNow,
                    RefundInitiatedAmount = 20M,
                    RefundSettledAmount = 0M,
                    RefundCancelledAmount = 0M,
                    IsCardVoid = false
                }
            }
        };

        // Act
        var available = attempt.GetAmountAvailableToRefund();

        // Assert: 100 - 30 settled - 20 pending = 50 available
        Assert.Equal(50M, available);
    }

    [Theory]
    [InlineData(200.45,100.55,  0, 99.90)]
    [InlineData(200.45, 100.55,  99.90, 0)]
    public void GetAmountAvailableToVoid_Tests(decimal cardAuthorisedAmount, decimal capturedAmount, decimal voidedAmount, decimal result)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = cardAuthorisedAmount,
            CardAuthorisedAt = cardAuthorisedAmount > 0 ? DateTime.UtcNow : null,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt>()
                { new() { CapturedAmount = capturedAmount / 2 }, new() { CapturedAmount = capturedAmount / 2 } },
            AttemptedAmount = cardAuthorisedAmount,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundSettledAmount = voidedAmount,
                    IsCardVoid = true
                },
            }
        };
        
        // Act
        var amountAvailableToVoid = paymentRequestPaymentAttempt.GetAmountAvailableToVoid();
        
        // Assert
        Assert.Equal(result, amountAvailableToVoid);
    }
    
    [Theory]
    [InlineData(100,  0, false)]
    [InlineData(100,  100, true)]
    public void IsCardPaymentVoided_Tests(decimal capturedAmount, decimal voidedAmount, bool result)
    {
        // Arrange
        var paymentRequestPaymentAttempt = new PaymentRequestPaymentAttempt
        {
            PaymentMethod = PaymentMethodTypeEnum.card,
            CardAuthorisedAmount = capturedAmount * 2,
            CardAuthorisedAt = capturedAmount > 0 ? DateTime.UtcNow : null,
            CaptureAttempts = new List<PaymentRequestCaptureAttempt>()
                { new() { CapturedAmount = capturedAmount / 2 }, new() { CapturedAmount = capturedAmount / 2 } },
            AttemptedAmount = capturedAmount * 2,
            RefundAttempts = new List<PaymentRequestRefundAttempt>
            {
                new()
                {
                    RefundSettledAmount = voidedAmount,
                    IsCardVoid = true
                },
            }
        };
        
        // Act
        var isCardPaymentVoided = paymentRequestPaymentAttempt.IsCardPaymentVoided();
        
        // Assert
        Assert.Equal(result, isCardPaymentVoided);
    }
}
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
}
// -----------------------------------------------------------------------------
//  Filename: PaymentRequestPaymentAttemptExtensions.cs
// 
//  Description: Extension methods for PaymentRequestPaymentAttempt.
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

using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class PaymentRequestPaymentAttemptExtensions
{
    public static PaymentResultEnum GetPaymentAttemptStatus(this PaymentRequestPaymentAttempt attempt)
    {
        var amountReceived = attempt switch
        {
            var att when att.PaymentMethod == PaymentMethodTypeEnum.pisp && att.SettledAmount > 0 => att
                .SettledAmount,
            var att when att.PaymentMethod == PaymentMethodTypeEnum.lightning && att.SettledAmount > 0 => att
                .SettledAmount,
            var att when att.PaymentMethod == PaymentMethodTypeEnum.card && att.CardAuthorisedAmount > 0 => att
                .CardAuthorisedAmount,
            var att when att.PaymentMethod == PaymentMethodTypeEnum.directDebit && att.SettledAmount > 0 => att
                .SettledAmount,
            _ => 0
        };

        var amountRefunded = attempt.RefundAttempts.Sum(x => x.RefundSettledAmount);

        var amountPaid = amountReceived - amountRefunded;

        return attempt switch
        {
            var att when amountPaid > 0 && amountPaid == att.AttemptedAmount => PaymentResultEnum.FullyPaid,
            var att when amountPaid > 0 && amountPaid > att.AttemptedAmount => PaymentResultEnum.OverPaid,
            var att when amountPaid > 0 && amountPaid < att.AttemptedAmount => PaymentResultEnum.PartiallyPaid,
            var att when att.SettleFailedAt != null => PaymentResultEnum.None,
            var att when att.SettledAt == null && att.AuthorisedAt != null => PaymentResultEnum.Authorized,
            _ => PaymentResultEnum.None
        };
    }

    public static bool IsCardPaymentVoided(this PaymentRequestPaymentAttempt attempt)
    {
        return (attempt.CardAuthorisedAmount -
                attempt.CaptureAttempts.Sum(y => y.CapturedAmount) -
                attempt.RefundAttempts.Where(p => p.IsCardVoid)
                    .Sum(z => z.RefundSettledAmount)) == 0;
    }

    public static decimal GetAmountAvailableToRefund(this PaymentRequestPaymentAttempt attempt)
    {
        return attempt.CaptureAttempts.Sum(x => x.CapturedAmount) -
                                       attempt.RefundAttempts.Where(x => x.IsCardVoid == false)
                                           .Sum(y => y.RefundSettledAmount);
    }
    
    public static decimal GetAmountAvailableToVoid(this PaymentRequestPaymentAttempt attempt)
    {
        return attempt.CardAuthorisedAmount -
               attempt.CaptureAttempts.Sum(x => x.CapturedAmount) -
               attempt.RefundAttempts.Where(x => x.IsCardVoid)
                   .Sum(y => y.RefundSettledAmount);
    }

    /// <summary>
    /// Used to check whether a pay by bank (pisp) attempt has expired after geing authorised.
    /// The expiry occurs if the funds don't arrive into the destination account within the prescribed
    /// period (e.g. 2 business days)
    /// </summary>
    /// <param name="attempt">The payment attempt to check the pay by bank expiry for.</param>
    /// <param name="expiresAt">The point the pay by bank attempt should expire At.</param>
    /// <returns>True if the attempt meets the criteria for expiry. False if not.</returns>
    public static bool IsPayByBankExpired(this PaymentRequestPaymentAttempt attempt, DateTimeOffset expiresAt)
    {
        if(attempt.PaymentMethod != PaymentMethodTypeEnum.pisp)
        {
            return false;
        }

        int secondsToExpiry = expiresAt.Subtract(DateTimeOffset.UtcNow).Seconds;

        return secondsToExpiry < PaymentRequestPaymentAttempt.PAYBYBANK_EXPIRY_MARGIN_SECONDS;
    }
}
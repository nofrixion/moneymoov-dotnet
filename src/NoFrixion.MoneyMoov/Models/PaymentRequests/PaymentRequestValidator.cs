//-----------------------------------------------------------------------------
// Filename: PaymentRequestValidator.cs
//
// Description: Common logic for validating payment request related classes.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Nov 2022  Aaron Clauson   Refactored from PaymentRequestValidatorService.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public static class PaymentRequestValidator
{
    public static bool ValidatePaymentRequestCurrency(CurrencyTypeEnum currency, PaymentMethodTypeEnum paymentMethodTypes)
    {
        return !(currency == CurrencyTypeEnum.LBTC &&
               (paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.card) ||
                paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp)));
    }

    public static IEnumerable<ValidationResult> Validate(
        IPaymentRequest paymentRequest,
        ValidationContext validationContext)
    {
        if (paymentRequest.PaymentMethodTypes == PaymentMethodTypeEnum.None)
        {
            yield return new ValidationResult($"At least one payment method type must be specified.", new string[] { nameof(paymentRequest.PaymentMethodTypes) });
        }
        if (paymentRequest.PaymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp) &&
            paymentRequest.Currency == CurrencyTypeEnum.EUR &&
            !PayoutsValidator.ValidatePaymentRequestAmount(paymentRequest.Amount, paymentRequest.PaymentMethodTypes, paymentRequest.Currency))
        {
            yield return new ValidationResult($"The amount was invalid. If a PISP payment method is being used, the amount must be at least EUR {PaymentsConstants.PISP_MINIMUM_EUR_PAYMENT_AMOUNT}.", new string[] { nameof(paymentRequest.Amount) });
        }
        else if (paymentRequest.PaymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp) &&
            paymentRequest.Currency == CurrencyTypeEnum.GBP &&
            !PayoutsValidator.ValidatePaymentRequestAmount(paymentRequest.Amount, paymentRequest.PaymentMethodTypes, paymentRequest.Currency))
        {
            yield return new ValidationResult($"The amount was invalid. If a PISP payment method is being used, the amount must be at least GBP {PaymentsConstants.PISP_MINIMUM_GBP_PAYMENT_AMOUNT}.", new string[] { nameof(paymentRequest.Amount) });
        }
        else if (paymentRequest.PaymentMethodTypes.HasFlag(PaymentMethodTypeEnum.card) && !PayoutsValidator.ValidatePaymentRequestAmount(paymentRequest.Amount, paymentRequest.PaymentMethodTypes, paymentRequest.Currency))
        {
            yield return new ValidationResult($"The amount was invalid. If a card payment method is being used, the amount must be at least {PaymentsConstants.CARD_MINIMUM_PAYMENT_AMOUNT}.", new string[] { nameof(paymentRequest.Amount) });
        }
        else if (paymentRequest.PaymentMethodTypes.HasFlag(PaymentMethodTypeEnum.cardtoken) && !PayoutsValidator.ValidatePaymentRequestAmount(paymentRequest.Amount, paymentRequest.PaymentMethodTypes, paymentRequest.Currency))
        {
            yield return new ValidationResult($"The amount was invalid. If a card token payment method is being used, the amount must be at least {PaymentsConstants.CARD_MINIMUM_PAYMENT_AMOUNT}.", new string[] { nameof(paymentRequest.Amount) });
        }

        if (!ValidatePaymentRequestCurrency(paymentRequest.Currency, paymentRequest.PaymentMethodTypes))
        {
            yield return new ValidationResult($"One or more of the payment methods specified are not compatible with the {paymentRequest.Currency} currency.",
                new string[] { nameof(PaymentMethodTypeEnum) });
        }

        if (paymentRequest.Amount == decimal.Zero && paymentRequest.PaymentMethodTypes != PaymentMethodTypeEnum.card)
        {
            yield return new ValidationResult($"The amount can only be set to 0 when the payment methods are set to a single option of card.",
                new string[] { nameof(paymentRequest.Amount) });
        }

        if (!string.IsNullOrEmpty(paymentRequest.BaseOriginUrl))
        {
            if (!Uri.TryCreate(paymentRequest.BaseOriginUrl, UriKind.Absolute, out var uriResult))
            {
                yield return new ValidationResult($"The {nameof(paymentRequest.BaseOriginUrl)} was not recognised as a valid URL. It must be in the format https://localhost.",
                 new string[] { nameof(paymentRequest.BaseOriginUrl) });
            }
            else
            {
                // The BaseOriginUrl was a valid URL, do some additional checks.
                if (uriResult.Segments.Count() > 1)
                {
                    yield return new ValidationResult($"The {nameof(paymentRequest.BaseOriginUrl)} had extra segments. it must be in the format https://localhost without any additional path segments.",
                        new string[] { nameof(paymentRequest.BaseOriginUrl) });
                }

                if (uriResult.Scheme != "https")
                {
                    yield return new ValidationResult($"The {nameof(paymentRequest.BaseOriginUrl)} must be an https URL in the format https://localhost.",
                        new string[] { nameof(paymentRequest.BaseOriginUrl) });
                }
            }
        }

        // If callback URL is not set default to the hosted payment page result. AC 16 Nov 2022.
        //if (!paymentRequest.UseHostedPaymentPage && IsEmptyUrlWithCard(paymentRequest.CallbackUrl, paymentRequest.PaymentMethodTypes))
        //{
        //    yield return new ValidationResult($"The {nameof(paymentRequest.CallbackUrl)} cannot be empty",
        //        new string[] { nameof(paymentRequest.CallbackUrl) });
        //}

        if (!string.IsNullOrEmpty(paymentRequest.CallbackUrl))
        {
            if (!Uri.TryCreate(paymentRequest.CallbackUrl, UriKind.Absolute, out var uriResult))
            {
                yield return new ValidationResult($"The {nameof(paymentRequest.CallbackUrl)} was not recognised as a valid URL. It must be in the format https://localhost.",
                 new string[] { nameof(paymentRequest.CallbackUrl) });
            }
        }

        if (!string.IsNullOrEmpty(paymentRequest.SuccessWebHookUrl))
        {
            if (!Uri.TryCreate(paymentRequest.SuccessWebHookUrl, UriKind.Absolute, out var uriResult))
            {
                yield return new ValidationResult($"The {nameof(paymentRequest.SuccessWebHookUrl)} was not recognised as a valid URL. It must be in the format http://localhost.",
                 new string[] { nameof(paymentRequest.SuccessWebHookUrl) });
            }
        }

        if (paymentRequest.CardCreateToken && string.IsNullOrEmpty(paymentRequest.CustomerEmailAddress))
        {
            yield return new ValidationResult($"The {nameof(paymentRequest.CustomerEmailAddress)} must be set when the {nameof(paymentRequest.CardCreateToken)} is set.",
                new string[] { nameof(paymentRequest.CustomerEmailAddress) });
        }
    }
}


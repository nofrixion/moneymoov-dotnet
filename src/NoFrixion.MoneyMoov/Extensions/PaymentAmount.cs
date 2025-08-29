//-----------------------------------------------------------------------------
// Filename: PaymentAmount.cs
// 
// Description: Helper and extension methods for dealing with payment amounts.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Oct 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Globalization;

namespace NoFrixion.MoneyMoov;

public static class PaymentAmount
{
    /// <summary>
    /// Combines the display currency symbol and amount.
    /// </summary>
    public static string DisplayCurrencyAndAmount(CurrencyTypeEnum currency, decimal amount) =>
        currency.GetCurrencySymbol() + " " + GetDisplayAmount(currency, amount);

    // Decide decimals once, reuse everywhere.
    private static int GetDecimalPlaces(CurrencyTypeEnum currency, decimal amount)
    {
        if (!currency.IsFiat())
        {
            return PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES;
        }

        return decimal.Remainder(decimal.Abs(amount), 0.01m) != 0m
            ? PaymentsConstants.FIAT_INTERNAL_ROUNDING_DECIMAL_PLACES
            : PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES;
    }

    public static string GetDisplayAmount(CurrencyTypeEnum currency, decimal amount, IFormatProvider? culture = null)
    {
        var dps = GetDecimalPlaces(currency, amount);
        return amount.ToString($"N{dps}", culture ?? CultureInfo.CurrentCulture);
    }

    public static decimal GetRoundedAmount(CurrencyTypeEnum currency, decimal amount, MidpointRounding mode = MidpointRounding.ToEven)
    {
        var dps = GetDecimalPlaces(currency, amount);
        return Math.Round(amount, dps, mode);
    }
}


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

namespace NoFrixion.MoneyMoov;

public static class PaymentAmount
{
    /// <summary>
    /// Combines the display currency symbol and amount.
    /// </summary>
    public static string DisplayCurrencyAndAmount(CurrencyTypeEnum currency, decimal amount) =>
        GetCurrencySymbol(currency) +  GetDisplayAmount(currency, amount);

    public static string GetDisplayAmount(CurrencyTypeEnum currency, decimal amount) =>
        IsFiat(currency) ? amount.ToString("N2") : amount.ToString("N8");

    public static decimal GetRoundedAmount(CurrencyTypeEnum currency, decimal amount) =>
         Math.Round(amount, IsFiat(currency) ? PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES : PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES);

    public static string GetCurrencySymbol(CurrencyTypeEnum currency) =>
    currency switch
    {
        CurrencyTypeEnum.BTC => "₿",
        CurrencyTypeEnum.GBP => "£",
        CurrencyTypeEnum.EUR => "€",
        _ => "€"
    };

    private static bool IsFiat(CurrencyTypeEnum currency) =>
        currency switch
        {
            CurrencyTypeEnum.BTC => false,
            _ => true
        };
}


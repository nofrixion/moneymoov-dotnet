//-----------------------------------------------------------------------------
// Filename: CurrencyExtensions.cs
// 
// Description: Contains extension methods for the Currency enum.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 23 Oct 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// MIT
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class CurrencyExtensions
{
    public static string GetCurrencySymbol(this CurrencyTypeEnum currency) =>
    currency switch
    {
        CurrencyTypeEnum.BTC => "₿",
        CurrencyTypeEnum.GBP => "£",
        CurrencyTypeEnum.EUR => "€",
        _ => "€"
    };

    public static bool IsFiat(this CurrencyTypeEnum currency) =>
        currency switch
        {
            CurrencyTypeEnum.BTC => false,
            _ => true
        };

    public static int GetDecimalPlaces(this CurrencyTypeEnum currency) =>
        currency switch
        {
            CurrencyTypeEnum.BTC => PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES,
            _ => PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES
        };
}
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
// 29 Apr 2026  Constantine Nalimov   Updated for supported currencies.
// 
// License:
// MIT
//-----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Constants;

namespace NoFrixion.MoneyMoov;

public static class CurrencyExtensions
{
    public static string GetCurrencySymbol(this CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency).Symbol ;

    public static bool IsFiat(this CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency).IsFiat;

    public static string Iso4217AlphaCode(this CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency).Iso4217AlphaCode;

    public static string Iso4217NumericCode(this CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency).Iso4217NumericCode;

    public static int DefaultDecimals(this CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency).Decimals;
}
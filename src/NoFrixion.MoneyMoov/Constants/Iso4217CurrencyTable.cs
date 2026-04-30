//-----------------------------------------------------------------------------
// Filename: Iso4217CurrencyTable.cs
// 
// Description: Canonical table of static currency metadata.
// 
// Author(s):
// Constantine Nalimov (constantine.nalimov@nofrixion.com)
// 
// History:
// 22 Apr 2026  Constantine Nalimov   Created, Prague, CZ.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models.Currency;

namespace NoFrixion.MoneyMoov.Constants;

public static class Iso4217CurrencyTable
{
    private static readonly CurrencyInfo NoneCurrencyInfo = new(
        CurrencyTypeEnum.None,
        "NONE",
        string.Empty,
        0,
        "?");

    private static readonly IReadOnlyDictionary<CurrencyTypeEnum, CurrencyInfo> Currencies =
        new Dictionary<CurrencyTypeEnum, CurrencyInfo>
        {
            [CurrencyTypeEnum.None] = NoneCurrencyInfo,
            [CurrencyTypeEnum.GBP] = new(CurrencyTypeEnum.GBP, "GBP", "826", 2, "£"),
            [CurrencyTypeEnum.EUR] = new(CurrencyTypeEnum.EUR, "EUR", "978", 2, "€"),
            [CurrencyTypeEnum.USD] = new(CurrencyTypeEnum.USD, "USD", "840", 2, "$"),
            [CurrencyTypeEnum.AUD] = new(CurrencyTypeEnum.AUD, "AUD", "036", 2, "$"),
            [CurrencyTypeEnum.BGN] = new(CurrencyTypeEnum.BGN, "BGN", "975", 2, "лв"),
            [CurrencyTypeEnum.CAD] = new(CurrencyTypeEnum.CAD, "CAD", "124", 2, "$"),
            [CurrencyTypeEnum.CZK] = new(CurrencyTypeEnum.CZK, "CZK", "203", 2, "Kč"),
            [CurrencyTypeEnum.DKK] = new(CurrencyTypeEnum.DKK, "DKK", "208", 2, "kr"),
            [CurrencyTypeEnum.HUF] = new(CurrencyTypeEnum.HUF, "HUF", "348", 2, "Ft"),
            [CurrencyTypeEnum.ISK] = new(CurrencyTypeEnum.ISK, "ISK", "352", 0, "kr"),
            [CurrencyTypeEnum.CHF] = new(CurrencyTypeEnum.CHF, "CHF", "756", 2, "CHF"),
            [CurrencyTypeEnum.NOK] = new(CurrencyTypeEnum.NOK, "NOK", "578", 2, "kr"),
            [CurrencyTypeEnum.PLN] = new(CurrencyTypeEnum.PLN, "PLN", "985", 2, "zł"),
            [CurrencyTypeEnum.RON] = new(CurrencyTypeEnum.RON, "RON", "946", 2, "lei"),
            [CurrencyTypeEnum.BTC] = new(CurrencyTypeEnum.BTC, "BTC", string.Empty, PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES, "₿")
        };

    public static IReadOnlyDictionary<CurrencyTypeEnum, CurrencyInfo> All => Currencies;

    public static CurrencyInfo GetInfo(CurrencyTypeEnum currency) =>
        Currencies.TryGetValue(currency, out var info)
            ? info
            : NoneCurrencyInfo;
}

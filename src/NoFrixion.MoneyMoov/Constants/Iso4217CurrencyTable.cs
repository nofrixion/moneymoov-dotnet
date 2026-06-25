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
// 23 Jun 2026  Axel Granillo         Updated for supported currencies.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models.Currency;

namespace NoFrixion.MoneyMoov.Constants;
#pragma warning disable CS0618
public static class Iso4217CurrencyTable
{
    private static readonly CurrencyInfo NoneCurrencyInfo = new(
        CurrencyTypeEnum.None,
        "NONE",
        string.Empty,
        0,
        "?",
        true);

    private static readonly IReadOnlyDictionary<CurrencyTypeEnum, CurrencyInfo> Currencies =
        new Dictionary<CurrencyTypeEnum, CurrencyInfo>
        {
            [CurrencyTypeEnum.None] = NoneCurrencyInfo,
            [CurrencyTypeEnum.GBP] = new(CurrencyTypeEnum.GBP, "GBP", "826", 2, "£", true),
            [CurrencyTypeEnum.EUR] = new(CurrencyTypeEnum.EUR, "EUR", "978", 2, "€", true),
            [CurrencyTypeEnum.USD] = new(CurrencyTypeEnum.USD, "USD", "840", 2, "$", true),
            [CurrencyTypeEnum.AUD] = new(CurrencyTypeEnum.AUD, "AUD", "036", 2, "$", true),
            [CurrencyTypeEnum.BGN] = new(CurrencyTypeEnum.BGN, "BGN", "975", 2, "лв", true),
            [CurrencyTypeEnum.CAD] = new(CurrencyTypeEnum.CAD, "CAD", "124", 2, "$", true),
            [CurrencyTypeEnum.CZK] = new(CurrencyTypeEnum.CZK, "CZK", "203", 2, "Kč", true),
            [CurrencyTypeEnum.DKK] = new(CurrencyTypeEnum.DKK, "DKK", "208", 2, "kr", true),
            [CurrencyTypeEnum.HUF] = new(CurrencyTypeEnum.HUF, "HUF", "348", 2, "Ft", true),
            [CurrencyTypeEnum.ISK] = new(CurrencyTypeEnum.ISK, "ISK", "352", 0, "kr", true),
            [CurrencyTypeEnum.CHF] = new(CurrencyTypeEnum.CHF, "CHF", "756", 2, "CHF", true),
            [CurrencyTypeEnum.NOK] = new(CurrencyTypeEnum.NOK, "NOK", "578", 2, "kr", true),
            [CurrencyTypeEnum.PLN] = new(CurrencyTypeEnum.PLN, "PLN", "985", 2, "zł", true),
            [CurrencyTypeEnum.RON] = new(CurrencyTypeEnum.RON, "RON", "946", 2, "lei", true),
            [CurrencyTypeEnum.AED] = new(CurrencyTypeEnum.AED, "AED", "784", 2, "د.إ", true),
            [CurrencyTypeEnum.CNH] = new(CurrencyTypeEnum.CNH, "CNH", "156", 2, "¥", true), // CNH has no official ISO 4217 numeric code; 156 (CNY) used by convention
            [CurrencyTypeEnum.HKD] = new(CurrencyTypeEnum.HKD, "HKD", "344", 2, "HK$", true),
            [CurrencyTypeEnum.ILS] = new(CurrencyTypeEnum.ILS, "ILS", "376", 2, "₪", true),
            [CurrencyTypeEnum.JPY] = new(CurrencyTypeEnum.JPY, "JPY", "392", 0, "¥", true),
            [CurrencyTypeEnum.MXN] = new(CurrencyTypeEnum.MXN, "MXN", "484", 2, "$", true),
            [CurrencyTypeEnum.NZD] = new(CurrencyTypeEnum.NZD, "NZD", "554", 2, "NZ$", true),
            [CurrencyTypeEnum.SAR] = new(CurrencyTypeEnum.SAR, "SAR", "682", 2, "﷼", true),
            [CurrencyTypeEnum.SEK] = new(CurrencyTypeEnum.SEK, "SEK", "752", 2, "kr", true),
            [CurrencyTypeEnum.SGD] = new(CurrencyTypeEnum.SGD, "SGD", "702", 2, "S$", true),
            [CurrencyTypeEnum.TRY] = new(CurrencyTypeEnum.TRY, "TRY", "949", 2, "₺", true),
            [CurrencyTypeEnum.ZAR] = new(CurrencyTypeEnum.ZAR, "ZAR", "710", 2, "R", true),
            [CurrencyTypeEnum.BTC] = new(CurrencyTypeEnum.BTC, "BTC", string.Empty, PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES, "₿", false)
        };

    public static IReadOnlyDictionary<CurrencyTypeEnum, CurrencyInfo> All => Currencies;

    public static CurrencyInfo GetInfo(CurrencyTypeEnum currency) =>
        Currencies.TryGetValue(currency, out var info)
            ? info
            : NoneCurrencyInfo;
}
#pragma warning restore CS0618
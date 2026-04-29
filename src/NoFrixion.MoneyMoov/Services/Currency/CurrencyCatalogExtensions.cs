//-----------------------------------------------------------------------------
// Filename: CurrencyCatalogExtensions.cs
// 
// Description: Formatting helpers for currency capability lookups.
// 
// Author(s):
// Oz (oz-agent@warp.dev)
// 
// History:
// 29 Apr 2026  Oz Created, Warp.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Services.Currency;

public static class CurrencyCatalogExtensions
{
    public static string GetSupportedCodesText(
        this ICurrencyCatalog currencyCatalog,
        CurrencyCapability capability)
    {
        ArgumentNullException.ThrowIfNull(currencyCatalog);

        var supported = currencyCatalog
            .GetSupported(capability)
            .Select(info => info.Code.ToString())
            .OrderBy(code => code)
            .ToList();

        return supported.Count > 0
            ? string.Join(", ", supported)
            : "none";
    }
}

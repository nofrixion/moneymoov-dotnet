//-----------------------------------------------------------------------------
// Filename: CurrencyCatalogAccessor.cs
// 
// Description: Static accessor for the currency catalog used by validation layers.
// 
// Author(s):
// Constantine Nalimov (constantine.nalimov@nofrixion.com)
// 
// History:
// 23 Apr 2026  Constantine Nalimov   Created, Prague, CZ.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System;

namespace NoFrixion.MoneyMoov.Services.Currency;

public static class CurrencyCatalogAccessor
{
    private static ICurrencyCatalog? _catalog;

    public static ICurrencyCatalog? Catalog => _catalog;

    public static void Set(ICurrencyCatalog catalog)
    {
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    public static void Clear()
    {
        _catalog = null;
    }
}

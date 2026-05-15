//-----------------------------------------------------------------------------
// Filename: CurrencyInfo.cs
// 
// Description: Describes static currency metadata.
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

namespace NoFrixion.MoneyMoov.Models.Currency;

public record CurrencyInfo(
    CurrencyTypeEnum Code,
    string Iso4217AlphaCode,
    string Iso4217NumericCode,
    int Decimals,
    string Symbol
);

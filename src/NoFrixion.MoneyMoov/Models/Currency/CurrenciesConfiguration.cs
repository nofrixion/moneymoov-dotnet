//-----------------------------------------------------------------------------
// Filename: CurrenciesConfiguration.cs
// 
// Description: Configuration model for currency capabilities.
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

public class CurrenciesConfiguration
{
    public IReadOnlyList<CurrencyTypeEnum> Holding { get; set; } = [];

    public IReadOnlyList<CurrencyTypeEnum> IncomingBc { get; set; } = [];

    public IReadOnlyList<CurrencyTypeEnum> FxBc { get; set; } = [];
}

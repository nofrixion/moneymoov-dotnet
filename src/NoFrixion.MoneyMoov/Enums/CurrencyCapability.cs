//-----------------------------------------------------------------------------
// Filename: CurrencyCapability.cs
// 
// Description: Capability flags for how currencies are supported by the platform.
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

namespace NoFrixion.MoneyMoov;

[Flags]
public enum CurrencyCapability
{
    None = 0,
    Holding = 1,
    IncomingBc = 2,
    FxBc = 4
}

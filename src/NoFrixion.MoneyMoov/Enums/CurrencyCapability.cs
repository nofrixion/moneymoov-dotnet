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

/// <summary>
/// Capability flags for how currencies are supported by the platform.
/// </summary>
[Flags]
public enum CurrencyCapability
{
    /// <summary>
    /// No capability flags are selected. Using this value as a filter returns no currencies.
    /// </summary>
    None = 0,

    /// <summary>
    /// Currency is supported for held balances and payment accounts.
    /// </summary>
    Holding = 1,

    /// <summary>
    /// Currency is supported for inbound payment flows.
    /// </summary>
    Inbound = 2,

    /// <summary>
    /// Currency is supported for FX conversion flows.
    /// </summary>
    FxConversion = 4
}

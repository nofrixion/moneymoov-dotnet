//-----------------------------------------------------------------------------
// Filename: ICurrencyCatalog.cs
// 
// Description: Abstraction for resolving configured currency capabilities.
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

namespace NoFrixion.MoneyMoov.Services.Currency;

/// <summary>
/// Abstraction for resolving configured currency capabilities and metadata.
/// </summary>
public interface ICurrencyCatalog
{
    /// <summary>
    /// Checks if a specific currency supports all the requested capabilities.
    /// </summary>
    /// <param name="currency">The currency to check.</param>
    /// <param name="capability">The capability flags to check for. If multiple flags are provided,
    /// the currency must support ALL of them (Intersection/AND logic).</param>
    /// <returns>True if the currency supports all requested capabilities; otherwise, false.</returns>
    bool IsSupportedFor(CurrencyTypeEnum currency, CurrencyCapability capability);
    /// <summary>
    /// Retrieves a list of currencies that support the specified capabilities.
    /// </summary>
    /// <param name="capability">The capability flags to filter by. If multiple flags are provided,
    /// only currencies that satisfy ALL flags are returned (Intersection/AND logic).</param>
    /// <returns>A list of <see cref="CurrencyInfo"/> objects for the matching currencies.</returns>
    IReadOnlyList<CurrencyInfo> GetSupported(CurrencyCapability capability);
    /// <summary>
    /// Gets all known currencies excluding <see cref="CurrencyTypeEnum.None"/>.
    /// </summary>
    /// <returns>A list of <see cref="CurrencyInfo"/> objects for all currencies.</returns>
    IReadOnlyList<CurrencyInfo> GetAll();
    /// <summary>
    /// Gets the static metadata for a currency.
    /// </summary>
    /// <param name="currency">The currency to retrieve metadata for.</param>
    /// <returns>The <see cref="CurrencyInfo"/> for the supplied currency.</returns>
    CurrencyInfo GetInfo(CurrencyTypeEnum currency);
}

//-----------------------------------------------------------------------------
// Filename: CurrencyCatalog.cs
// 
// Description: Provides capability lookups for configured currencies.
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

using Microsoft.Extensions.Options;
using NoFrixion.MoneyMoov.Constants;
using NoFrixion.MoneyMoov.Models.Currency;

namespace NoFrixion.MoneyMoov.Services.Currency;

public class CurrencyCatalog : ICurrencyCatalog
{
    private readonly IReadOnlyList<CurrencyTypeEnum> _holding;
    private readonly IReadOnlyList<CurrencyTypeEnum> _incomingBc;
    private readonly IReadOnlyList<CurrencyTypeEnum> _fxBc;

    private readonly HashSet<CurrencyTypeEnum> _holdingSet;
    private readonly HashSet<CurrencyTypeEnum> _incomingBcSet;
    private readonly HashSet<CurrencyTypeEnum> _fxBcSet;

    public CurrencyCatalog(IOptions<CurrenciesConfiguration> options)
    {
        var config = options?.Value ?? new CurrenciesConfiguration();

        _holding = BuildUniqueList(nameof(config.Holding), config.Holding);
        _incomingBc = BuildUniqueList(nameof(config.IncomingBc), config.IncomingBc);
        _fxBc = BuildUniqueList(nameof(config.FxBc), config.FxBc);

        _holdingSet = _holding.ToHashSet();
        _incomingBcSet = _incomingBc.ToHashSet();
        _fxBcSet = _fxBc.ToHashSet();
    }

    /// <summary>
    /// Checks if a specific currency supports all the requested capabilities.
    /// </summary>
    /// <param name="currency">The currency to check.</param>
    /// <param name="capability">The capability flags to check for. If multiple flags are provided, 
    /// the currency must support ALL of them (Intersection/AND logic).</param>
    /// <returns>True if the currency supports all requested capabilities; otherwise, false.</returns>
    public bool IsSupportedFor(CurrencyTypeEnum currency, CurrencyCapability capability)
    {
        if (capability == CurrencyCapability.None) return false;

        var supported = true;
        if (capability.HasFlag(CurrencyCapability.Holding)) supported &= _holdingSet.Contains(currency);
        if (capability.HasFlag(CurrencyCapability.IncomingBc)) supported &= _incomingBcSet.Contains(currency);
        if (capability.HasFlag(CurrencyCapability.FxBc)) supported &= _fxBcSet.Contains(currency);

        return supported;
    }

    /// <summary>
    /// Retrieves a list of currencies that support the specified capabilities.
    /// </summary>
    /// <param name="capability">The capability flags to filter by. If multiple flags are provided, 
    /// only currencies that satisfy ALL flags are returned (Intersection/AND logic).</param>
    /// <returns>A list of <see cref="CurrencyInfo"/> objects for the matching currencies.</returns>
    public IReadOnlyList<CurrencyInfo> GetSupported(CurrencyCapability capability)
    {
        var codes = capability switch
        {
            CurrencyCapability.None => (IEnumerable<CurrencyTypeEnum>) [],
            CurrencyCapability.Holding => _holding,
            CurrencyCapability.IncomingBc => _incomingBc,
            CurrencyCapability.FxBc => _fxBc,
            _ => GetIntersectedCodes(capability)
        };

        return codes
            .Select(GetInfo)
            .ToList();
    }

    private IEnumerable<CurrencyTypeEnum> GetIntersectedCodes(CurrencyCapability capability)
    {
        IEnumerable<CurrencyTypeEnum>? result = null;
        
        if (capability.HasFlag(CurrencyCapability.Holding))
            result = result?.Intersect(_holdingSet) ?? _holding;

        if (capability.HasFlag(CurrencyCapability.IncomingBc))
            result = result?.Intersect(_incomingBcSet) ?? _incomingBc;

        if (capability.HasFlag(CurrencyCapability.FxBc))
            result = result?.Intersect(_fxBcSet) ?? _fxBc;

        return result ?? [];
    }

    public IReadOnlyList<CurrencyInfo> GetAll() =>
        Iso4217CurrencyTable.All
            .Where(x => x.Key != CurrencyTypeEnum.None)
            .Select(x => x.Value)
            .ToList();

    public CurrencyInfo GetInfo(CurrencyTypeEnum currency) =>
        Iso4217CurrencyTable.GetInfo(currency);

    private static IReadOnlyList<CurrencyTypeEnum> BuildUniqueList(string listName, IReadOnlyList<CurrencyTypeEnum>? currencies)
    {
        var source = currencies ?? [];
        var unique = new HashSet<CurrencyTypeEnum>();
        var ordered = new List<CurrencyTypeEnum>();

        foreach (var currency in source)
        {
            ValidateCurrency(listName, currency);

            if (unique.Add(currency))
            {
                ordered.Add(currency);
            }
        }

        return ordered;
    }

    private static void ValidateCurrency(string listName, CurrencyTypeEnum currency)
    {
        if (currency == CurrencyTypeEnum.None)
        {
            throw new InvalidOperationException(
                $"Currency list {listName} contains {CurrencyTypeEnum.None}, which is not valid for capabilities.");
        }

        if (!Enum.IsDefined(currency))
        {
            throw new InvalidOperationException(
                $"Currency list {listName} contains an invalid currency value {currency}.");
        }
    }
}

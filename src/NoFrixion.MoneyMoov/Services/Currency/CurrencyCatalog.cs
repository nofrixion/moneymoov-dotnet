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
    private readonly IReadOnlyList<CurrencyTypeEnum> _inbound;
    private readonly IReadOnlyList<CurrencyTypeEnum> _fxConversion;

    private readonly HashSet<CurrencyTypeEnum> _holdingSet;
    private readonly HashSet<CurrencyTypeEnum> _inboundSet;
    private readonly HashSet<CurrencyTypeEnum> _fxConversionSet;

    public CurrencyCatalog(IOptions<CurrenciesConfiguration> options)
    {
        var config = options?.Value ?? new CurrenciesConfiguration();

        _holding = BuildUniqueList(nameof(config.Holding), config.Holding);
        _inbound = BuildUniqueList(nameof(config.Inbound), config.Inbound);
        _fxConversion = BuildUniqueList(nameof(config.FxConversion), config.FxConversion);

        _holdingSet = _holding.ToHashSet();
        _inboundSet = _inbound.ToHashSet();
        _fxConversionSet = _fxConversion.ToHashSet();
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
        if (capability.HasFlag(CurrencyCapability.Inbound)) supported &= _inboundSet.Contains(currency);
        if (capability.HasFlag(CurrencyCapability.FxConversion)) supported &= _fxConversionSet.Contains(currency);

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
            CurrencyCapability.Inbound => _inbound,
            CurrencyCapability.FxConversion => _fxConversion,
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

        if (capability.HasFlag(CurrencyCapability.Inbound))
            result = result?.Intersect(_inboundSet) ?? _inbound;

        if (capability.HasFlag(CurrencyCapability.FxConversion))
            result = result?.Intersect(_fxConversionSet) ?? _fxConversion;

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

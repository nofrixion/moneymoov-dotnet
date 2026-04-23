//-----------------------------------------------------------------------------
// Filename: CurrencyCatalogTests.cs
// 
// Description: Tests for CurrencyCatalog.
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
using NoFrixion.MoneyMoov.Models.Currency;
using NoFrixion.MoneyMoov.Services.Currency;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Services.Currency;

public class CurrencyCatalogTests
{
    [Fact]
    public void Constructor_Throws_When_Capability_List_Contains_None()
    {
        var config = new CurrenciesConfiguration
        {
            Holding = [CurrencyTypeEnum.None]
        };

        Assert.Throws<InvalidOperationException>(() => new CurrencyCatalog(Options.Create(config)));
    }

    [Fact]
    public void Constructor_Throws_When_Capability_List_Contains_Unknown_Currency()
    {
        var config = new CurrenciesConfiguration
        {
            Holding = [(CurrencyTypeEnum)999]
        };

        Assert.Throws<InvalidOperationException>(() => new CurrencyCatalog(Options.Create(config)));
    }

    [Fact]
    public void IsSupportedFor_Returns_Expected_Results_For_Single_And_Combined_Capabilities()
    {
        var sut = CreateCatalog(
            holding: [CurrencyTypeEnum.EUR, CurrencyTypeEnum.USD],
            incomingBc: [CurrencyTypeEnum.EUR, CurrencyTypeEnum.DKK],
            fxBc: [CurrencyTypeEnum.EUR, CurrencyTypeEnum.DKK]);

        Assert.True(sut.IsSupportedFor(CurrencyTypeEnum.EUR, CurrencyCapability.Holding));
        Assert.True(sut.IsSupportedFor(CurrencyTypeEnum.EUR, CurrencyCapability.Holding | CurrencyCapability.FxBc));
        Assert.False(sut.IsSupportedFor(CurrencyTypeEnum.DKK, CurrencyCapability.Holding));
        Assert.False(sut.IsSupportedFor(CurrencyTypeEnum.DKK, CurrencyCapability.Holding | CurrencyCapability.FxBc));
        Assert.False(sut.IsSupportedFor(CurrencyTypeEnum.EUR, CurrencyCapability.None));
    }

    [Fact]
    public void GetSupported_Returns_Configured_Order_And_Removes_Duplicates()
    {
        var sut = CreateCatalog(
            holding: [CurrencyTypeEnum.USD, CurrencyTypeEnum.EUR, CurrencyTypeEnum.USD]);

        var holding = sut.GetSupported(CurrencyCapability.Holding);

        Assert.Collection(
            holding,
            first => Assert.Equal(CurrencyTypeEnum.USD, first.Code),
            second => Assert.Equal(CurrencyTypeEnum.EUR, second.Code));
    }

    [Fact]
    public void GetSupported_Returns_Currencies_Supporting_Combined_Capabilities_And_Removes_Duplicates()
    {
        var sut = CreateCatalog(
            holding: [CurrencyTypeEnum.EUR, CurrencyTypeEnum.USD, CurrencyTypeEnum.GBP],
            fxBc: [CurrencyTypeEnum.EUR, CurrencyTypeEnum.GBP, CurrencyTypeEnum.DKK]);

        var supported = sut.GetSupported(CurrencyCapability.Holding | CurrencyCapability.FxBc);

        // EUR is in both Holding and FxBc (duplicated in both).
        // GBP is in both Holding and FxBc.
        // USD is only in Holding.
        // DKK is only in FxBc.
        IReadOnlyList<CurrencyInfo> expected = [sut.GetInfo(CurrencyTypeEnum.EUR), sut.GetInfo(CurrencyTypeEnum.GBP)];
        Assert.Equal(expected.OrderBy(x => x.Code), supported.OrderBy(x => x.Code));
    }

    [Fact]
    public void GetAll_Returns_All_Currencies_Except_None()
    {
        var sut = CreateCatalog();
        var all = sut.GetAll();

        Assert.DoesNotContain(all, x => x.Code == CurrencyTypeEnum.None);
        Assert.Equal(Enum.GetValues<CurrencyTypeEnum>().Length - 1, all.Count);
        Assert.Contains(all, x => x.Code == CurrencyTypeEnum.EUR);
        Assert.Contains(all, x => x.Code == CurrencyTypeEnum.BTC);
    }

    private static CurrencyCatalog CreateCatalog(
        IReadOnlyList<CurrencyTypeEnum>? holding = null,
        IReadOnlyList<CurrencyTypeEnum>? incomingBc = null,
        IReadOnlyList<CurrencyTypeEnum>? fxBc = null)
    {
        var config = new CurrenciesConfiguration
        {
            Holding = holding ?? [],
            IncomingBc = incomingBc ?? [],
            FxBc = fxBc ?? []
        };

        return new CurrencyCatalog(Options.Create(config));
    }
}

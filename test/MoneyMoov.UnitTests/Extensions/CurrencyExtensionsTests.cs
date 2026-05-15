//-----------------------------------------------------------------------------
// Filename: CurrencyExtensionsTests.cs
// 
// Description: Tests for CurrencyExtensions.
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

using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Extensions;

public class CurrencyExtensionsTests
{
    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, "EUR")]
    [InlineData(CurrencyTypeEnum.GBP, "GBP")]
    [InlineData(CurrencyTypeEnum.ISK, "ISK")]
    [InlineData(CurrencyTypeEnum.BTC, "BTC")]
    [InlineData(CurrencyTypeEnum.None, "NONE")]
    public void Iso4217AlphaCode_Returns_Expected_Value(CurrencyTypeEnum currency, string expectedCode)
    {
        Assert.Equal(expectedCode, currency.Iso4217AlphaCode());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, "978")]
    [InlineData(CurrencyTypeEnum.GBP, "826")]
    [InlineData(CurrencyTypeEnum.ISK, "352")]
    [InlineData(CurrencyTypeEnum.BTC, "")]
    [InlineData(CurrencyTypeEnum.None, "")]
    public void Iso4217NumericCode_Returns_Expected_Value(CurrencyTypeEnum currency, string expectedCode)
    {
        Assert.Equal(expectedCode, currency.Iso4217NumericCode());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, 2)]
    [InlineData(CurrencyTypeEnum.ISK, 0)]
    [InlineData(CurrencyTypeEnum.BTC, 8)]
    [InlineData(CurrencyTypeEnum.None, 0)]
    public void DefaultDecimals_Returns_Expected_Value(CurrencyTypeEnum currency, int expectedDecimals)
    {
        Assert.Equal(expectedDecimals, currency.DefaultDecimals());
    }

    [Fact]
    public void GetCurrencySymbol_Returns_Legacy_Euro_Fallback_For_Other_Fiat_Currencies()
    {
        Assert.Equal("€", CurrencyTypeEnum.DKK.GetCurrencySymbol());
        Assert.Equal("€", CurrencyTypeEnum.CHF.GetCurrencySymbol());
    }
}

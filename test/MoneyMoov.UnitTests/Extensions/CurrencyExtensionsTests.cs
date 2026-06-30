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
    [InlineData(CurrencyTypeEnum.JPY, "JPY")]
    [InlineData(CurrencyTypeEnum.BTC, "BTC")]
    [InlineData(CurrencyTypeEnum.None, "NONE")]
    public void Iso4217AlphaCode_Returns_Expected_Value(CurrencyTypeEnum currency, string expectedCode)
    {
        Assert.Equal(expectedCode, currency.Iso4217AlphaCode());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, "978")]
    [InlineData(CurrencyTypeEnum.GBP, "826")]
    [InlineData(CurrencyTypeEnum.JPY, "392")]
    [InlineData(CurrencyTypeEnum.BTC, "")]
    [InlineData(CurrencyTypeEnum.None, "")]
    public void Iso4217NumericCode_Returns_Expected_Value(CurrencyTypeEnum currency, string expectedCode)
    {
        Assert.Equal(expectedCode, currency.Iso4217NumericCode());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, 2)]
    [InlineData(CurrencyTypeEnum.JPY, 0)]
    [InlineData(CurrencyTypeEnum.BTC, 8)]
    [InlineData(CurrencyTypeEnum.None, 0)]
    public void DefaultDecimals_Returns_Expected_Value(CurrencyTypeEnum currency, int expectedDecimals)
    {
        Assert.Equal(expectedDecimals, currency.DefaultDecimals());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, "€")]
    [InlineData(CurrencyTypeEnum.GBP, "£")]
    [InlineData(CurrencyTypeEnum.USD, "$")]
    [InlineData(CurrencyTypeEnum.JPY, "¥")]
    [InlineData(CurrencyTypeEnum.CHF, "CHF")]
    [InlineData(CurrencyTypeEnum.None, "?")]
    public void GetCurrencySymbol_Returns_Expected_Value(CurrencyTypeEnum currency, string expectedSymbol)
    {
        Assert.Equal(expectedSymbol, currency.GetCurrencySymbol());
    }

    [Theory]
    [InlineData(CurrencyTypeEnum.EUR, true)]
    [InlineData(CurrencyTypeEnum.GBP, true)]
    [InlineData(CurrencyTypeEnum.USD, true)]
    [InlineData(CurrencyTypeEnum.BTC, false)]
    [InlineData(CurrencyTypeEnum.None, true)]
    public void GetIsFiatCurrency_Returns_Expected_Value(CurrencyTypeEnum currency, bool expectedFiatValue)
    {
        Assert.Equal(expectedFiatValue, currency.IsFiat());
    }
}

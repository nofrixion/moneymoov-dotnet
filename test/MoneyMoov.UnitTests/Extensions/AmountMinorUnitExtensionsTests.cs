//-----------------------------------------------------------------------------
// Filename: AmountMinorUnitExtensionsTests.cs
// 
// Description: Tests for AmountMinorUnitExtensions.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 24 Apr 2025  Aaron Clauson   Created, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Extensions;

public class AmountMinorUnitExtensionsTests
{
    [Theory]
    [InlineData(0.01, CurrencyTypeEnum.EUR, 1)]
    [InlineData(0.011, CurrencyTypeEnum.EUR, 1)]
    [InlineData(0.019, CurrencyTypeEnum.EUR, 1)]
    [InlineData(1000.019, CurrencyTypeEnum.EUR, 100001)]
    [InlineData(1000.019, CurrencyTypeEnum.USD, 100001)]
    [InlineData(1000.019, CurrencyTypeEnum.GBP, 100001)]
    [InlineData(-1000.019, CurrencyTypeEnum.EUR, -100001)]
    [InlineData(-1000.019, CurrencyTypeEnum.USD, -100001)]
    [InlineData(-1000.019, CurrencyTypeEnum.GBP, -100001)]
    [InlineData(1.019, CurrencyTypeEnum.BTC, 101900000)]
    [InlineData(-1.019, CurrencyTypeEnum.BTC, -101900000)]
    [InlineData(1.019011119, CurrencyTypeEnum.BTC, 101901111)]
    public void GetAmountMinorUnits(decimal amount, CurrencyTypeEnum currency, long expectedUnits)
    {
        Assert.Equal(expectedUnits, amount.ToAmountMinorUnits(currency));
    }
}
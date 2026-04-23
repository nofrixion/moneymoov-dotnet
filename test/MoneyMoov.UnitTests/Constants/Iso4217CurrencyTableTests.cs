//-----------------------------------------------------------------------------
// Filename: Iso4217CurrencyTableTests.cs
// 
// Description: Tests for Iso4217CurrencyTable.
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

using NoFrixion.MoneyMoov.Constants;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Constants;

public class Iso4217CurrencyTableTests
{
    [Fact]
    public void All_Has_Entry_For_All_Currency_Enum_Members()
    {
        var enumValues = Enum.GetValues<CurrencyTypeEnum>();

        Assert.Equal(enumValues.Length, Iso4217CurrencyTable.All.Count);

        foreach (var currency in enumValues)
        {
            Assert.True(Iso4217CurrencyTable.All.ContainsKey(currency));
            Assert.Equal(currency, Iso4217CurrencyTable.GetInfo(currency).Code);
        }
    }

    [Fact]
    public void GetInfo_Returns_None_Currency_Info_For_Unknown_Enum_Value()
    {
        var info = Iso4217CurrencyTable.GetInfo((CurrencyTypeEnum)int.MaxValue);

        Assert.Equal(CurrencyTypeEnum.None, info.Code);
        Assert.Equal("NONE", info.Iso4217AlphaCode);
        Assert.Equal(0, info.Decimals);
    }

    [Fact]
    public void GetInfo_Returns_Expected_Decimals_For_Isk_And_Btc()
    {
        Assert.Equal(0, Iso4217CurrencyTable.GetInfo(CurrencyTypeEnum.ISK).Decimals);
        Assert.Equal(PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES, Iso4217CurrencyTable.GetInfo(CurrencyTypeEnum.BTC).Decimals);
    }
}

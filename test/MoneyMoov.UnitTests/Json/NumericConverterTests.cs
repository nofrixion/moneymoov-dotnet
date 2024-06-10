//-----------------------------------------------------------------------------
// Filename:NumericConverterTests.cs
//
// Description: Unit tests for the MoneyMoov Json numeric converter class.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Jun 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class NumericConverterTests
{
    [Theory]
    [InlineData("\"42\"", 42)]
    [InlineData("42", 42)]
    [InlineData("\"\"", 0)]
    //[InlineData("null", 0)]
    public void Deserialize_Int_Success(string json, int expected)
    {
        var result = json.FromJson<int>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"42.42\"", 42.42)]
    [InlineData("42.42", 42.42)]
    [InlineData("\"\"", 0.0)]
    //[InlineData("null", 0.0)]
    public void Deserialize_Double_Success(string json, double expected)
    {
        var result = json.FromJson<double>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"42.42\"", 42.42)]
    [InlineData("42.42", 42.42)]
    [InlineData("\"\"", 0.0)]
    //[InlineData("null", 0.0)]
    public void Deserialize_Decimal_Success(string json, decimal expected)
    {
        var result = json.FromJson<decimal>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(42, "42")]
    [InlineData(0, "0")]
    public void Serialize_Int_Success(int value, string expected)
    {
        var json = value.ToJsonFlat();
        Assert.Equal(expected, json);
    }

    [Theory]
    [InlineData(42.42, "42.42")]
    [InlineData(0.0, "0")]
    public void Serialize_Double_Success(double value, string expected)
    {
        var json = value.ToJsonFlat();
        Assert.Equal(expected, json);
    }

    [Theory]
    [InlineData(42.42, "42.42")]
    [InlineData(0.0, "0")]
    public void Serialize_Decimal_Success(decimal value, string expected)
    {
        var json = value.ToJsonFlat();
        Assert.Equal(expected, json);
    }
}
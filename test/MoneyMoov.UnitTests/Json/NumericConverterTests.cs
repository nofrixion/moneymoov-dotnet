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
    [InlineData("123", 123)]
    [InlineData("\"123\"", 123)]
    public void Deserialise_Int_Success(string json, int expected)
    {
        var result = json.FromJson<int>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234567890123", 1234567890123L)]
    [InlineData("\"1234567890123\"", 1234567890123L)]
    public void Deserialise_Long_Success(string json, long expected)
    {
        var result = json.FromJson<long>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123.45", 123.45f)]
    [InlineData("\"123.45\"", 123.45f)]
    public void Deserialise_Float_Success(string json, float expected)
    {
        var result = json.FromJson<float>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123.456789", 123.456789)]
    [InlineData("\"123.456789\"", 123.456789)]
    public void Deserialise_Double_Success(string json, double expected)
    {
        var result = json.FromJson<double>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123456.789", 123456.789)]
    [InlineData("\"123456.789\"", 123456.789)]
    public void Deserialise_Decimal_Success(string json, decimal expected)
    {
        var result = json.FromJson<decimal>();
        Assert.Equal(expected, result);
    }
}
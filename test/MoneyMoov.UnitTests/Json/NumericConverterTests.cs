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

using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class NumericConverterTests
{
    private readonly ILogger<NumericConverterTests> _logger;
    private LoggerFactory _loggerFactory;

    public NumericConverterTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<NumericConverterTests>();
    }

    [Theory]
    [InlineData("\"42\"", 42)]
    [InlineData("42", 42)]
    public void Deserialize_Int_Success(string json, int expected)
    {
        var result = json.FromJson<int>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"42.42\"", 42.42)]
    [InlineData("42.42", 42.42)]
    public void Deserialize_Double_Success(string json, double expected)
    {
        var result = json.FromJson<double>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"42.42\"", 42.42)]
    [InlineData("42.42", 42.42)]
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
    [InlineData(0.0, "0.0")]
    public void Serialize_Double_Success(double value, string expected)
    {
        var json = value.ToJsonFlat();
        Assert.Equal(expected, json);
    }

    [Theory]
    [InlineData(42.42, "42.42")]
    [InlineData(0.0, "0.0")]
    [InlineData(1, "1.0")]
    public void Serialize_Decimal_Success(decimal value, string expected)
    {
        var json = value.ToJsonFlat();

        _logger.LogDebug("{method} result: {result}", nameof(Serialize_Decimal_Success), json);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void Deserialize_Empty_Int_Exception()
    {
        string json = "\"\"";
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => json.FromJson<int>());
    }

    [Fact]
    public void Deserialize_Empty_Decimal_Exception()
    {
        string json = "\"\"";
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => json.FromJson<decimal>());
    }

    [Fact]
    public void Deserialize_Empty_Float_Exception()
    {
        string json = "\"\"";
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => json.FromJson<float>());
    }

    [Fact]
    public void Deserialize_Empty_Double_Exception()
    {
        string json = "\"\"";
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => json.FromJson<double>());
    }

    [Fact]
    public void Deserialize_Empty_Long_Exception()
    {
        string json = "\"\"";
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => json.FromJson<long>());
    }
}
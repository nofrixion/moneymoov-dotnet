//-----------------------------------------------------------------------------
// Filename: EnumConverterTests.cs
//
// Description: Unit tests for the MoneyMoov Json enumconverter class.
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

using System.Runtime.Serialization;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class EnumConverterTests
{
    public enum TestEnum
    {
        Unknown = 0,

        [EnumMember(Value = "INT_INTERC")]
        INTINTERC = 16,

        [EnumMember(Value = "INCOMING_PAYMENT_PROCESSED")]
        IncomingPaymentProcessed = 1
    }

    private class TestClass
    {
        public TestEnum? Instructedamountcurrency { get; set; }
    }

    [Theory]
    [InlineData("\"INT_INTERC\"", TestEnum.INTINTERC)]
    [InlineData("\"INCOMING_PAYMENT_PROCESSED\"", TestEnum.IncomingPaymentProcessed)]
    [InlineData("\"Unknown\"", TestEnum.Unknown)]
    [InlineData("16", TestEnum.INTINTERC)]
    [InlineData("1", TestEnum.IncomingPaymentProcessed)]
    public void Deserialise_Enum_Success(string json, TestEnum expected)
    {
        var result = json.FromJson<TestEnum>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(TestEnum.INTINTERC, "INT_INTERC")]
    [InlineData(TestEnum.IncomingPaymentProcessed, "INCOMING_PAYMENT_PROCESSED")]
    [InlineData(TestEnum.Unknown, "Unknown")]
    public void Serialise_Enum_Success(TestEnum enumValue, string expected)
    {
        var json = enumValue.ToJsonFlat();
        Assert.Equal($"\"{expected}\"", json);
    }

    [Theory]
    [InlineData("{\"Instructedamountcurrency\":\"\"}", null)]
    [InlineData("{\"Instructedamountcurrency\":\"INT_INTERC\"}", TestEnum.INTINTERC)]
    [InlineData("{\"Instructedamountcurrency\":\"INCOMING_PAYMENT_PROCESSED\"}", TestEnum.IncomingPaymentProcessed)]
    [InlineData("{\"Instructedamountcurrency\":\"Unknown\"}", TestEnum.Unknown)]
    [InlineData("{\"Instructedamountcurrency\":16}", TestEnum.INTINTERC)]
    [InlineData("{\"Instructedamountcurrency\":1}", TestEnum.IncomingPaymentProcessed)]
    public void Deserialise_NullableEnum_Success(string json, TestEnum? expected)
    {
        var result = json.FromJson<TestClass>();
        Assert.NotNull(result);
        Assert.Equal(expected, result.Instructedamountcurrency);
    }

    [Theory]
    [InlineData(null, "{\"instructedamountcurrency\":null}")]
    [InlineData(TestEnum.INTINTERC, "{\"instructedamountcurrency\":\"INT_INTERC\"}")]
    [InlineData(TestEnum.IncomingPaymentProcessed, "{\"instructedamountcurrency\":\"INCOMING_PAYMENT_PROCESSED\"}")]
    [InlineData(TestEnum.Unknown, "{\"instructedamountcurrency\":\"Unknown\"}")]
    public void Serialise_NullableEnum_Success(TestEnum? enumValue, string expected)
    {
        var myClass = new TestClass { Instructedamountcurrency = enumValue };
        var json = myClass.ToJsonFlat();
        Assert.Equal(expected, json);
    }
}
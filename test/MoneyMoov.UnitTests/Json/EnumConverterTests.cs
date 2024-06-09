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
}